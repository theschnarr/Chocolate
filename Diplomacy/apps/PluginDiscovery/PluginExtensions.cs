using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Contains IPlugin, and related collection, extension methods used with various demonstrations in this project.
    /// </summary>
    public static class PluginExtensions
    {
        /// <summary>
        /// Iterates through the contents of the <paramref name="collection"/>, calling <see cref="PrintPlugin(IPlugin)"/> for each item.
        /// </summary>
        /// <param name="collection">Type: <see cref="IEnumerable{IPlugin}"/><para>The collection of <see cref="IPlugin"/> instances to be printed.</para></param>
        public static void PrintPlugins(this IEnumerable<IPlugin> collection)
        {
            foreach (var plugin in collection)
            {
                PrintPlugin(plugin);
            }
        }
        /// <summary>
        /// Runs a simulation of the <paramref name="plugin"/> lifecycle, printing the status and results to the console.
        /// </summary>
        /// <param name="plugin">Type: <see cref="IPlugin"/><para>The plugin that is to be processed.</para></param>
        public static void PrintPlugin(this IPlugin plugin)
        {
            if (plugin == null)
            {
                Console.WriteLine("Null plugin provided encountered.");
                return;
            }
            Console.WriteLine("Loaded plugin " + plugin.ID);
            Console.WriteLine("Starting plugin " + plugin.ID);
            var result = plugin.Start();
            Console.WriteLine("Plugin Started: " + result.Code);
            Console.WriteLine("Stopping plugin " + plugin.ID);
            result = plugin.Stop();
            Console.WriteLine("Plugin Stopped: " + result.Code);
        }
        /// <summary>
        /// Iterates through the contents of the <paramref name="collection"/>, calling <see cref="PrintPlugin(IPlugin)"/> for each item's Value.
        /// </summary>
        /// <param name="collection">Type: <see cref="IEnumerable{Lazy{IPlugin}}"/><para>The collection of <see cref="Lazy{IPlugin}"/> instances to be printed.</para></param>
        public static void PrintPlugins(this IEnumerable<Lazy<IPlugin>> collection)
        {
            foreach (var lazyPlugin in collection)
            {
                PrintPlugin(lazyPlugin.Value);
            }
        }
    }
}
