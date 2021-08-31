using Data;
using LoymaxTest.Properties;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Services.Accounts;
using Services.Accounts.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;

namespace LoymaxTest
{
    public class Startup
    {
        private readonly string _connectionString;

        public Startup(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection(ConfigurationKeyConstants.ConnectionString).Value;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LoymaxTestContext>(options => options.UseSqlServer(_connectionString));

            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<IAccountValidator, AccountValidator>();
            services.AddScoped<ITransactionValidator, TransactionValidator>();

            services.AddAutoMapper(typeof(Startup));

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "LoymaxTest API V1");
                });

                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
