# .NET Core plugins framework 

## Features

- Use any type as base for plugins
- Supports multipe plugin groups (base types)
- Use DI to create plugins

## Quick Start

```csharp
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddSingleton<IConfiguration>(configuration);
      services.AddPlugins();
    }
```


```csharp
      app.Run(async (context) =>
      {
        var count = manager.Plugins.Count;
        var plugins = string.Join(Environment.NewLine, manager.Plugins.Select(p => p.ToString()));
        await context.Response.WriteAsync($"Hello {count} Plugins:{Environment.NewLine}{plugins}");
      });
```
```javascript
{
  "Plugins": {
    "Path": "/../Extensions"
  }
}
```
