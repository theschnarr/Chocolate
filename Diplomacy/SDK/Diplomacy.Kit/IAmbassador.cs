﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit
{
    /// <summary>
    /// Defines the methods and properties that will be exposed by any class conforming to the <see cref="IAmbassador"/> interface.
    /// </summary>
    public interface IAmbassador: IPlugin
    {
        #region Properties
        /// <summary>
        /// Type: <see cref="IEnumerable{String}"/><para>The collection of the actions supported by the implementing plugins.</para>
        /// </summary>
        IEnumerable<String> SupportedActions { get; }
        #endregion
        #region Methods
        /// <summary>
        /// Processes the request, detailed in the <paramref name="requestData"/>.
        /// </summary>
        /// <param name="requestData">Type: <see cref="String"/><para>Contains the data associated with the request.</para></param>
        /// <returns>Type: <see cref="Result"/><para>Details the result of the process request.</para></returns>
        Result Process(string requestData);
        #endregion
    }
}
