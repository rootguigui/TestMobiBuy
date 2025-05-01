using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Localization;
using TestMobiBuy.Api.Configuration;

[assembly: RootNamespace("TestMobiBuy.Api")]

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencyInjection(builder.Configuration);

var app = builder.Build();

app.UseMiddleware(builder.Configuration);

app.Run();

[ExcludeFromCodeCoverage]

#pragma warning disable CA1050
public partial class Program { }
#pragma warning restore CA1050
