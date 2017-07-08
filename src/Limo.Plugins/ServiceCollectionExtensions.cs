using Microsoft.Extensions.DependencyInjection;

namespace Limo.Plugins
{
    /// <summary>
    /// <see cref="IServiceCollection"/> extension to add and confgure <see cref="PluginsManager{T}"/> to 
    /// </summary>
    public static class ServiceCollectionExtensions
  {
    #region Public Methods
    /// <summary>
    /// Addse <see cref="PluginsManager{T}"/> to <see cref="IServiceCollection"/>
    /// </summary>
    /// <param name="services">service collection to add <see cref="PluginsManager{T}"/></param>
    /// <returns>service collection </returns>
    public static IServiceCollection AddPlugins<T>(this IServiceCollection services)
    {

      services.AddSingleton<IPluginsFinder, FolderPluginsFinder>();
      services.AddSingleton<PluginsManager<T>>();

      return services;
    }

    #endregion Public Methods
  }
}