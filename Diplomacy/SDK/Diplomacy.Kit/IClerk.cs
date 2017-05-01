using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Defines the methods and properties required to implement a clerk plugin for the Diplomacy framework.
    /// </summary>
    public interface IClerk: IPlugin
    {
        #region Methods
        /// <summary>
        /// Sets the consul the Clerk plugin should use when submitting requests for processing.
        /// </summary>
        /// <param name="consul">Type: <see cref="IConsulRequest"/><para>The instance of the request interface the plugin should use for submit requests.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the consul set request.</para></returns>
        Result SetConsul(IConsulRequest consul);
        #endregion
    }
}
