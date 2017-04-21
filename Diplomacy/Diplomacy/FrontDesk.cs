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
    /// Class responsible for loading and initializing Clerk plugins at run time.
    /// </summary>
    public class FrontDesk
    {
        /// <summary>
        /// Type: <see cref="Boolean"/><para>Indicates if the instance has been initialized.</para>
        /// </summary>
        private bool isInitialized = false;
        /// <summary>
        /// Type: <see cref="List{String}"/><para>The collection of paths that will be used when discovering and loading plugin assemblies.</para>
        /// </summary>
        private List<String> _Paths = new List<string>();
        /// <summary>
        /// Type: <see cref="Dictionary{String,IClerk}"/><para>The collection of loaded and validated Clerk plugins.</para>
        /// </summary>
        private ConcurrentDictionary<string, IClerk> _Clerks = new ConcurrentDictionary<string, IClerk>();
        /// <summary>
        /// Type: <see cref="IEnumerable{String}"/><para>The collection of paths that will be used when discovering and loading plugin assemblies.</para>
        /// </summary>
        public IEnumerable<string> Paths
        {
            get
            {
                return _Paths;
            }
        }
        /// <summary>
        /// Creates a new instance of <see cref="FrontDesk"/>.
        /// </summary>
        /// <param name="paths">Type: <see cref="IEnumerable{String}"/><para>The optional collection of paths/assemblies to use when loading plugin assemblies.</para></param>
        public FrontDesk(IEnumerable<string> paths = null)
        {
            if (paths != null)
            {
                //Add the paths to the collection.
                _Paths.AddRange(paths);
            }

        }
        /// <summary>
        /// Initializes the <see cref="FrontDesk"/> instance, loading viable Clerk plugins and preparing the validated plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the initialization.</para></returns>
        public Result Initialize()
        {
            if (isInitialized)
            {
                //The class has not been initialized.
                return new Result(ResultCode.AlreadyInitialized);
            }
            //Load the clerks associated with the provided paths and current assembly.
            var potentialClerks = LoadClerks();
            foreach (var applicant in potentialClerks)
            {
                //Each clerk needs to be validated before being added to the dictionary.
                var result = ValidateClerk(applicant.Metadata);
                if (result.Code == ResultCode.Success)
                {
                    //Add the validated clerk to the dictionary.
                    _Clerks.TryAdd(applicant.Metadata.ID, applicant.Value);
                }
                else
                {
                    //Log message about invalid plugin.
                }
            }
            isInitialized = true;
            return new Result();
        }
        /// <summary>
        /// Retrieves the <see cref="IClerk"/> instance associated with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The unique ID of the <see cref="IClerk"/> desired.</param>
        /// <returns>Type: <see cref="IClerk"/><para>The instance associated with the <paramref name="id"/>. If not found, NULL will be returned.</para></returns>
        public IClerk GetClerk(string id)
        {
            IClerk value;
            if (_Clerks.TryGetValue(id, out value))
            {
                //THe matching clerk was found.
                return value;
            }
            //No Clerk matches the id.
            return null;
        }
        /// <summary>
        /// Starts all the <see cref="IClerk"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the start operation.</para></returns>
        public Result StartClerks()
        {
            if (!isInitialized)
            {
                //The class has not been initialized.
                return new Result(ResultCode.NotInitialized);
            }
            int count = 0;
            foreach (var clerk in _Clerks.Values)
            {
                //Start each clerk.
                var result = clerk.Start();
                count++; //Increment the clerk count
            }
            if (count <= 0)
            {
                //No Clerks were loaded/validated.
                return new Result(ResultCode.NoPluginsLoaded);
            }
            return new Result();
        }
        /// <summary>
        /// Stops all the <see cref="IClerk"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the stop operation.</para></returns>
        public Result StopClerks()
        {
            if (!isInitialized)
            {
                //Class has not been initialized.
                return new Result(ResultCode.NotInitialized);
            }
            int count = 0;
            foreach (var clerk in _Clerks.Values)
            {
                //Stop each clerk.
                var result = clerk.Stop();
                count++; //Increment the clerk count.
            }
            if (count <= 0)
            {
                //No clerks were found/validated.
                return new Result(ResultCode.NoPluginsLoaded);
            }
            return new Result();
        }
        /// <summary>
        /// Validates the plugin associated with the <paramref name="applicantData"/>.
        /// </summary>
        /// <param name="applicantData">Type: <see cref="IClerkData"/><para>The metadata to use for plugin validation.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the plugin validation.</para></returns>
        protected Result ValidateClerk(IClerkData applicantData)
        {
            if (String.IsNullOrEmpty(applicantData.ID))
            {
                //Clerk ID not provided.
                return new Result(ResultCode.InvalidPluginID);
            }
            if (_Clerks.Keys.Contains(applicantData.ID))
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
            //We'll check for built-in clerks, supplied in the executing assembly.
            AssemblyCatalog aCatalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            catalogs.Add(aCatalog);
            foreach (var path in Paths)
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
            return catalogs;
        }
        /// <summary>
        /// Loads all Clerk plugins from the expected assemblies and <see cref="Paths"/>.
        /// </summary>
        /// <returns>Type: <see cref="IEnumerable{Lazy{IClerk, IClerkData}}"/><para>The list of Clerk plugins loaded from assemblies and paths.</para></returns>
        protected IEnumerable<Lazy<IClerk, IClerkData>> LoadClerks()
        {
            //Build Aggregate Catalog 
            AggregateCatalog combinedCatalog = new AggregateCatalog(GetCatalogs());
            CompositionContainer container = new CompositionContainer(combinedCatalog);
            //Load Clerk plugins
            var candidates = container.GetExports<IClerk, IClerkData>();
            return candidates;
        }
    }
}
