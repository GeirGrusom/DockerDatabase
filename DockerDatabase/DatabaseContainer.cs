// Original filename: DatabaseContainer.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    using System.Threading.Tasks;

    /// <summary>
    /// Represents a Docker container for a database engine
    /// </summary>
    public abstract class DatabaseContainer
    {
        private readonly DockerContainer container;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseContainer"/> class.
        /// </summary>
        /// <param name="container">Container for this database</param>
        /// <param name="type">Type of database this container represents</param>
        internal DatabaseContainer(DockerContainer container, DatabaseType type)
        {
            this.container = container;
            this.Type = type;
        }

        /// <summary>
        /// Gets the Id associated with this container
        /// </summary>
        public string Id => this.container.Id;

        /// <summary>
        /// Gets the database type this container was built for
        /// </summary>
        public DatabaseType Type { get; }

        /// <summary>
        /// Gets the connection string for this <see cref="DatabaseContainer"/>.
        /// </summary>
        /// <returns>Connection string to database</returns>
        public abstract string CreateConnectionString();

        /// <summary>
        /// Gets the connection string for this <see cref="DatabaseContainer"/>.
        /// </summary>
        /// <param name="parameters">Database to create connection string for</param>
        /// <returns>Connection string to database</returns>
        public abstract string CreateConnectionString(ConnectionStringParameters parameters);

        /// <summary>
        /// Gets the connection string for this <see cref="DatabaseContainer"/>.
        /// </summary>
        /// <param name="parameters">Database to create connection string for</param>
        /// <returns>Connection string to database</returns>
        public abstract string CreateConnectionString(AuthorizedConnectionStringParameters parameters);

        /// <summary>
        /// Stops the container asynchronously
        /// </summary>
        /// <returns>Asynchronous task</returns>
        public Task StopAsync()
        {
            return this.container.StopAsync();
        }
    }
}
