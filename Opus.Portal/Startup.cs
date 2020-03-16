using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Opus.Contracts;
using Opus.Domain.Validators;
using Opus.Portal.Converters;
using Opus.Services;

namespace Opus.Portal
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
            services.AddTransient<ILabourService, LabourService>();
            services.AddTransient<IPricingService, PricingService>();
            services.AddTransient<IJobService, JobService>();

            services.AddTransient<IValidator<CreateJobRequest>, CreateJobValidator>();

            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                {
                    var jobItemConverter = new ObjectFromJsonFieldConverter<IJobItem>("$type")
                        .AddType<TyreReplacement>(Constants.JobItems.TyreReplacementTypeName)
                        .AddType<BrakeDiscReplacement>(Constants.JobItems.BrakeDiscReplacementTypeName)
                        .AddType<BrakePadReplacement>(Constants.JobItems.BrakePadReplacementTypeName)
                        .AddType<ExhaustReplacement>(Constants.JobItems.ExhaustTypeName)
                        .AddType<OilChange>(Constants.JobItems.OilChangeTypeName);

                    options.SerializerSettings.Converters.Add(jobItemConverter);
                });
                
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
