// Original filename: ConnectionStringParameters.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    /// <summary>
    /// Specifies connection string parameters using a database name
    /// </summary>
    public sealed class ConnectionStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionStringParameters"/> class.
        /// </summary>
        /// <param name="database">Database to use in the connection</param>
        public ConnectionStringParameters(string database)
        {
            this.Database = database;
        }

        /// <summary>
        /// Gets the database to open the connection with
        /// </summary>
        public string Database { get; }
    }
}
