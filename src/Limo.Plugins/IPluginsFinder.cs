using System;
using System.Collections.Generic;
using System.Text;

namespace Limo.Plugins
{
  public interface IPluginsFinder
  {
    IEnumerable<Type> FindPlugins<T>();
  }
}
