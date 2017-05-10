using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Console.WriteLine("Loading local plugin(s) by reflection...");
                List<IPlugin> loadedPlugins = new List<IPlugin>();
                /*var plugins = ReflectionDiscovery.LoadPlugins<IPlugin>(typeof(Program).Assembly);
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
                foreach (var plugin in loadedPlugins)
                {
                    Console.WriteLine("Loaded plugin " + plugin.ID);
                    Console.WriteLine("Starting plugin " + plugin.ID);
                    var result = plugin.Start();
                    Console.WriteLine("Plugin Started: " + result.Code);
                    Console.WriteLine("Stopping plugin " + plugin.ID);
                    result = plugin.Stop();
                    Console.WriteLine("Plugin Stopped: " + result.Code);
                }*/
                loadedPlugins.Clear();
                Console.WriteLine("Loading local plugin(s) by MEF/Export...");
                var plugins = MefDiscovery.LoadPlugins<IPlugin>(typeof(Program).Assembly);
                if (plugins == null)
                {
                    Console.WriteLine("No MEF plugins found in the currently executing assembly.");
                }
                else
                {
                    Console.WriteLine(plugins.Count().ToString() + " found in the current assembly.");
                    loadedPlugins.AddRange(plugins);
                }
                Console.WriteLine("Loading MEF plugins from Plugins directory: " + Environment.CurrentDirectory + "\\Plugins");
                plugins = MefDiscovery.LoadPlugins<IPlugin>(Environment.CurrentDirectory + "\\Plugins");
                if (plugins == null)
                {
                    Console.WriteLine("No plugins found in the Plugins directory.");
                }
                else
                {
                    Console.WriteLine(plugins.Count().ToString() + " found in the Plugins directory.");
                    loadedPlugins.AddRange(plugins);
                }
                foreach (var plugin in loadedPlugins)
                {
                    Console.WriteLine("Loaded plugin " + plugin.ID);
                    Console.WriteLine("Starting plugin " + plugin.ID);
                    var result = plugin.Start();
                    Console.WriteLine("Plugin Started: " + result.Code);
                    Console.WriteLine("Stopping plugin " + plugin.ID);
                    result = plugin.Stop();
                    Console.WriteLine("Plugin Stopped: " + result.Code);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press <Enter> to Exit");
            Console.ReadLine();
        }
    }
}
