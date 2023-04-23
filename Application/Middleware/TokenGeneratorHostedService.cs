using Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class TokenGeneratorHostedService : IHostedService
{
    private readonly TokenManager _tokenManager;

    public TokenGeneratorHostedService(TokenManager tokenManager)
    {
        _tokenManager = tokenManager;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // Generate a token during application startup
        var token = _tokenManager.GenerateToken();

        // Other initialization tasks

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        // Clean up tasks

        return Task.CompletedTask;
    }
}
