using System;
using BencoPracticeTransitions.Email;
using BencoPracticeTransitions.Infrastructure.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using reCAPTCHA.AspNetCore;
using RazorLight;
using Mindscape.Raygun4Net.AspNetCore;

namespace BencoPracticeTransitions
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
            services.AddOptions();
            services.Configure<BencoSmtpSettings>(Configuration.GetSection("BencoSmtpSettings"));

            services.Configure<BencoEmailMessageSettings>(Configuration.GetSection("BencoEmailMessageSettings"));

            services.AddSingleton<ISendEmail, EmailSender>();
            services.AddSingleton<IGenerateEmail, JobListingInquireEmailGenerator>();
            services.AddSingleton<IGenerateEmail, JobListingCreateEmailGenerator>();
            services.AddSingleton<IGenerateEmail, PracticeSellEmailGenerator>();
            services.AddSingleton<IGenerateEmail, PracticeBuyEmailGenerator>();
            services.AddSingleton<IGenerateEmail, ContactUsEmailGenerator>();

            services.AddSingleton<IGenerateEmailAttachment, EmailAttachmentGenerator>();
            services.AddSingleton<IValidateResume, ResumeValidator>();

            services.AddSingleton<IRazorLightEngine>(serviceProvider =>
                new RazorLightEngineBuilder()
                    .UseEmbeddedResourcesProject(typeof(Program))
                    .UseMemoryCachingProvider()
                    .Build());

            if (Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT") == null) // Kestrel sets this, IIS does not 
            {
                services.AddHttpsRedirection(options =>
                {
                    options.HttpsPort = 443;
                });
            }
            services.AddRaygun(Configuration);

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.Configure<RecaptchaSettings>(Configuration.GetSection("ReCaptchaSettings"));
            services.AddTransient<IRecaptchaService, RecaptchaService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            if (Configuration.GetValue<bool>("RaygunSettings:EnableCommunication"))
            {
                app.UseRaygun();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseStaticFiles();
        }
    }
}
