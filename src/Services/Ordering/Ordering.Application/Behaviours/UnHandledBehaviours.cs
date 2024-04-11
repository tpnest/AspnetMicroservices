﻿using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviours;

public class UnHandledBehaviours<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
{
    private readonly  ILogger<TRequest> _logger;

    public UnHandledBehaviours(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        try
        {
            return await next();
        }
        catch (Exception e)
        {
            var requestName = typeof(TRequest).Name;
            _logger.LogError(e, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
            throw;
        }
    }
}