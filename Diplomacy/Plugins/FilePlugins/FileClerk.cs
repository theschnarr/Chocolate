using Diplomacy.Kit;
using Diplomacy.Kit.MEF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilePlugins
{
    /// <summary>
    /// Demonstration implementation of the <see cref="IClerk"/> plugin.  Demo will simulate a file scanning clerk, which sends scanned files for processing.
    /// </summary>
    [ExportClerk("FileClerk", "ProviderABC")]
    public class FileClerk : IClerk
    {
        /// <summary>
        /// Type: <see cref="IConsulRequest"/><para>The instance that will handle the plugin's request processing.</para>
        /// </summary>
        private IConsulRequest _ConsulRequest;
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        public string ID
        {
            get
            {
                return "FileClerk";
            }
        }
        /// <summary>
        /// Sets the consul the Clerk plugin should use when submitting requests for processing.
        /// </summary>
        /// <param name="consul">Type: <see cref="IConsulRequest"/><para>The instance of the request interface the plugin should use for submit requests.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the consul set request.</para></returns>
        public Result SetConsul(IConsulRequest consul)
        {
            if (consul == null)
            {
                return new Result(ResultCode.InvalidArgument);
            }
            _ConsulRequest = consul;
            return new Result();
        }
        /// <summary>
        /// Starts the plugin, performing necessary tasks required before subsequent processing can occur.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the start request.</para></returns>
        public Result Start()
        {
            //TODO:  This will eventually need to set up the FileSystemWatcher to scan directories.
            return new Result();
        }
        /// <summary>
        /// Stops the plugin, performing any required clean up tasks.
        /// </summary>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the stop request.</para></returns>
        public Result Stop()
        {
            //TODO:  This weill eventually need to shutdown and dispose the FileSystemWatcher and any thread(s) used.
            return new Result();
        }
    }
}
