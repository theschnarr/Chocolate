using Diplomacy.Kit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginDiscovery
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //Uncomment the lines below to run the various demonstrations.
                //Demos.RunReflectionDemo();
                //Demos.RunMefDemo();
                //Demos.RunLazyMefDemo();
                //Demos.RunSpecificPluginDemo();
                Demos.RunSpecificPluginWithMetadataDemo();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press <Enter> to Exit");
            Console.ReadLine();
        }
    }
}
