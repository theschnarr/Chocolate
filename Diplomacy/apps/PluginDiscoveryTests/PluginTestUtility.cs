using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace PluginDiscoveryTests
{
    public class PluginTestUtility
    {
        /// <summary>
        /// Creates a new instance of <see cref="PluginTestUtility"/>.
        /// </summary>
        public PluginTestUtility()
        {
            PluginTempDirectory = Environment.CurrentDirectory + "\\PluginsTemp\\";
        }
        /// <summary>
        /// Type: <see cref="String"/><para>The full path to use for temporary storage and loading of plugins during testing.</para>
        /// </summary>
        public string PluginTempDirectory { get; private set; }
        /// <summary>
        /// Generates a Diplomacy plugin assembly, using the <paramref name="soure"/>.
        /// </summary>
        /// <param name="source">Type: <see cref="String"/><para>Contains the source code to be compiled into the output assembly.</para></param>
        /// <returns>Type: <see cref="Assembly"/><para>The compiled assembly.</para></returns>
        public Assembly GeneratePluginAssembly(string source)
        {
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.ComponentModel.Composition.dll");
            parameters.ReferencedAssemblies.Add("Diplomacy.Kit.dll");
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, source);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }
            else
            {
                return results.CompiledAssembly;
            }
        }
        /// <summary>
        /// Generates a Diplomacy plugin dll, using the <paramref name="soure"/>.
        /// </summary>
        /// <param name="source">Type: <see cref="String"/><para>Contains the source code to be compiled into the output assembly.</para></param>
        /// <param name="dllPath">Type: <see cref="String"/><para>The dll name to use when generating the plugin file.</para></param>
        /// <returns>Type: <see cref="String"/><para>The full path of the generated dll.</para></returns>
        public string GeneratePluginDll(string source, string dllPath)
        {
            string dir = Path.GetDirectoryName(dllPath);
            if (dir == String.Empty)
            {
                dir = PluginTempDirectory;
                dllPath = Path.Combine(dir, dllPath);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
            }
            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();
            parameters.ReferencedAssemblies.Add("System.ComponentModel.Composition.dll");
            parameters.ReferencedAssemblies.Add("Diplomacy.Kit.dll");
            parameters.GenerateInMemory = false;
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = dllPath;
            CompilerResults results = provider.CompileAssemblyFromSource(parameters, source);
            if (results.Errors.HasErrors)
            {
                StringBuilder sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(sb.ToString());
            }
            else
            {
                return results.PathToAssembly;
            }
        }
    }
}
