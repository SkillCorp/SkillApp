using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using SkillApp.Data;
using SkillApp.DependencyResolution;
using SkillApp.Entities.Entities;
using SkillApp.WebApi.Options;


namespace SkillApp.WebApi
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        private const string SECRET_KEY = "needtogetthisfromenvironment";
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SECRET_KEY));

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                //builder.AddUserSecrets();

                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyDbContext>(
                options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    builderOptions => builderOptions.MigrationsAssembly("SkillApp.Data")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyDbContext>()
                .AddDefaultTokenProviders();

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddCustomRepositories();
            services.AddCustomServices();

            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("AdminUser", "AdminUser"));
            });

            ConfigureIssuerOptions(services);
        }
        private void ConfigureIssuerOptions(IServiceCollection services)
        {
            var issuerOptionsConfig = Configuration.GetSection(nameof(JwtIssuerOptions));
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = issuerOptionsConfig[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = issuerOptionsConfig[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();

            app.UseJwtBearerAuthentication(GetBearerOptions());

            app.UseIdentity();
            app.UseMvc();
        }
        private JwtBearerOptions GetBearerOptions()
        {
            return new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = GetTokenValidationParameters()
            };
        }
        private TokenValidationParameters GetTokenValidationParameters()
        {
            var issuerOptionsConfig = Configuration.GetSection(nameof(JwtIssuerOptions));
            return new TokenValidationParameters
            {

                ValidateIssuer = true,
                ValidIssuer = issuerOptionsConfig[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = issuerOptionsConfig[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
