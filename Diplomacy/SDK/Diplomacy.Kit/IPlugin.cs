using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Defines the methods and properties required to implement a plugin for the Diplomacy framework.
    /// </summary>
    public interface IPlugin
    {
        #region Properties
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        string ID { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Starts the plugin, performing necessary tasks required before subsequent processing can occur.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the start request.</para></returns>
        Result Start();
        /// <summary>
        /// Stops the plugin, performing any required clean up tasks.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the stop request.</para></returns>
        Result Stop();
        #endregion
    }
}
