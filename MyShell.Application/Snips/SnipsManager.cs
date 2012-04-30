using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace MyShell.Application.Snips
{
    public class SnipsManager
    {
        public ApplicationHost Host { get; private set; }

        public SnipsManager(ApplicationHost host)
        {
            Host = host;
        }

        public void Load()
        {
            /* on charge les différents snips */
            var jsSnips =  "*.snip.js";
            var clrSnips = "*.snip.dll";

            LoadSnips(
                new string[]{
                    Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Snips"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MyShell", "Snips")
                },
                jsSnips,
                clrSnips);
        }

        private void LoadSnips(string[] baseDirectories, string jsSnips, string clrSnips)
        {
            var log = String.Empty;
            bool success = true;

            foreach (var baseDirectory in baseDirectories)
            {
                var directoryInfo = new DirectoryInfo(baseDirectory);
                if (directoryInfo.Exists)
                {
                    try
                    {
                        log += String.Format("Snip Directory : {0}\n", baseDirectory);
                        
                        foreach (var item in Directory.GetFiles(baseDirectory, clrSnips))
                        {
                            log += String.Format("dll: {0}\n", item);

                            try
                            {
                                LoadClrSnip(item, ref log);
                            }
                            catch (Exception ex)
                            {
                                success = false;
                                log += ex.ToString() + "\n\n";
                            }
                        }

                        foreach (var item in Directory.GetFiles(baseDirectory, jsSnips))
                        {
                            log += String.Format("js: {0}\n", item);

                            try
                            {
                                LoadJsSnip(item);
                            }
                            catch (Exception ex)
                            {
                                success = false;
                                log += ex.ToString() + "\n\n";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        success = false;
                        log += ex.ToString() + "\n\n";
                    }
                }
            }

            if (!success)
                Host.EndPoint.data("snips.errors", log);
        }

        public void LoadClrSnip(string assemblyPath, string executeAfter = null)
        {
            string log = String.Empty;

            LoadClrSnip(assemblyPath, ref log, executeAfter);
        }

        private void LoadClrSnip(string assemblyPath, ref string log, string executeAfter = null)
        {
            var dir = Path.GetDirectoryName(assemblyPath);
            AppDomain.CurrentDomain.AppendPrivatePath(dir);

            var asm = Assembly.LoadFile(assemblyPath);
            var types = asm.GetTypes();

            foreach (var type in types)
            {
                var atts = type.GetCustomAttributes(typeof(CLRSnipEnabledAttribute), true);
                foreach (var att in atts)
                {
                    var en_att = att as CLRSnipEnabledAttribute;
                    if (en_att != null)
                    {
                        log += String.Format("type: {0}\n", type.FullName);

                        var constructor = type.GetConstructor(new Type[0]);
                        var snip = (CLRSnip)constructor.Invoke(new object[0]);

                        snip.Load(Host);

                        break;
                    }
                }
            }

            if (!String.IsNullOrEmpty(executeAfter))
                Host.ExecuteScript(executeAfter);
        }

        public void LoadJsSnip(string filePath, string executeAfter = null)
        {
            using (var strm = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new StreamReader(strm))
                {
                    var script = reader.ReadToEnd();

                    Host.ExecuteScript(script);

                    if (!String.IsNullOrEmpty(executeAfter))
                        Host.ExecuteScript(executeAfter);
                }
            }
        }
    }
}
