using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using SecuringWebApiUsingJwtAuthentication.Entities;
using SecuringWebApiUsingJwtAuthentication.Handlers;
using SecuringWebApiUsingJwtAuthentication.Helpers;
using SecuringWebApiUsingJwtAuthentication.Interfaces;
using SecuringWebApiUsingJwtAuthentication.Requirements;
using SecuringWebApiUsingJwtAuthentication.Services;

namespace SecuringWebApiUsingJwtAuthentication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddDbContext<CustomersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("CustomersDbConnectionString")));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = TokenHelper.Issuer,
                            ValidAudience = TokenHelper.Audience,
                            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(TokenHelper.Secret))
                        };
                        
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyNonBlockedCustomer", policy => {
                    policy.Requirements.Add(new CustomerBlockedStatusRequirement(false));
                
                });
            });

            services.AddSingleton<IAuthorizationHandler, CustomerBlockedStatusHandler>();

            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
