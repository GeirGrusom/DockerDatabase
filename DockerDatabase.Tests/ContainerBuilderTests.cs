// Original filename: ContainerBuilderTests.cs
// Author: Henning Moe
// This file is licensed under MIT.
// The full license is available under the root in a file names LICENSE.

namespace DockerDatabase.Tests
{
    using System.Threading.Tasks;
    using NUnit.Framework;

    /// <summary>
    /// Container builder tests
    /// </summary>
    [TestFixture]
    public class ContainerBuilderTests
    {
        /// <summary>
        /// This test tries to create a PostgreSql container and check if it exists
        /// </summary>
        /// <returns>Asynchronous task</returns>
        [Test]
        public async Task CreateContainerAsync_PostgreSql_ContainerExists()
        {
            DatabaseContainer container = null;
            try
            {
                container = await DatabaseBuilder.CreateContainerAsync(DatabaseType.PostgreSql);

                Assert.That(container, Container.Exists);
            }
            finally
            {
                if (container != null)
                {
                    await container.StopAsync();
                }
            }
        }

        /// <summary>
        /// This test creates a PotgreSql container and connection and tries to query 'Hello World!' from it
        /// </summary>
        /// <returns>Asynchronous task</returns>
        [Test]
        public async Task CreateContainerAsync_PostgreSql_HelloWorld()
        {
            DatabaseContainer container = null;
            try
            {
                container = await DatabaseBuilder.CreateContainerAsync(DatabaseType.PostgreSql);
                var connection = await container.CreateConnectionAsync<Npgsql.NpgsqlConnection>();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "select 'Hello World!'";
                    var msg = await cmd.ExecuteScalarAsync();
                    Assert.That(msg, Is.EqualTo("Hello World!"));
                }
            }
            finally
            {
                if (container != null)
                {
                    await container.StopAsync();
                }
            }
        }
    }
}
