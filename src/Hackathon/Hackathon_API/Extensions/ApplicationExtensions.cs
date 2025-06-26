namespace Hackathon_API.Extensions
{
	public static class ApplicationExtensions
	{
		public static IServiceCollection AddApplicationDatabase(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddDbContext<TimbanbonchanDbContext>(options =>
			//{
			//	options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
			//});

			return services;
		}

		public static IServiceCollection AddApplicationSettings(this IServiceCollection services, IConfiguration configuration)
		{
			// Setting Base URL for CI/CD process
			//services.Configure<ApplicationSettings>(configuration.GetSection("Application"));

			//// Email Setting Configuration 
			//services.Configure<EmailSettingsDto>(configuration.GetSection("EmailSettings"));

			//// Stripe Setting Configuration
			//services.Configure<StripeSettings>(configuration.GetSection("Stripe"));
			//StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];

			return services;
		}
	}
}
