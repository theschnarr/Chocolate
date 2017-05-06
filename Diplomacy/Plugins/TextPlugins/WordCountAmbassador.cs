using Diplomacy.Kit;
using Diplomacy.Kit.MEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextPlugins
{
    /// <summary>
    /// Demonstration implementation of the <see cref="IAmbassador"/> plugin.  Demo will simulate a word counting ambassador.
    /// </summary>
    [ExportAmbassador("WordCountAmbassador", "ProviderABC")]
    public class WordCountAmbassador : IAmbassador
    {
        /// <summary>
        /// Type: <see cref="List{String}"/><para>Lists the action phrases associated with the supported actions of the plugin.</para>
        /// </summary>
        private List<string> _SupportedActions = new List<string>() { "WordCount" };
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        public string ID
        {
            get
            {
                return "WordCountAmbassador";
            }
        }
        /// <summary>
        /// Type: <see cref="IEnumerable{String}"/><para>The collection of the actions supported by the implementing plugins.</para>
        /// </summary>
        public IEnumerable<string> SupportedActions
        {
            get
            {
                return _SupportedActions;
            }
        }
        /// <summary>
        /// Processes the request, detailed in the <paramref name="requestData"/>.
        /// </summary>
        /// <param name="requestData">Type: <see cref="String"/><para>Contains the data associated with the request.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the process request.</para></returns>
        public Result Process(string requestData)
        {
            if (String.IsNullOrWhiteSpace(requestData))
            {
                return new Result(ResultCode.InvalidArgument);
            }
            //TODO:  Add functionality to count words in a text file.
            return new Result();
        }
        /// <summary>
        /// Starts the plugin, performing necessary tasks required before subsequent processing can occur.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the start request.</para></returns>
        public Result Start()
        {
            return new Result();
        }
        /// <summary>
        /// Stops the plugin, performing any required clean up tasks.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the stop request.</para></returns>
        public Result Stop()
        {
            return new Result();
        }
    }
}
