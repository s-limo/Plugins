using Limo.Plugins.Abstractions;
using System;

namespace Limo.Plugins.Demo.Plugin2
{
    public class PluginTwo : IPlugin
    {
        public int Priority => 1;
    }
}
