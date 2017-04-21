using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy
{
    /// <summary>
    /// Defines the methods and properties required to control the initialization and shutdown of a consul.
    /// </summary>
    public interface IConsulControl
    {
        #region Methods
        /// <summary>
        /// Starts the Consul for request processing.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the Start method.</para></returns>
        Result Start();
        /// <summary>
        /// Stops the Consul, disposing of any objects/handles.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the Stop method.</para></returns>
        Result Stop();
        #endregion
    }
}
