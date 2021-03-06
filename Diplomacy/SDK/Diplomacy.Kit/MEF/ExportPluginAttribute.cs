﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplomacy.Kit.MEF
{
    /// <summary>
    /// Used to provide discovery details needed to find and validate plugins. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false), MetadataAttribute]
    public class ExportPluginAttribute : ExportAttribute, IPluginData
    {
        /// <summary>
        /// Creates a new istance of <see cref="ExportPluginAttribute"/>.
        /// </summary>
        /// <param name="id">Type: <see cref="String"/><para>The unique ID to use for the metadata.</para></param>
        /// <param name="providerID">Type: <see cref="String"/><para>The unique ID of the provider of the plugin.</para></param>
        public ExportPluginAttribute(string id, string providerID): base(typeof(IPlugin))
        {
            ID = id;
            ProviderID = providerID;
        }
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the implementing plugin.</para>
        /// </summary>
        public string ID
        {
            get; private set;
        }
        /// <summary>
        /// Type: <see cref="String"/><para>The unique ID associated with the provider of the associated plugin.</para>
        /// </summary>
        public string ProviderID
        {
            get; private set;
        }
    }
}
