using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.AspireWebComponents_ApiService>("apiservice");

var frontend = builder.AddNpmApp("webfrontend", "../web-frontend", "start")
    .WithReference(apiService)
    .WithHttpEndpoint(port: 8000, env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

if (builder.Environment.IsDevelopment() && builder.Configuration["DOTNET_LAUNCH_PROFILE"] == "https")
{
    // Disable TLS certificate validation in development, see https://github.com/dotnet/aspire/issues/3324 for more details.
    frontend.WithEnvironment("NODE_TLS_REJECT_UNAUTHORIZED", "0");
}

await builder.Build().RunAsync();