using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Limo.Plugins
{

    /// <summary>
    /// Manages plugins of specified base type
    /// </summary>
    /// <typeparam name="T">Base type of plugins tu manage</typeparam>
    public class PluginsManager<T>
    {

        #region Public Properties

        /// <summary>
        /// The list of collected plugins.
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
        /// <param name="pluginsFinder">Plugins finder to use to find available plugins</param>
        /// <param name="order">Comparision object used to sort plugin list</param>
        public PluginsManager(ILogger<PluginsManager<T>> logger, IConfiguration configuration, IServiceProvider serviceProvider,
          IPluginsFinder pluginsFinder, Comparison<T> order = null)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
            this.configuration = configuration;
            this.pluginsFinder = pluginsFinder;


            plugins = CreatePlugins();
            Configure(order);

            Plugins = new ReadOnlyCollection<T>(plugins);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Configure PluginsManager
        /// </summary>
        /// <param name="order">Comparision object used to sort plugins</param>
        public void Configure(Comparison<T> order)
        {
            this.order = order;
            if (order != null)
            {
                plugins.Sort(order);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private List<T> CreatePlugins()
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
        Comparison<T> order;
        private List<T> plugins;
        IPluginsFinder pluginsFinder;

        #endregion Private Fields
    }
}