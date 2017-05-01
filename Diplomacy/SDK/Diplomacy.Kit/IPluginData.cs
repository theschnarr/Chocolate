using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Defines the properties associated with <see cref="IPlugin"/> metadata for discovery/export.
    /// </summary>
    public interface IPluginData
    {
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        string ID { get; }
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the provider of the associated plugin.</para>
        /// </summary>
        string ProviderID { get; }
    }
}
