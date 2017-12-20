// Original filename: DatabaseContainerExtensions.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    using System;
    using System.Data;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using Npgsql;

    /// <summary>
    /// Extension functions for <see cref="DatabaseContainer"/>.
    /// </summary>
    public static class DatabaseContainerExtensions
    {
        /// <summary>
        /// Creates a connection to the database container using default credentials.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IDbConnection"/> to create</typeparam>
        /// <param name="container">Container to build connection to</param>
        /// <returns>Connection to the container</returns>
        public static Task<T> CreateConnectionAsync<T>(this DatabaseContainer container)
            where T : IDbConnection
        {
            return CreateConnectionAsync<T>(container, container.CreateConnectionString());
        }

        /// <summary>
        /// Creates a connection to the database container using the specified <paramref name="connectionStringParameters"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IDbConnection"/> to create</typeparam>
        /// <param name="container">Container to build connection to</param>
        /// <param name="connectionStringParameters">Connection string parameters to use</param>
        /// <returns>Connection to the container</returns>
        public static Task<T> CreateConnectionAsync<T>(this DatabaseContainer container, ConnectionStringParameters connectionStringParameters)
            where T : IDbConnection
        {
            return CreateConnectionAsync<T>(container, container.CreateConnectionString(connectionStringParameters));
        }

        /// <summary>
        /// Creates a connection to the database container using the specified <paramref name="connectionStringParameters"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IDbConnection"/> to create</typeparam>
        /// <param name="container">Container to build connection to</param>
        /// <param name="connectionStringParameters">Connection string parameters to use</param>
        /// <returns>Connection to the container</returns>
        public static Task<T> CreateConnectionAsync<T>(this DatabaseContainer container, AuthorizedConnectionStringParameters connectionStringParameters)
            where T : IDbConnection
        {
            return CreateConnectionAsync<T>(container, container.CreateConnectionString(connectionStringParameters));
        }

        /// <summary>
        /// Creates a conenction to the database container
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IDbConnection"/> to create</typeparam>
        /// <param name="container">Container to build connection to</param>
        /// <param name="connectionString">Connection string to use</param>
        /// <returns><see cref="IDbConnection"/> to the container</returns>
        private static async Task<T> CreateConnectionAsync<T>(this DatabaseContainer container, string connectionString)
            where T : IDbConnection
        {
            if (typeof(T) == typeof(NpgsqlConnection))
            {
                if (container.Type != DatabaseType.PostgreSql)
                {
                    throw new ArgumentException("Container was not built for PostgreSQL.", nameof(container));
                }

                var connString = container.CreateConnectionString();
                var result = new NpgsqlConnection(connString);

                int tries = 0;
                while (tries++ < 10)
                {
                    try
                    {
                        await result.OpenAsync();
                        return (T)(object)result;
                    }
                    catch (NpgsqlException ngex) when (ngex.InnerException is System.IO.IOException)
                    {
                        // Postgres will close the stream for some reason when it's not ready
                        await Task.Delay(500);
                    }
                    catch (SocketException)
                    {
                        // If the container is not ready for connections it will get a connection refused.
                        await Task.Delay(500);
                    }
                }
            }

            throw new NotSupportedException();
        }
    }
}
