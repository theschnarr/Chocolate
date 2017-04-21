using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Defines the methods and properties associated with submitting Diplomacy process requests.
    /// </summary>
    public interface IConsulRequest
    {
        #region Methods
        /// <summary>
        /// Submits a request for processing in the Diplomacy framework.
        /// </summary>
        /// <param name="routeInfo">Type: <see cref="RouteInfo"/><para>Details the target ambassador(s) and processing operation type(s) required to fully process a request.</para></param>
        /// <param name="requestData">Type: <see cref="String"/><para>Contains the data and settings associated with the request.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the success/failure of the request.</para></returns>
        Result ProcessRequest(RouteInfo routeInfo, string requestData);
        #endregion
    }
}
