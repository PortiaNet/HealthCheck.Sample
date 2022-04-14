using PortiaNet.HealthCheck.Reporter;
using PortiaNet.HealthCheck.Writer;
using PortiaNet.HealthCheck.Writer.HTTP.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSQLServerWriter(options =>
{
    options.TableName = "RequestTracks"; // Default Table Name
    options.MuteOnError = false;
    options.NodeName = "Main Node";
    options.ConnectionString = "data source=127.0.0.1;initial catalog=HealthCheckTest;Trusted_Connection=True;multipleactiveresultsets=True;";
});

builder.Services.AddMongoDBWriter(options =>
{
    options.DatabaseName = "HealthCheck"; // Default Database Name
    options.ConnectionString = "mongodb://localhost:27017";
    options.MuteOnError = false;
    options.CollectionName = "RequestTracks"; // Default Collection Name
    options.NodeName = "Main Node";
});

builder.Services.AddHTTPWriter(options =>
{
    options.ListenerAddress = new Uri("https://localhost:7149/ReportWriterEmulator/SaveReportWithoutAuthentication");
    options.MuteOnError = false;
    options.AuthenticationType = AuthenticationType.None;
    options.NodeName = "Main Node without authentication";
});

builder.Services.AddHTTPWriter(options =>
{
    options.ListenerAddress = new Uri("https://localhost:7149/ReportWriterEmulator/SaveReportByStaticBearerToken");
    options.MuteOnError = false;
    options.AuthenticationType = AuthenticationType.StaticBearerToken;
    options.NodeName = "Main Node by Static Bearer Token";
    options.AuthenticationConfig = new StaticBearerTokenAuthentication
    {
        Token = "A very hard and long super secret token!!!"
    };
});

builder.Services.AddHTTPWriter(options =>
{
    options.ListenerAddress = new Uri("https://localhost:7149/ReportWriterEmulator/SaveReportByClientSecret");
    options.MuteOnError = false;
    options.AuthenticationType = AuthenticationType.ClientSecretBearerToken;
    options.NodeName = "Main Node by ClientSecret";
    options.AuthenticationConfig = new ClientSecretBearerTokenAuthentication
    {
        AuthenticationAPIPath = new Uri("https://localhost:7149/ReportWriterEmulator/AuthenticateByClientSecret"),
        ClientSecret = "***ClientSecretText&&&"
    };
});

builder.Services.AddHTTPWriter(options =>
{
    options.ListenerAddress = new Uri("https://localhost:7149/ReportWriterEmulator/SaveReportByAuthenticateByUsernamePassword");
    options.MuteOnError = false;
    options.AuthenticationType = AuthenticationType.UsernamePasswordBearerToken;
    options.NodeName = "Main Node by Username and Password";
    options.AuthenticationConfig = new UsernamePasswordBearerTokenAuthentication
    {
        AuthenticationAPIPath = new Uri("https://localhost:7149/ReportWriterEmulator/AuthenticateByUsernamePassword"),
        Username = "TestUser",
        Password = "P@ssvor3d"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHealthCheck();

app.Run();
