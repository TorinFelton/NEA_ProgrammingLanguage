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

                    Type plugs;

                    try
                    {
                        plugs = assembly.GetTypes().SingleOrDefault(x => x.GetInterfaces().Contains(typeof(IPlugin)));

                        IPlugin plug = (IPlugin)Activator.CreateInstance(plugs);


                        toReturn.Add(plug);
                    } catch
                    {
                        continue;
                    }
                }
            }

            return toReturn;
        }

        public static List<IParseHandler> LoadParseHandlers(string dir)
        {
            List<IParseHandler> toReturn = new List<IParseHandler>();

            DirectoryInfo dllsDir = new DirectoryInfo(dir);

            foreach (FileInfo file in dllsDir.GetFiles())
            {
                if (file.Extension.Equals(".dll"))
                {
                    //Load the assembly
                    Assembly assembly = Assembly.LoadFile(file.FullName);

                    Type phan;

                    try
                    {
                        phan = assembly.GetTypes().SingleOrDefault(x => x.GetInterfaces().Contains(typeof(IParseHandler)));


                        IParseHandler pHandler = (IParseHandler)Activator.CreateInstance(phan);

                        toReturn.Add(pHandler);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return toReturn;
        }
    }
}
