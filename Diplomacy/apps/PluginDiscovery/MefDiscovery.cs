using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery
{
    /// <summary>
    /// Class used to demonstrate "plugin discovery" using Managed Entity Framework (MEF).
    /// </summary>
    public static class MefDiscovery
    {
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/> from the provided <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Type: <see cref="Assembly"/><para>The .NET Assembly to check for viable plugin types.</para></param>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <returns>Type: <see cref="IEnumerable{T}"/><para>The collection of viable plugin instances.</para></returns>
        public static IEnumerable<T> LoadPlugins<T>(Assembly assembly)
        {
            //We need to make sure that a null parameter hasn't been passed in.
            if (assembly != null)
            {
                //Build AssemblyCatalog 
                AssemblyCatalog aCatalog = new AssemblyCatalog(assembly);
                CompositionContainer container = new CompositionContainer(aCatalog);
                //Load plugins
                var candidates = container.GetExports<T>();
                //Plugins are loaded into Lazy instances, which defers creation of the plugin instances until actually accessed.
                //For this method, we'll simply use LINQ to access and return each plugin instance, using the Lazy<T>.Value property.
                return candidates.Select(i => i.Value);
            }
            //The assembly was not valid, no plugins can be returned.
            return null;
        }
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/> from the provided <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <param name="path">Type: <see cref="String"/><para>The full path of the directory to load plugins from.</para></param>
        /// <returns></returns>
        public static IEnumerable<T> LoadPlugins<T>(string path)
        {
            //We need to make sure that a null/empty parameter has not been passed in.  Additionally, we want to make sure that the provided path actually exists.
            if (!String.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                //Build DirectoryCatalog
                DirectoryCatalog dCatalog = new DirectoryCatalog(path);
                CompositionContainer container = new CompositionContainer(dCatalog);
                //Load plugins
                var candidates = container.GetExports<T>();
                //Plugins are loaded into Lazy instances, which defers creation of the plugin instances until actually accessed.
                //For this method, we'll simply use LINQ to access and return each plugin instance, using the Lazy<T>.Value property.
                return candidates.Select(i => i.Value);
            }
            //The provided path was not valid or did not exist, no plugins can be returned.
            return null;
        }
        
    }
}
