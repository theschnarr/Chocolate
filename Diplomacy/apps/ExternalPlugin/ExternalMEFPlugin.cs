using Diplomacy.Kit;
using Diplomacy.Kit.MEF;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalPlugin
{
    /// <summary>
    /// Used for plugin demonstration via MEF.
    /// </summary>
    [ExportPlugin("ExternalMefPlugin", "DiscoveryDemo")]
    //Identifies this plugin as exporting the IPlugin interface, 
    //while using the ID and the Provider as metadata.
    public class ExternalMefPlugin : IPlugin
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExternalMefPlugin"/>.
        /// </summary>
        public ExternalMefPlugin()
        {
            Console.WriteLine("New instance of ExternalMefPlugin created.");
        }
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID of the plugin.</para>
        /// </summary>
        public string ID
        {
            get
            {
                return "ExternalMefPlugin";
            }
        }
        /// <summary>
        /// Starts the plugin, writing a string to the console before returning a success.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Returns the default <see cref="Result"/> instance.</para></returns>
        public Result Start()
        {
            Console.WriteLine("[MEF] " + ID + " is starting up...");
            return new Result();
        }
        /// <summary>
        /// Stops the plugin, writing a string to the console before returning a success.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Returns the default <see cref="Result"/> instance.</para></returns>
        public Result Stop()
        {
            Console.WriteLine("[MEF] " + ID + " is stopping...");
            return new Result();
        }
    }
}
