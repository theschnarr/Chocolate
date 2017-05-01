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
        /// Type: <see cref="PluginHandler{IClerk, IPluginData}"/><para>Responsible for loading and validating <see cref="IClerk"/> plugins.</para>
        /// </summary>
        protected PluginHandler<IClerk, IPluginData> _Handler = null;
        /// <summary>
        /// Creates a new instance of <see cref="FrontDesk"/>.
        /// </summary>
        /// <param name="paths">Type: <see cref="IEnumerable{String}"/><para>The optional collection of paths/assemblies to use when loading plugin assemblies.</para></param>
        /// <param name="assemblies">Type: <see cref="IEnumerable{Assembly}"/><para>The optional collection of loaded assemblies that should also be scanned for potential plugins.</para></param>
        public FrontDesk(IEnumerable<string> paths = null, IEnumerable<Assembly> assemblies = null)
        {
            _Handler = new PluginHandler<IClerk, IPluginData>(paths, assemblies);
        }
        /// <summary>
        /// Retrieves the <see cref="IClerk"/> instance associated with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The unique ID of the <see cref="IClerk"/> desired.</param>
        /// <returns>Type: <see cref="IClerk"/><para>The instance associated with the <paramref name="id"/>. If not found, NULL will be returned.</para></returns>
        public IClerk GetClerk(string id)
        {
            return _Handler.GetPlugin(id);
        }
        /// <summary>
        /// Starts all the <see cref="IClerk"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the start operation.</para></returns>
        public Result StartClerks()
        {
            return _Handler.StartPlugins();
        }
        /// <summary>
        /// Stops all the <see cref="IClerk"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the stop operation.</para></returns>
        public Result StopClerks()
        {
            return _Handler.StopPlugins();
        }
    }
}
