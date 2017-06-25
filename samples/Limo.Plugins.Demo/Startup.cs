using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Limo.Plugins.Demo
{
  public class Startup
  {
    IConfiguration configuration;
    public Startup(IHostingEnvironment env)
    {
      var builder = new ConfigurationBuilder()
            .SetBasePath(env.ContentRootPath)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
            .AddEnvironmentVariables();

      builder.AddEnvironmentVariables();
      configuration = builder.Build();
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddSingleton<IConfiguration>(configuration);
      services.AddPlugins();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IServiceProvider services)
    {
      loggerFactory.AddConsole();

      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      var manager = services.GetService<PluginsManager>();

      app.Run(async (context) =>
      {
        var count = manager.Plugins.Count;
        var plugins = string.Join(Environment.NewLine, manager.Plugins.Select(p => p.ToString()));
        await context.Response.WriteAsync($"Hello {count} Plugins:{Environment.NewLine}{plugins}");
      });
    }
  }
}
