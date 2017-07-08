using System;
using System.Collections.Generic;
using System.Text;

namespace Limo.Plugins
{
    /// <summary>
    /// Interface for plugin finders
    /// </summary>
    public interface IPluginsFinder
    {
        /// <summary>
        /// Finds and returns list of available plugins
        /// </summary>
        /// <typeparam name="T">Plugin type to search for</typeparam>
        /// <returns>List of plugins</returns>
        IEnumerable<Type> FindPlugins<T>();
    }
}
