using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery
{
    /// <summary>
    /// Contains all of the various demonstration methods for quick use.
    /// </summary>
    public static class Demos
    {
        /// <summary>
        /// Runs a demonstration of plugin discovery using .NET Reflection fuctionality.
        /// </summary>
        public static void RunReflectionDemo()
        {
            Console.WriteLine("Loading local plugin(s) by reflection...");
            List<IPlugin> loadedPlugins = new List<IPlugin>();
            var plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(typeof(Program).Assembly);
            if (plugins == null)
            {
                Console.WriteLine("No plugins found in the currently executing assembly.");
            }
            else
            {
                Console.WriteLine(plugins.Count().ToString() + " found in the current assembly:");
                loadedPlugins.AddRange(plugins);
            }
            Console.WriteLine("Loading plugins from Plugins directory: " + Environment.CurrentDirectory + "\\Plugins");
            plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(Environment.CurrentDirectory + "\\Plugins");
            if (plugins == null)
            {
                Console.WriteLine("No plugins found in the Plugins directory.");
            }
            else
            {
                Console.WriteLine(plugins.Count().ToString() + " found in the Plugins directory.");
                loadedPlugins.AddRange(plugins);
            }
            loadedPlugins.PrintPlugins();
            return;
        }
        /// <summary>
        /// Runs a demonstration using MEF and non-Lazy plugin discovery.
        /// </summary>
        public static void RunMefDemo()
        {
            List<IPlugin> loadedPlugins = new List<IPlugin>();
            Console.WriteLine("Loading local plugin(s) by MEF/Export...");
            var plugins = MefDiscovery.LoadPlugins<IPlugin>(typeof(Program).Assembly);
            loadedPlugins.AddRange(plugins);
            Console.WriteLine(plugins.Count() + " found in the " + typeof(Program).Assembly.GetName().Name + " Assembly");
            string path = Environment.CurrentDirectory + "\\Plugins";
            Console.WriteLine("Loading MEF plugins from Plugins directory: " + path);
            plugins = MefDiscovery.LoadPlugins<IPlugin>(path);
            Console.WriteLine(plugins.Count() + " found in the Plugins directory.");
            loadedPlugins.AddRange(plugins);
            loadedPlugins.PrintPlugins();
            return;
        }
        /// <summary>
        /// Runs a demonstration using MEF and Lazy plugin discovery.
        /// </summary>
        public static void RunLazyMefDemo()
        {
            List<Lazy<IPlugin>> lazyPlugins = new List<Lazy<IPlugin>>();
            Console.WriteLine("Loading local plugin(s) by MEF/Export...");
            var plugins = MefDiscovery.LoadLazyPlugins<IPlugin>(typeof(Program).Assembly);
            Console.WriteLine(plugins.Count() + " found in the " + typeof(Program).Assembly.GetName().Name + " Assembly");
            lazyPlugins.AddRange(plugins);
            string path = Environment.CurrentDirectory + "\\Plugins";
            Console.WriteLine("Loading MEF plugins from Plugins directory: " + path);
            plugins = MefDiscovery.LoadLazyPlugins<IPlugin>(path);
            Console.WriteLine(plugins.Count() + " found in the Plugins directory.");
            lazyPlugins.AddRange(plugins);
            lazyPlugins.PrintPlugins();
        }
        /// <summary>
        /// Runs a demonstration using MEF, which targets a specific plugin to use.
        /// </summary>
        public static void RunSpecificPluginDemo()
        {
            List<Lazy<IPlugin>> lazyPlugins = new List<Lazy<IPlugin>>();
            Console.WriteLine("Loading local plugin(s) by MEF/Export...");
            var plugins = MefDiscovery.LoadLazyPlugins<IPlugin>(typeof(Program).Assembly);
            Console.WriteLine(plugins.Count() + " found in the " + typeof(Program).Assembly.GetName().Name + " Assembly");
            lazyPlugins.AddRange(plugins);
            string path = Environment.CurrentDirectory + "\\Plugins";
            Console.WriteLine("Loading MEF plugins from Plugins directory: " + path);
            plugins = MefDiscovery.LoadLazyPlugins<IPlugin>(path);
            Console.WriteLine(plugins.Count() + " found in the Plugins directory.");
            lazyPlugins.AddRange(plugins);
            var plugin = lazyPlugins.Where(i => i.Value.ID == "ExternalMefPlugin").Select(i => i.Value).SingleOrDefault();
            plugin.PrintPlugin();
        }
        /// <summary>
        /// Runs a demonstration using MEF and plugin metadata to maximize the efficiency of plugin loading and initialization.
        /// </summary>
        public static void RunSpecificPluginWithMetadataDemo()
        {
            List<Lazy<IPlugin, IPluginData>> lazyPlugins = new List<Lazy<IPlugin, IPluginData>>();
            Console.WriteLine("Loading local plugin(s) by MEF/Export...");
            var plugins = MefDiscovery.LoadPluginsWithMetadata<IPlugin, IPluginData>(typeof(Program).Assembly);
            Console.WriteLine(plugins.Count() + " found in the " + typeof(Program).Assembly.GetName().Name + " Assembly");
            lazyPlugins.AddRange(plugins);
            string path = Environment.CurrentDirectory + "\\Plugins";
            Console.WriteLine("Loading MEF plugins from Plugins directory: " + path);
            plugins = MefDiscovery.LoadPluginsWithMetadata<IPlugin, IPluginData>(path);
            Console.WriteLine(plugins.Count() + " found in the Plugins directory.");
            lazyPlugins.AddRange(plugins);
            var plugin = lazyPlugins.Where(i => i.Metadata.ID == "ExternalMefPlugin").Select(i => i.Value).SingleOrDefault();
            plugin.PrintPlugin();
        }
    }
}
