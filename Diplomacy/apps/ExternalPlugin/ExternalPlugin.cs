using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalPlugin
{
    /// <summary>
    /// Used for plugin demonstration via Reflection.
    /// </summary>
    public class ExternalPlugin: IPlugin
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExternalPlugin"/>.
        /// </summary>
        public ExternalPlugin()
        {
            Console.WriteLine("New instance of ExternalPlugin created.");
        }
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID of the plugin.</para>
        /// </summary>
        public string ID
        {
            get
            {
                return "ExternalPlugin";
            }
        }
        /// <summary>
        /// Starts the plugin, writing a string to the console before returning a success.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Returns the default <see cref="Result"/> instance.</para></returns>
        public Result Start()
        {
            Console.WriteLine("[Plugin] " + ID + " is starting up...");
            return new Result();
        }
        /// <summary>
        /// Stops the plugin, writing a string to the console before returning a success.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Returns the default <see cref="Result"/> instance.</para></returns>
        public Result Stop()
        {
            Console.WriteLine("[Plugin] " + ID + " is stopping...");
            return new Result();
        }
    }
}
