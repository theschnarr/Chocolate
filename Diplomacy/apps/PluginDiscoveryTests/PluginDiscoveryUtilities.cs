using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscoveryTests
{
    public class PluginDiscoveryUtilities
    {
        /// <summary>
        /// Static method used to grab the relative path of the Directory used for testing directory-based plugin discovery.
        /// </summary>
        /// <returns>Type: <see cref="String"/><para>The path of our DirectoryDiscovery folder, relative to the currently UnitTest project.</para></returns>
        public static string GetPluginDirectoryPath()
        {
            return Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Environment.CurrentDirectory)))) + "\\Tests\\Plugins\\DirectoryDiscovery";
        }
    }
}
