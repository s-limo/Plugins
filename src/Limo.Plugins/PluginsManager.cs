using Limo.Plugins.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Limo.Plugins
{
  /// <summary>
  /// Plugins manager manages plugins implementing <see cref="IPlugin"/> interface
  /// </summary>
  public class PluginsManager : PluginsManager<IPlugin>
  {

    #region Public Constructors

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    /// <param name="serviceProvider"></param>
    public PluginsManager(ILogger<PluginsManager> logger, IConfiguration configuration, IServiceProvider serviceProvider,
      IPluginsFinder pluginsFinder)
      : base(logger, configuration, serviceProvider, pluginsFinder)
    {
    }

    #endregion Public Constructors

  }

  /// <summary>
  /// Manages plugins of specified base type
  /// </summary>
  /// <typeparam name="T">Base type of plugins tu manage</typeparam>
  public class PluginsManager<T>
  {

    #region Public Properties

    /// <summary>
    /// The list of collected plygins.
    /// </summary>
    public IReadOnlyCollection<T> Plugins { get; private set; } = new List<T>();

    #endregion Public Properties

    #region Public Constructors

    /// <summary>
    /// Constructor. Mostly likely it will be called from DI framework.
    /// </summary>
    /// <param name="logger">logger where information must be printed</param>
    /// <param name="configuration"></param>
    /// <param name="serviceProvider">This service provider will be used to find services required by service plygins</param>
    public PluginsManager(ILogger<PluginsManager<T>> logger, IConfiguration configuration, IServiceProvider serviceProvider, 
      IPluginsFinder pluginsFinder)
    {
      this.logger = logger;
      this.serviceProvider = serviceProvider;
      this.configuration = configuration;
      this.pluginsFinder = pluginsFinder;

      plugins = CreatePlugins();
      Plugins = new ReadOnlyCollection<T>(plugins);
    }

    #endregion Public Constructors

    #region Private Methods

    private IList<T> CreatePlugins()
    {
      List<T> instances = new List<T>();

      foreach (Type type in pluginsFinder.FindPlugins<T>())
      {
        if (!type.GetTypeInfo().IsAbstract)
        {
          T instance = (T)ActivatorUtilities.CreateInstance(serviceProvider, type, new object[] { });

          instances.Add(instance);
        }
      }

      return instances;
    }
    #endregion Private Methods

    #region Private Fields

    private readonly IServiceProvider serviceProvider;
    private IConfiguration configuration;
    private ILogger<PluginsManager<T>> logger;
    private IList<T> plugins;
    IPluginsFinder pluginsFinder;

    #endregion Private Fields

  }
}