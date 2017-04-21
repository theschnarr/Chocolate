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
    public interface IClerk
    {
        #region Properties
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        string ID { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Starts the clerk plugin, performing necessary tasks required before subsequent processing can occur.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the start request.</para></returns>
        Result Start();
        /// <summary>
        /// Sets the consul the Clerk plugin should use when submitting requests for processing.
        /// </summary>
        /// <param name="consul">Type: <see cref="IConsulRequest"/><para>The instance of the request interface the plugin should use for submit requests.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the consul set request.</para></returns>
        Result SetConsul(IConsulRequest consul);
        /// <summary>
        /// Stops the clerk plugin, performing any required clean up tasks.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the stop request.</para></returns>
        Result Stop();
        #endregion
    }
    /// <summary>
    /// Defines the properties associated with <see cref="IClerk"/> metadata for discovery/export.
    /// </summary>
    public interface IClerkData
    {
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        string ID { get; }
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the provider of the associated <see cref="IClerk"/> plugin.</para>
        /// </summary>
        string ProviderID { get; }
    }
}
