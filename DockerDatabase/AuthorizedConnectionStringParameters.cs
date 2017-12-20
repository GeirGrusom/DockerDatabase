// Original filename: AuthorizedConnectionStringParameters.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase
{
    /// <summary>
    /// Specifies connection string parameters for opening a connection with a database, username and password.
    /// </summary>
    public sealed class AuthorizedConnectionStringParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizedConnectionStringParameters"/> class.
        /// </summary>
        /// <param name="database">Specifies the database to use for a connection string</param>
        /// <param name="username">Specifies the username to use for a connection string</param>
        /// <param name="password">Specifies the password to use for a connection string</param>
        public AuthorizedConnectionStringParameters(string database, string username, string password)
        {
            this.Database = database;
            this.Username = username;
            this.Password = password;
        }

        /// <summary>
        /// Gets the database used for this connection string
        /// </summary>
        public string Database { get; }

        /// <summary>
        /// Gets the username used for this connection string
        /// </summary>
        public string Username { get; }

        /// <summary>
        /// Gets the password used for this connection string
        /// </summary>
        public string Password { get; }
    }
}
