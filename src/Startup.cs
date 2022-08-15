using UrlApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UrlApi
{
    public class Startup
  {
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<AppDataContext>();
      services.AddScoped<IUrlRepository, UrlRepository>();
      services.AddScoped<IKeyRepository, KeyRepository>();
      services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(cfg =>
      {
        cfg.MapControllers();
      });
    }
  }
}
