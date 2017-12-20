# DockerDatabase

This project aims to provide a simple solution for testing database integration using docker.

## Example

Currently PostgreSql is the only supported database.

### PostgreSQL

```csharp
container = await DatabaseBuilder.CreateContainerAsync(DatabaseType.PostgreSql);

using(var connection = container.CreateConnectionAsync<NpgsqlConnection>())
{
    // The connection should be ready to use inside this block.
}

await container.StopAsync();
```

Specifying database to connect to:

```csharp
container = await DatabaseBuilder.CreateContainerAsync(DatabaseType.PostgreSql);

using(var connection = container.CreateConnectionAsync<NpgsqlConnection>(new ConnectionStringParameters(database: "postgres")))
{
    // The connection should be ready to use inside this block.
    // It has connected to the database postgres
    // using 'postgres': 'qwerty12345' as credentials
}

await container.StopAsync();
```

Specifying database and credentials to connect with

```csharp
container = await DatabaseBuilder.CreateContainerAsync(DatabaseType.PostgreSql);

using(var connection = container.CreateConnectionAsync<NpgsqlConnection>(new AuthorizedConnectionStringParameters(database: "postgres", username: "postgres", password: "qwerty12345")))
{
    // The connection should be ready to use inside this block.
    // It has connected to the database specified
    // using the credentials specified
}

await container.StopAsync();
```