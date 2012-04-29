using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyShell.Application.Snips;
using System.Diagnostics;

namespace MyShell.Apps.Snip
{
    [CLRSnipEnabled]
    public class Run : CLRSnip
    {
        protected override void Initialize()
        {
            base.Initialize();

            Host.RegisterFunction("run_internal", (Func<string, Process>)RunFunction);
        }

        public Process RunFunction(string cmdLine)
        {
            if (!String.IsNullOrEmpty(cmdLine))
            {
                var parts = cmdLine.Split(new string[] { "|?|" }, StringSplitOptions.RemoveEmptyEntries);
                var exeName = Environment.ExpandEnvironmentVariables(parts[0]);

                var arguments = new List<string>();
                string workingDir = String.Empty;

                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].StartsWith("d:"))
                        workingDir = parts[i].Substring(2);
                    else if (parts[i].StartsWith("a:"))
                        arguments.Add(parts[i].Substring(2));
                }

                var psi = new ProcessStartInfo(exeName);
                psi.Arguments = String.Concat(from arg in arguments select arg.Replace("\"", "\"\"") + " ");

                if (!String.IsNullOrEmpty(workingDir))
                    psi.WorkingDirectory = Environment.ExpandEnvironmentVariables(workingDir);

                psi.UseShellExecute = false;

                return Process.Start(psi);
            }
            else
                return null;
        }
    }
}
