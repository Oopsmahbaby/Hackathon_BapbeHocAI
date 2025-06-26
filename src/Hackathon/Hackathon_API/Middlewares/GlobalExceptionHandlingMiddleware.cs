using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Hackathon_API.Middlewares
{
	public class GlobalExceptionHandlingMiddleware : IMiddleware
	{
		private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
		public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
		{
			_logger = logger;
		}
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Unhandled exception caught in middleware.");

				if (!context.Response.HasStarted)
				{
					var problemDetails = new ProblemDetails
					{
						Status = (int)HttpStatusCode.InternalServerError,
						Type = "https://httpstatuses.com/500",
						Title = "An unexpected error occurred and be caught in middleware.",
						Detail = ex.Message
					};

					var json = JsonSerializer.Serialize(problemDetails);

					context.Response.StatusCode = problemDetails.Status.Value;
					context.Response.ContentType = "application/json";
					await context.Response.WriteAsync(json);
				}
				else
				{
					_logger.LogWarning("The response has already started, the error handling middleware will not write to the response.");
				}
			}
		}
	}
}
