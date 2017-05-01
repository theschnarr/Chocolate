using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplomacy.Kit;
using System.Collections.Concurrent;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.ComponentModel.Composition.Primitives;
using System.Reflection;

namespace Diplomacy
{
    /// <summary>
    /// Handles the loading and routing for all <see cref="IAmbassador"/> plugins.
    /// </summary>
    public class Dispatch
    {
        /// <summary>
        /// Type: <see cref="PluginHandler{IAmbassador, IPluginData}"/><para>Responsible for loading and validating <see cref="IAmbassador"/> plugins.</para>
        /// </summary>
        protected PluginHandler<IAmbassador, IPluginData> _Handler = null;
        /// <summary>
        /// Creates a new instance of <see cref="Dispatch"/>.
        /// </summary>
        /// <param name="paths">Type: <see cref="IEnumerable{String}"/><para>The optional collection of paths/assemblies to use when loading plugin assemblies.</para></param>
        /// <param name="assemblies">Type: <see cref="IEnumerable{Assembly}"/><para>The optional collection of loaded assemblies that should also be scanned for potential plugins.</para></param>
        public Dispatch(IEnumerable<string> paths = null, IEnumerable<Assembly> assemblies = null)
        {
            _Handler = new PluginHandler<IAmbassador, IPluginData>(paths, assemblies);
        }
        /// <summary>
        /// Retrieves the <see cref="IClerk"/> instance associated with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The unique ID of the <see cref="IClerk"/> desired.</param>
        /// <returns>Type: <see cref="IAmbassador"/><para>The instance associated with the <paramref name="id"/>. If not found, NULL will be returned.</para></returns>
        public IAmbassador GetAmbassador(string id)
        {
            return _Handler.GetPlugin(id);
        }
        /// <summary>
        /// Starts all the <see cref="IAmbassador"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the start operation.</para></returns>
        public Result StartAmbassadors()
        {
            return _Handler.StartPlugins();
        }
        /// <summary>
        /// Stops all the <see cref="IAmbassador"/> plugins.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the stop operation.</para></returns>
        public Result StopAmbassadors()
        {
            return _Handler.StopPlugins();
        }
    }
}
