using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Limo.Plugins.Abstractions
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
}
