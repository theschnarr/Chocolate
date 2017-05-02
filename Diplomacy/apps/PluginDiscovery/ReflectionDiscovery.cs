using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery
{
    /// <summary>
    /// Class used to demonstrate "plugin discovery" using legacy .NET Reflection.
    /// This sample is based on functionality discussed on MSDN: https://code.msdn.microsoft.com/windowsdesktop/Creating-a-simple-plugin-b6174b62
    /// </summary>
    public static class ReflectionDiscovery
    {
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="assembly">Type: <see cref="Assembly"/><para>The .NET Assembly to check for viable plugin types.</para></param>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <returns>Type: <see cref="IEnumerable{T}"/><para>The collection of viable plugin instances.</para></returns>
        public static IEnumerable<T> LoadPlugins<T>(Assembly assembly)
        {
            var plugins = new List<T>();
            if (assembly != null)
            {
                //If the assembly is not null, then we need to load all the types exposed in the assembly.
                Type[] types = assembly.GetTypes();
                foreach(Type type in types)
                {
                    //For each type we need to run a check
                    if (type.IsInterface || type.IsAbstract)
                    {
                        //The current type is an interface or an abstract class.  As such can not have instances created at runtime.
                        //Proceed to the next type in the list.
                        continue;
                    }
                    else
                    {
                        //The type if an instantiable class
                        if (type.GetInterface(typeof(T).FullName) != null)
                        {
                            //The current type implements the provided interface, and is there for a viable plugin candidate.
                            //The System.Activator class is used to create a new instance of plugin candidate, using the type.
                            plugins.Add((T)Activator.CreateInstance(type));  
                        }
                    }
                }
            }
            //The list of viable plugin instances, if any were found and created, is returned.
            return plugins;
        }
        /// <summary>
        /// Load plugins of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Identifies the interface type that is being used to create/interact with plugins.</typeparam>
        /// <param name="directory">Type: <see cref="String"/><para>The full path of the directory to load plugins from.</para></param>
        /// <returns>Type: <see cref="IEnumerable{T}"/><para>The collection of viable plugin instances.</para></returns>
        public static IEnumerable<T> LoadPlugins<T>(string directory)
        {
            List<T> plugins = new List<T>();
            //We need to ensure that the provided directory path is valid.
            if (Directory.Exists(directory))
            {
                //If the path is valid, we need to load all the .dll file names found in the folder.
                var dllFiles = Directory.GetFiles(directory, "*.dll");
                foreach (var dllFile in dllFiles)
                {
                    //For each file, we need to load the dll into the application as a .NET Assembly, using reflection.
                    AssemblyName aName = AssemblyName.GetAssemblyName(dllFile);
                    //Once we have the AssemblyName, we can use that to load the assembly.
                    Assembly assembly = Assembly.Load(aName);
                    //Now that we have the assembly loaded, we can pass it to our LoadPlugins<T>(Assembly) overload to gather all viable plugin instances.
                    var aPlugins = LoadPlugins<T>(assembly);
                   
                    if (aPlugins != null)
                    {
                        //If there were plugins found, then we add them into our List.
                        plugins.AddRange(aPlugins);
                    }
                }
            }
            //The list of viable plugin instances, if any were found and created, is returned.
            return plugins;
        }
    }
}
