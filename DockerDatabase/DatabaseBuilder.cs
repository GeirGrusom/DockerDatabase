// Original filename: DatabaseBuilder.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// This class is used to create database connections
    /// </summary>
    public static class DatabaseBuilder
    {
        /// <summary>
        /// Creates an instance of <see cref="IDbConnection"/>
        /// </summary>
        /// <param name="type">Type of database to create container for</param>
        /// <returns>SQL Connection</returns>
        public static async Task<DatabaseContainer> CreateContainerAsync(DatabaseType type)
        {
            DockerContainerBuilder builder;
            if (type == DatabaseType.PostgreSql)
            {
                builder = new DockerContainerBuilder(
                    "healthcheck/postgres",
                    "latest",
                    5432,
                    "POSTGRES_USER=postgres",
                    "POSTGRES_PASSWORD=qwerty12345");

                var container = await builder.Start();
                return new PostgresDatabaseContainer(container);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}
