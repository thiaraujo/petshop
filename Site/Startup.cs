using System;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Middleware.Converters.Interface;
using Middleware.Converters.Service;
using Middleware.Email;
using Middleware.IoC;

namespace Site
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.LoginPath = "/Acesso/Acessar";
                options.AccessDeniedPath = "/Acesso/Acessar";
                options.ExpireTimeSpan = TimeSpan.FromDays(15);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            // Seta as urls para lowercase
            services.AddRouting(options => options.LowercaseUrls = true);
            // Registra as dependências
            RegisterContainer.RegisterDependencies(services);
            // Cria uma instância e faz injecção para as toastr
            services.AddScoped<IToastrMensagem, ToastrMensagem>();

            // Inicia o serviço de envio de email
            try
            {
                var servicoEmail = new VerificaAgendamento();
            }
            catch (Exception)
            {
                //error
            }
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
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Caixa}/{action=Registro}/{id?}");
            });
        }
    }
}
