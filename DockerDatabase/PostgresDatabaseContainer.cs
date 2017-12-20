// Original filename: PostgresDatabaseContainer.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    /// <summary>
    /// Represents a PostgreSQL database container
    /// </summary>
    internal sealed class PostgresDatabaseContainer : DatabaseContainer
    {
        /// <summary>
        /// Provides default connection string parameters
        /// </summary>
        internal static readonly AuthorizedConnectionStringParameters DefaultConnectionStringParameters = new AuthorizedConnectionStringParameters("postgres", "postgres", "qwerty12345");

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgresDatabaseContainer"/> class.
        /// </summary>
        /// <param name="container">Container to wrap</param>
        public PostgresDatabaseContainer(DockerContainer container)
            : base(container, DatabaseType.PostgreSql)
        {
        }

        /// <summary>
        /// Creates a connection string using 'postgres' database, 'postgres' user and password 'qerty12345'
        /// </summary>
        /// <returns>Connection string</returns>
        public override string CreateConnectionString()
        {
            return this.CreateConnectionString(DefaultConnectionStringParameters);
        }

        /// <inheritdoc/>
        public override string CreateConnectionString(ConnectionStringParameters parameters)
        {
            return this.CreateConnectionString(new AuthorizedConnectionStringParameters(parameters.Database, "postgres", "qwerty12345"));
        }

        /// <inheritdoc/>
        public override string CreateConnectionString(AuthorizedConnectionStringParameters parameters)
        {
            var builder = new Npgsql.NpgsqlConnectionStringBuilder()
            {
                Host = "127.0.0.1",
                Port = 5432,
                Username = parameters.Username,
                Password = parameters.Password,
                Database = parameters.Database,
                KeepAlive = 200000,
                TcpKeepAliveTime = 200000,
            };

            return builder.ToString();
        }
    }
}
