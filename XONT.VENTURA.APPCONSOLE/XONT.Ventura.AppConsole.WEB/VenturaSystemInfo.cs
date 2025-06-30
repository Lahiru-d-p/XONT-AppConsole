using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using XONT.Common.Message;

namespace XONT.Ventura.AppConsole
{
    public class LibInfo
    {
        public string name { get; set; }
        public string version { get; set; }
        public DateTime? BuildDate { get; set; }
    }

    public class RefLibInfo : LibInfo
    {
        public string builtWithVersion { get; set; }
    }

    public class ComponentVersionInfo
    {
        public List<LibInfo> CoponentLibs = new List<LibInfo>();
        public List<RefLibInfo> Level1RefLibs = new List<RefLibInfo>();
        public List<LibInfo> Level2RefLibs = new List<LibInfo>();
        public List<LibInfo> OtherXONTLibs = new List<LibInfo>();
        public List<LibInfo> ThirdPartyLibs = new List<LibInfo>();
        public List<LibInfo> ClientLibs = new List<LibInfo>();
    }

    public class SystemVersionInfo
    {
        public List<LibInfo> XONTLibs = new List<LibInfo>();
        public List<LibInfo> ThirdPartyLibs = new List<LibInfo>();
    }

    public static class VenturaSystemInfo
    {
        //public static void Fun()
        //{
        //    DecompilerSettings settings = new DecompilerSettings();
        //    settings.FullyQualifyAmbiguousTypeNames = true;
        //    var resolver = new DefaultAssemblyResolver();
        //    resolver.AddSearchDirectory("C:\\Documents and Settings\\3274mtop\\Desktop\\dlls");

        //    var parameters = new ReaderParameters
        //    {
        //        AssemblyResolver = resolver,
        //    };

        //    AssemblyDefinition assembly1 = AssemblyDefinition.ReadAssembly(assemblyName);



        //    AstBuilder decompiler = new AstBuilder(new DecompilerContext(assembly1.MainModule) { Settings = settings });
        //    decompiler.AddAssembly(assembly1);

        //    StringWriter output = new StringWriter();
        //    decompiler.GenerateCode(new PlainTextOutput(output));

        //    byte[] byteArray = Encoding.ASCII.GetBytes(output.ToString());
        //    TextReader codeReader = new StreamReader(new MemoryStream(byteArray));
        //    string line = codeReader.ReadToEnd();
        //}

        public static SystemVersionInfo GetSystemVersionInfo(out MessageSet msg)
        {
            msg = null;
            SystemVersionInfo toReturn = new SystemVersionInfo();
            try
            {
                var ourAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList().FindAll(a => a.FullName.Contains("XONT"));

                List<LibInfo> xontAssembliesInfo = new List<LibInfo>();
                ourAssemblies.ForEach(a =>
                {
                    xontAssembliesInfo.Add(new LibInfo() { name = a.GetName().Name, version = a.GetName().Version.ToString(), BuildDate = a.GetLinkerTime() });

                });
                toReturn.XONTLibs = xontAssembliesInfo.OrderBy(l => l.name).ToList();

                List<LibInfo> otherAssembliesInfo = new List<LibInfo>();
                var otherAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList().FindAll(a => !a.FullName.Contains("XONT"));
                otherAssemblies.ForEach(a =>
                {
                    otherAssembliesInfo.Add(new LibInfo() { name = a.GetName().Name, version = a.GetName().Version.ToString() });

                });
                toReturn.ThirdPartyLibs = otherAssembliesInfo.OrderBy(l => l.name).ToList();
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetSystemVersionInfo", "XONT.Ventura.AppConsole.WEB.dll");
            }

            return toReturn;

        }

        public static ComponentVersionInfo GetComponentVersionInfo(string taskCode, bool isWithThirdPartyAndOther, out MessageSet msg)
        {
            msg = null;
            ComponentVersionInfo toReturn = new ComponentVersionInfo();
            try
            {
                var ourAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList().FindAll(a => a.FullName.Contains("XONT"));
                var otherAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList().FindAll(a => !a.FullName.Contains("XONT"));
                var componentAssemblies = ourAssemblies.FindAll(a => a.FullName.Contains(taskCode));


                //referenced dlls - start
                List<Assembly> referencedAssemblies = new List<Assembly>();//only what contain in the site bin

                List<RefLibInfo> relatedAssembliesInfo = new List<RefLibInfo>();//including what is and what is not in the site bin


                foreach (var layer in componentAssemblies)//bll,dal,domain,web
                {
                    AssemblyName[] layerReferences = layer.GetReferencedAssemblies();

                    foreach (var layerRef in layerReferences)
                    {
                        var originalAssmebly = ourAssemblies.Find(a => a.GetName().Name == layerRef.Name);
                        var alreadyContains = relatedAssembliesInfo.Find(l => l.name == layerRef.Name);

                        if (alreadyContains == null && !referencedAssemblies.Contains(originalAssmebly) && !componentAssemblies.Contains(originalAssmebly))
                        {
                            if (originalAssmebly == null && layerRef.Name.Contains("XONT"))
                            {
                                relatedAssembliesInfo.Add(new RefLibInfo() { name = layerRef.Name, builtWithVersion = layerRef.Version.ToString(), version = "NA" });
                            }
                            else if (originalAssmebly != null)
                            {
                                relatedAssembliesInfo.Add(new RefLibInfo() { name = layerRef.Name, builtWithVersion = layerRef.Version.ToString(), BuildDate = originalAssmebly.GetLinkerTime(), version = originalAssmebly.GetName().Version.ToString() });
                                referencedAssemblies.Add(originalAssmebly);
                            }
                        }
                    }
                }

                toReturn.Level1RefLibs = relatedAssembliesInfo.OrderBy(l => l.name).ToList();
                //referenced dlls - end

                //2nd layer dlls - start
                List<Assembly> SecondLayerRefLibs = new List<Assembly>();

                foreach (Assembly refAssembly in referencedAssemblies)
                {
                    AssemblyName[] refRelatedLibs = refAssembly.GetReferencedAssemblies();

                    foreach (var layerRef in refRelatedLibs)
                    {
                        var originalAssmebly = ourAssemblies.Find(a => a.GetName().Name == layerRef.Name);

                        if (originalAssmebly != null && !referencedAssemblies.Contains(originalAssmebly) && !SecondLayerRefLibs.Contains(originalAssmebly))
                        {
                            SecondLayerRefLibs.Add(originalAssmebly);
                        }
                    }
                }
                List<LibInfo> SecondLayerRefLibsInfo = new List<LibInfo>();
                foreach (var refAssembly in SecondLayerRefLibs)
                {
                    SecondLayerRefLibsInfo.Add(new LibInfo() { name = refAssembly.GetName().Name, version = refAssembly.GetName().Version.ToString(), BuildDate = refAssembly.GetLinkerTime() });
                }

                toReturn.Level2RefLibs = SecondLayerRefLibsInfo.OrderBy(l => l.name).ToList();
                //2nd layer dlls - end

                //component dlls themselves- start
                List<LibInfo> componentAssembliesInfo = new List<LibInfo>();
                foreach (var comAssembly in componentAssemblies)
                {
                    componentAssembliesInfo.Add(new LibInfo() { name = comAssembly.GetName().Name, version = comAssembly.GetName().Version.ToString(), BuildDate = comAssembly.GetLinkerTime() });
                }

                toReturn.CoponentLibs = componentAssembliesInfo.OrderBy(l => l.name).ToList();
                //component dlls themselves- end

                if (isWithThirdPartyAndOther)
                {
                    //other xont dlls that are not referenced - start
                    List<Assembly> xontOtherAssemblies = new List<Assembly>();
                    ourAssemblies.ForEach(a =>
                    {
                        if (!componentAssemblies.Contains(a) && !referencedAssemblies.Contains(a))
                            xontOtherAssemblies.Add(a);
                    });

                    List<LibInfo> xontOtherAssembliesInfo = new List<LibInfo>();
                    foreach (var comAssembly in xontOtherAssemblies)
                    {
                        xontOtherAssembliesInfo.Add(new LibInfo() { name = comAssembly.GetName().Name, version = comAssembly.GetName().Version.ToString(), BuildDate = comAssembly.GetLinkerTime() });
                    }

                    toReturn.OtherXONTLibs = xontOtherAssembliesInfo.OrderBy(l => l.name).ToList();
                    //other xont dlls that are not referenced - end

                    //third party dlls -start
                    List<LibInfo> thirdPartyAssemblies = new List<LibInfo>();
                    foreach (var otherAssembly in otherAssemblies)
                    {
                        thirdPartyAssemblies.Add(new LibInfo() { name = otherAssembly.GetName().Name, version = otherAssembly.GetName().Version.ToString() });
                    }

                    toReturn.ThirdPartyLibs = thirdPartyAssemblies.OrderBy(l => l.name).ToList();
                    //third party dlls -end
                }

            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetComponentVersionInfo", "XONT.Ventura.AppConsole.WEB.dll");
            }
            return toReturn;
        }

        public static List<LibInfo> GetClientLibsVersionInfo(out MessageSet msg)
        {
            msg = null;
            List<LibInfo> nodeModules = new List<LibInfo>();
            try
            {
                string nodeModulesPath = HttpRuntime.AppDomainAppPath + "node_modules";
                List<string> ClientLibraries = Directory.GetDirectories(nodeModulesPath).ToList().FindAll(d => d.Contains("xont-ventura"));

                foreach (string lib in ClientLibraries)
                {
                    string pkgPath = lib + "\\package.json";
                    using (StreamReader r = new StreamReader(pkgPath))
                    {
                        string json = r.ReadToEnd();
                        LibInfo item = JsonConvert.DeserializeObject<LibInfo>(json);
                        nodeModules.Add(item);
                    }
                }
                return nodeModules.OrderBy(l => l.name).ToList();
            }
            catch (Exception ex)
            {
                msg = MessageCreate.CreateErrorMessage(0, ex, "GetClientLibsVersionInfo", "XONT.Ventura.AppConsole.WEB.dll");
            }
            return nodeModules;
        }

        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
    }
}