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
            //We'll get all the Lazy plugins, and use Linq to Transform them to a flat IEnumerable<T>.
            var lazyCandidates = LoadLazyPlugins<T>(assembly);
            //We need to make sure that the Lazy load method found plugins, before performing the transform call with Select.
            return lazyCandidates != null ? lazyCandidates.Select(i => i.Value) : null;
        }
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/> from the provided <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <param name="path">Type: <see cref="String"/><para>The full path of the directory to load plugins from.</para></param>
        /// <returns>Type: <see cref="IEnumerable{T}"/><para>The collection of viable plugin instances.</para></returns>
        public static IEnumerable<T> LoadPlugins<T>(string path)
        {
            //We'll get all the Lazy plugins, and use Linq to Transform them to a flat IEnumerable<T>.
            var lazyCandidates = LoadLazyPlugins<T>(path);
            //We need to make sure that the Lazy load method found plugins, before performing the transform call with Select.
            return lazyCandidates != null ? lazyCandidates.Select(i => i.Value) : null;
        }
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/> from the provided <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">Type: <see cref="Assembly"/><para>The .NET Assembly to check for viable plugin types.</para></param>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <returns>Type: <see cref="IEnumerable{Lazy{T}}"/><para>The collection of lazy plugin instances.</para></returns>
        public static IEnumerable<Lazy<T>> LoadLazyPlugins<T>(Assembly assembly)
        {
            //We need to make sure that a null parameter hasn't been passed in.
            if (assembly != null)
            {
                //Build AssemblyCatalog 
                AssemblyCatalog aCatalog = new AssemblyCatalog(assembly);
                CompositionContainer container = new CompositionContainer(aCatalog);
                //Load plugins
                var candidates = container.GetExports<T>();
                return candidates;
            }
            //The assembly was not valid, no plugins can be returned.
            return Enumerable.Empty<Lazy<T>>();
        }
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/> from the provided <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <param name="path">Type: <see cref="String"/><para>The full path of the directory to load plugins from.</para></param>
        /// <returns>Type: <see cref="IEnumerable{Lazy{T}}"/><para>The collection of lazy plugin instances.</para></returns>
        public static IEnumerable<Lazy<T>> LoadLazyPlugins<T>(string path)
        {
            //We need to make sure that a null/empty parameter has not been passed in.  Additionally, we want to make sure that the provided path actually exists.
            if (!String.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                //Build DirectoryCatalog
                DirectoryCatalog dCatalog = new DirectoryCatalog(path);
                CompositionContainer container = new CompositionContainer(dCatalog);
                //Load plugins
                return container.GetExports<T>();
            }
            //The provided path was not valid or did not exist, no plugins can be returned.
            return Enumerable.Empty<Lazy<T>>();
        }
        /// <summary>
        /// Load plugins, and corresponding metadata, of type <typeparamref name="TPlugin"/> from the provided <paramref name="assembly"/>.
        /// </summary>
        /// <typeparam name="TPlugin">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <typeparam name="TPluginMetadata">Identifies the interface type that is being used to create/interact with plugin metadata.</typeparam>
        /// <param name="assembly">Type: <see cref="Assembly"/><para>The .NET Assembly to check for viable plugin types.</para></param>
        /// <returns>Type: <see cref="IEnumerable{Lazy{TPlugin,TPluginMetadata}}"/><para>The collection of lazy plugin instances and matching metadata.</para></returns>
        public static IEnumerable<Lazy<TPlugin, TPluginMetadata>> LoadPluginsWithMetadata<TPlugin, TPluginMetadata>(Assembly assembly)
        {
            //We need to make sure that a null parameter hasn't been passed in.
            if (assembly != null)
            {
                //Build AssemblyCatalog 
                AssemblyCatalog aCatalog = new AssemblyCatalog(assembly);
                CompositionContainer container = new CompositionContainer(aCatalog);
                //Load plugins
                var candidates = container.GetExports<TPlugin, TPluginMetadata>();
                return candidates;
            }
            //The assembly was not valid, no plugins can be returned.
            return Enumerable.Empty<Lazy<TPlugin, TPluginMetadata>>();
        }
        /// <summary>
        /// Load plugins, and corresponding metadata, of type <typeparamref name="TPlugin"/> from the provided <paramref name="path"/>.
        /// </summary>
        /// <typeparam name="TPlugin">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <typeparam name="TPluginMetadata">Identifies the interface type that is being used to create/interact with plugin metadata.</typeparam>
        /// <param name="path">Type: <see cref="String"/><para>The full path of the directory to load plugins from.</para></param>
        /// <returns>Type: <see cref="IEnumerable{Lazy{TPlugin,TPluginMetadata}}"/><para>The collection of lazy plugin instances and matching metadata.</para></returns>
        public static IEnumerable<Lazy<TPlugin, TPluginMetadata>> LoadPluginsWithMetadata<TPlugin, TPluginMetadata>(string path)
        {
            //We need to make sure that a null parameter hasn't been passed in.
            if (!String.IsNullOrWhiteSpace(path) && Directory.Exists(path))
            {
                //Build DirectoryCatalog
                DirectoryCatalog dCatalog = new DirectoryCatalog(path);
                CompositionContainer container = new CompositionContainer(dCatalog);
                //Load plugins
                var candidates = container.GetExports<TPlugin, TPluginMetadata>();
                return candidates;
            }
            //The assembly was not valid, no plugins can be returned.
            return Enumerable.Empty<Lazy<TPlugin, TPluginMetadata>>();
        }
    }
}
