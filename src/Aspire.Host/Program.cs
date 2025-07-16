
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// the PostgreSQL database

var dockerCompose = builder.AddDockerComposeEnvironment("docker-compose");
#pragma warning disable ASPIRECOMPUTE001
var postgresUsername = builder.AddParameter("postgres-username", secret: true);
var postgresPassword = builder.AddParameter("postgres-password", secret: true);
var postgres = builder.AddPostgres("postgres", postgresUsername, postgresPassword, 5435)
                      .WithComputeEnvironment(dockerCompose)
                      .WithPgAdmin(pgAdmin => pgAdmin.WithHostPort(7087))
                      .WithPgWeb(pgWeb => pgWeb.WithHostPort(7085))
                      .WithLifetime(ContainerLifetime.Persistent);
var mysqldb = postgres.AddDatabase("OCBlazorMeadowDb");

// the Azure Storage emulator
var storage = builder.AddAzureStorage("storage").RunAsEmulator(azurite =>
{
    azurite.WithComputeEnvironment(dockerCompose)
        .WithLifetime(ContainerLifetime.Persistent)
        .WithBlobPort(10000)
        .WithQueuePort(10001)
        .WithTablePort(10002);
});

var blobs = storage.AddBlobs("blobs");
var container = blobs.AddBlobContainer("shells");

// the admin password
var adminPassword = builder.AddParameter("admin-password", secret: true);

builder.AddProject<OrchardCore_App>("App", "https")
    .WithComputeEnvironment(dockerCompose)
    .WithEnvironment("OrchardCore__OrchardCore_DataProtection_Azure__ConnectionString", blobs)
    .WithEnvironment("OrchardCore__OrchardCore_Shells_Azure__ConnectionString", blobs)
    .WithEnvironment("OrchardCore__OrchardCore_AutoSetup__Tenants__0__DatabaseConnectionString", mysqldb)
    .WithEnvironment("OrchardCore__OrchardCore_AutoSetup__Tenants__0__AdminPassword", adminPassword)
    .WaitFor(container)
    .WaitFor(mysqldb);

#pragma warning restore ASPIRECOMPUTE001

builder.Build().Run();