using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NEA_ProgrammingLanguage
{
    static class PluginLoader
    {
        public static List<IPlugin> LoadPlugins(string dir)
        {
            List<IPlugin> toReturn = new List<IPlugin>();

            DirectoryInfo dllsDir = new DirectoryInfo(dir);

            foreach (FileInfo file in dllsDir.GetFiles())
            {
                if (file.Extension.Equals(".dll"))
                {
                    //Load the assembly
                    Assembly assembly = Assembly.LoadFile(file.FullName);

                    Type modules;

                    try
                    {
                        modules = assembly.GetTypes().SingleOrDefault(x => x.GetInterfaces().Contains(typeof(IPlugin)));

                        IPlugin module = (IPlugin)Activator.CreateInstance(modules);

                        toReturn.Add(module);
                    } catch
                    {
                        continue;
                    }
                }
            }

            return toReturn;
        }
    }
}
