using Diplomacy.Kit;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy
{
    /// <summary>
    /// Contains the methods for discovering, loading, validating, and controlling <typeparamref name="TPlugin">plugins</typeparamref>.
    /// </summary>
    /// <typeparam name="TPlugin">Type: <typeparamref name="TPlugin"/><para>The plugins handled by this class.</para></typeparam>
    /// <typeparam name="TPluginData">Type: <typeparamref name="TPluginData"/><para>The plugin metadata used during discovery and validation.</para></typeparam>
    public class PluginHandler<TPlugin, TPluginData> where TPlugin : IPlugin
                                                where TPluginData : IPluginData
    {
        /// <summary>
        /// Type: <see cref="List{String}"/><para>The collection of paths that will be used when discovering and loading plugin assemblies.</para>
        /// </summary>
        protected List<String> _Paths = new List<string>();
        /// <summary>
        /// Type: <see cref="List{Assembly}"/><para>The collection of loaded <see cref="Assembly"/> instances that should be scanned for plugins.</para>
        /// </summary>
        protected List<Assembly> _Assemblies = new List<Assembly>();  
        /// <summary>
        /// Type: <see cref="Dictionary{String,TPlugin}"/><para>The collection of loaded and validated <typeparamref name="TPlugin">plugins</typeparamref>.</para>
        /// </summary>
        protected ConcurrentDictionary<string, TPlugin> _Plugins = new ConcurrentDictionary<string, TPlugin>();
        /// <summary>
        /// Creates a new instance of <see cref="PluginHandler{TPlugin,TPluginData}"/>.
        /// </summary>
        /// <param name="paths">Type: <see cref="IEnumerable{String}"/><para>The optional collection of paths to folders and/or assemblies to use when loading plugin assemblies.</para></param>
        /// <param name="assemblies">Type: <see cref="IEnumerable{Assembly}"/><para>The optional collection of loaded assemblies that should also be scanned for potential plugins.</para></param>
        public PluginHandler(IEnumerable<string> paths = null, IEnumerable<Assembly> assemblies = null)
        {
            //Boolean flag to update when optional params were provided.
            bool collectionsUpdated = false;
            if (paths != null)
            {
                //Add the paths to the collection.
                _Paths.AddRange(paths);
                //Update collectionUpdated flag.
                collectionsUpdated = true;
            }
            if (assemblies != null)
            {
                //Add the loaded assemblies to the collection
                _Assemblies.AddRange(assemblies);
                //Update collectionUpdated flag.
                collectionsUpdated = true;
            }
            if (collectionsUpdated)
            {
                //Collections have been updated using the optional ctor params, we can do a preliminary load call to setup the handler.
                var res = LoadPlugins();
                //TODO:  We should add some logging here to capture any errors that occur during the LoadPlugins call.
            }
        }
        /// <summary>
        /// Adds <paramref name="paths"/> to the collection of directory paths to scan for plugins.
        /// </summary>
        /// <param name="paths">Type: <see cref="string[]"/><para>The path(s) to add.</para></param>
        public void AddPaths(params string[] paths)
        {
            _Paths.AddRange(paths);
        }
        /// <summary>
        /// Adds <paramref name="assemblies"/> to the collection of <see cref="Assembly">assemblies</see> to check for plugins.
        /// </summary>
        /// <param name="assemblies">Type: <see cref="Assembly[]"/><para>The assemblies to add.</para></param>
        public void AddAssemblies(params Assembly[] assemblies)
        {
            _Assemblies.AddRange(assemblies);
        }
        /// <summary>
        /// Loads viable <typeparamref name="TPlugin">plugins</typeparamref> and preparing the validated plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the load operation.</para></returns>
        public virtual Result LoadPlugins()
        {
            //Load the plugins associated with the provided paths and current assembly.
            var potentialPlugins = GetCatalogExports();
            foreach (var applicant in potentialPlugins)
            {
                //Each plugin needs to be validated before being added to the dictionary.
                var result = ValidatePlugin(applicant.Metadata);
                if (result.Code == ResultCode.Success)
                {
                    //Add the validated plugin to the dictionary.
                    _Plugins.TryAdd(applicant.Metadata.ID, applicant.Value);
                }
                else
                {
                    //Log message about invalid plugin.
                }
            }
            return new Result();
        }
        /// <summary>
        /// Retrieves the <typeparamref name="TPlugin"/> instance associated with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The unique ID of the <typeparamref name="TPlugin"/> desired.</param>
        /// <returns>Type: <typeparamref name="TPlugin"/><para>The instance associated with the <paramref name="id"/>. If not found, NULL will be returned.</para></returns>
        public virtual TPlugin GetPlugin(string id)
        {
            TPlugin value;
            if (_Plugins.TryGetValue(id, out value))
            {
                //The matching plugin was found.
                return value;
            }
            //No plugin matches the id.
            return default(TPlugin);
        }
        /// <summary>
        /// Starts all the <typeparamref name="TPlugin"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the start operation.</para></returns>
        public virtual Result StartPlugins()
        {
            int count = 0;
            foreach (var plugin in _Plugins.Values)
            {
                //Start each plugin.
                var result = plugin.Start();
                count++; //Increment the plugin count
            }
            if (count <= 0)
            {
                //No plugins were loaded/validated.
                return new Result(ResultCode.NoPluginsLoaded);
            }
            return new Result();
        }
        /// <summary>
        /// Stops all the <see cref="TPlugin"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the stop operation.</para></returns>
        public virtual Result StopPlugins()
        {
            int count = 0;
            foreach (var plugin in _Plugins.Values)
            {
                //Stop each plugin.
                var result = plugin.Stop();
                count++; //Increment the plugin count.
            }
            if (count <= 0)
            {
                //No plugins were found/validated.
                return new Result(ResultCode.NoPluginsLoaded);
            }
            return new Result();
        }
        /// <summary>
        /// Validates the plugin associated with the <paramref name="applicantData"/>.
        /// </summary>
        /// <param name="applicantData">Type: <typeparamref name="TPluginData"/><para>The metadata to use for plugin validation.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the plugin validation.</para></returns>
        protected Result ValidatePlugin(TPluginData applicantData)
        {
            if (String.IsNullOrEmpty(applicantData.ID))
            {
                //Clerk ID not provided.
                return new Result(ResultCode.InvalidPluginID);
            }
            if (_Plugins.Keys.Contains(applicantData.ID))
            {
                //Clerk ID already registerred.
                return new Result(ResultCode.InvalidPluginDuplicateID);
            }
            if (!ValidateProvider(applicantData.ProviderID))
            {
                //Provider ID is not valid.
                return new Result(ResultCode.InvalidPluginProvider);
            }
            //TODO: Validate Assembly with Provider ID
            return new Result();
        }
        /// <summary>
        /// Validates the Provider <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The unique ID associated with the Provider to be validated.</para></param>
        /// <returns>Type: <see cref="Boolean"/><para>Indicates if the <paramref name="id"/> is valid.</para></returns>
        protected bool ValidateProvider(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                //No ID provided.
                return false;
            }
            //TODO: Provider ID validation
            return true;
        }
        /// <summary>
        /// Retrieves the catalogs that will be used for discovering plugins.
        /// </summary>
        /// <returns>Type: <see cref="IEnumerable{ComposablePartCatalog}"/><para>The list of catalogs based on current assembly, and <see cref="Paths"/>.</para></returns>
        protected IEnumerable<ComposablePartCatalog> GetCatalogs()
        {
            List<ComposablePartCatalog> catalogs = new List<ComposablePartCatalog>();
            //We'll check for built-in plugins, supplied in the executing assembly.
            AssemblyCatalog aCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            catalogs.Add(aCatalog);
            foreach (var path in _Paths)
            {
                if (path.EndsWith(".dll"))
                {
                    //Assembly, we need to verify that the file exists, and then create a new catalog.
                    if (File.Exists(path))
                    {
                        AssemblyCatalog aCat = new AssemblyCatalog(path);
                        catalogs.Add(aCat);
                    }
                }
                else
                {
                    //Directory, we need to verify that the directory exists, and then create a new catalog.
                    if (Directory.Exists(path))
                    {
                        DirectoryCatalog dCat = new DirectoryCatalog(path);
                        catalogs.Add(dCat);
                    }
                }
            }
            foreach (var assembly in _Assemblies)
            {
                if (assembly != null)
                {
                    catalogs.Add(new AssemblyCatalog(assembly));
                }
            }
            return catalogs;
        }
        /// <summary>
        /// Loads all plugins from the expected assemblies and <see cref="Paths"/>.
        /// </summary>
        /// <returns>Type: <see cref="IEnumerable{Lazy{TPlugin, TPluginData}}"/><para>The list of Clerk plugins loaded from assemblies and paths.</para></returns>
        protected IEnumerable<Lazy<TPlugin, TPluginData>> GetCatalogExports()
        {
            //Build Aggregate Catalog 
            AggregateCatalog combinedCatalog = new AggregateCatalog(GetCatalogs());
            CompositionContainer container = new CompositionContainer(combinedCatalog);
            //Load plugins
            var candidates = container.GetExports<TPlugin, TPluginData>();
            return candidates;
        }
    }
}
