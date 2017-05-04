using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Defines the methods and properties which detail the result of a request.
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Creates a new instance of <see cref="Result"/>, using the provided <paramref name="code"/>.
        /// </summary>
        /// <param name="code">Type: <see cref="ResultCode"/><para>The code/message to use with this instance.  If not provided, <see cref="ResultCode.Success"/> will be used by default.</para></param>
        public Result(ResultCode code = null)
        {
            if (code == null)
                code = ResultCode.Success;
            Code = code;
        }
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>The result value and display name associated with the current <see cref="Result{T}"/>.</para>
        /// </summary>
        public ResultCode Code { get; private set; }
    }
    /// <summary>
    /// Defines the expected "enum" constants for framework operations.
    /// </summary>
    public class ResultCode: Enumeration
    {
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate a success.</para>
        /// </summary>
        public static readonly ResultCode Success = new ResultCode(0, "Success");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate that an invalid argument was provided.</para>
        /// </summary>
        public static readonly ResultCode InvalidArgument = new ResultCode(-100, "Invalid argument specified.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate an error when the class has not been properly initialized.</para>
        /// </summary>
        public static readonly ResultCode NotInitialized = new ResultCode(-400, "Class has not been initialized.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate an error when the class has already been initialized.</para>
        /// </summary>
        public static readonly ResultCode AlreadyInitialized = new ResultCode(-401, "Class has already been initialized.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate an invalid Plugin ID has been provided.</para>
        /// </summary>
        public static readonly ResultCode InvalidPluginID = new ResultCode(-410, "Plugin ID is an invalid format.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate a duplicate Plugin ID has been provided.</para>
        /// </summary>
        public static readonly ResultCode InvalidPluginDuplicateID = new ResultCode(-411, "Plugin ID is already registerred.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate an invalid Plugin Provider has been provided.</para>
        /// </summary>
        public static readonly ResultCode InvalidPluginProvider = new ResultCode(-412, "Plugin Provider is not a viable ID.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate that a plugin assembly could not be validated for the specified Provider.</para>
        /// </summary>
        public static readonly ResultCode InvalidPluginAssembly = new ResultCode(-413, "Plugin assembly is not valid for the Provider.");
        /// <summary>
        /// Type: <see cref="ResultCode"/><para>Code returned to indicate that no plugin assemblies were loaded.</para>
        /// </summary>
        public static readonly ResultCode NoPluginsLoaded = new ResultCode(-420, "No plugins loaded.");
        /// <summary>
        /// Default constructor.
        /// </summary>
        public ResultCode(): base()
        {

        }
        /// <summary>
        /// Creates a new instance of <see cref="ResultCode/> with the <paramref name="value"/> and <paramref name="displayName"/>.
        /// </summary>
        /// <param name="value">Type: <see cref="Int32"/><para>The numeric value of the new instance.</para></param>
        /// <param name="displayName">Type: <see cref="String"/><para>The display name of the new instance.</para></param>
        public ResultCode(int value, string displayName): base(value, displayName)
        {

        }
    }
}
