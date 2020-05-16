using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QuickBuy.Dominio.Contratos;
using QuickBuy.Repositorio.Contexto;
using QuickBuy.Repositorio.Repositorios;

namespace QuickBuy.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder();
            //optional false pra q esse arquivo sempre seja referenciado
            //e reloadOnChange q quando for indentificado alguma coisa nessa nesse config.json ele vai ser regarregado por causa da chave true
            builder.AddJsonFile("config.json", optional:false, reloadOnChange:true);
            Configuration = builder.Build(); //builder.Build() construi uma interface de configuração com as chaves e valor setadas em cima (optional e reloadOnChange) 
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //GetConnectionString ta percorendo o arquivo de configuração dentro da estrutura config.json o par chaveValor onde a chave eh QuickBuyDB e retorna a string de conexao
            var connectionString = Configuration.GetConnectionString("QuickBuyDB");
            //UseLazyLoadingProxies permite o carregamento automatico de relacionamento entre as classes de configuração
            services.AddDbContext<QuickBuyContexto>(option => option.UseLazyLoadingProxies()
                                .UseMySql(connectionString, m => m.MigrationsAssembly("QuickBuy.Repositorio")));

            services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
            //services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
            //services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("https://localhost:44302/");
                }
            });
        }
    }
}
