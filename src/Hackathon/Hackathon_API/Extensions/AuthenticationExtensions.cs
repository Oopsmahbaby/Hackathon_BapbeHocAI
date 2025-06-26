using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Hackathon_API.Extensions
{
	public static class AuthenticationExtensions
	{
		public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
		{
			var issuer = configuration["Jwt:Issuer"];
			var audience = configuration["Jwt:Audience"];
			var key = configuration["Jwt:Key"];

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = issuer,
						ValidAudience = audience,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
					};

					options.Events = new JwtBearerEvents
					{
						OnAuthenticationFailed = context =>
						{
							var response = new
							{
								Status = (int)HttpStatusCode.Unauthorized,
								Title = "Authentication Failed",
								Message = context.Exception.Message
							};

							context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
							context.Response.ContentType = "application/json";
							return context.Response.WriteAsync(JsonSerializer.Serialize(response));
						},
						OnChallenge = context =>
						{
							context.HandleResponse(); // Stop default behavior

							var response = new
							{
								Status = (int)HttpStatusCode.Unauthorized,
								Title = "Unauthorized",
								Message = "You are not authorized to access this resource."
							};

							context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
							context.Response.ContentType = "application/json";
							return context.Response.WriteAsync(JsonSerializer.Serialize(response));
						},
						OnForbidden = context =>
						{
							var response = new
							{
								Status = (int)HttpStatusCode.Forbidden,
								Title = "Forbidden",
								Message = "You do not have permission to access this resource."
							};

							context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
							context.Response.ContentType = "application/json";
							return context.Response.WriteAsync(JsonSerializer.Serialize(response));
						}
					};
				});

			return services;
		}
	}
}
