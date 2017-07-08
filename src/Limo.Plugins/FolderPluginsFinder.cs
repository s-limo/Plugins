using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Limo.Plugins
{
    /// <summary>
    /// IPluginsFinder implementation which serached for plugins in defined folder. Folder is defined
    /// as Plugins:Path key in configuration
    /// </summary>
    public class FolderPluginsFinder : IPluginsFinder
    {
        #region Public Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public FolderPluginsFinder(IConfiguration configuration, ILogger<FolderPluginsFinder> logger)
        {
            this.logger = logger;

            var key = "Plugins:Path";

            this.path = configuration?[key];
            if (path == null)
            {
                logger.LogWarning($"Folder path was not found. Pleas add configuration with key '{key}'");
            }
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Searches for available plugins
        /// </summary>
        /// <typeparam name="T">Base plugin type to look for</typeparam>
        /// <returns>List of available plugins</returns>
        public IEnumerable<Type> FindPlugins<T>()
        {
            var folder = Directory.GetCurrentDirectory() + path;
            List<Type> implementations = new List<Type>();

            foreach (Assembly assembly in GetAssembliesFromPath(folder))
                foreach (Type type in assembly.GetTypes())
                    if (typeof(T).GetTypeInfo().IsAssignableFrom(type) && type.GetTypeInfo().IsClass)
                    {
                        implementations.Add(type);
                    }

            return implementations;

        }

        #endregion Public Methods

        #region Private Methods

        private IEnumerable<Assembly> GetAssembliesFromPath(string path)
        {
            List<Assembly> assemblies = new List<Assembly>();

            if (!Directory.Exists(path))
            {
                logger.LogWarning($"Path '{path}' was not found");
            }
            else
            {
                logger.LogInformation($"Loading assemblies from path '{path}'");

                foreach (string file in Directory.EnumerateFiles(path, "*.dll"))
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file);
                        assemblies.Add(assembly);
                        logger.LogInformation($"Loaded: '{assembly.FullName}'");
                    }
                    catch (Exception e)
                    {
                        logger.LogWarning(0, e, $"Error loading assembly '{file}'");
                    }
                }
            }

            return assemblies;
        }

        #endregion Private Methods

        #region Private Fields

        private ILogger<FolderPluginsFinder> logger;
        String path;

        #endregion Private Fields
    }
}

