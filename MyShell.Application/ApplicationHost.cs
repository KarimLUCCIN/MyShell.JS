using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noesis.Javascript;
using System.Diagnostics;
using System.Reflection;

namespace MyShell.Application
{
    public class ApplicationHost : IDisposable
    {
        private JavascriptContext context;

        public JavascriptContext Context
        {
            get { return context; }
        }

        private ApplicationEndPoint endPoint;

        public ApplicationEndPoint EndPoint
        {
            get { return endPoint; }
        }

        private IShellImplementation shellImpl;

        public IShellImplementation ShellImpl
        {
            get { return shellImpl; }
            set { shellImpl = value; }
        }

        public ApplicationHost(IShellImplementation shellImpl)
        {
            if (shellImpl == null)
                throw new ArgumentNullException("shellImpl");

            this.shellImpl = shellImpl;

            endPoint = new ApplicationEndPoint(this);

            context = new JavascriptContext();
            context.SetParameter("app", endPoint);
            context.SetParameter("shell", shellImpl);

            RegisterGlobalFunctions();
        }

        private void RegisterGlobalFunctions()
        {
            context.SetParameter("close", (Action)shellImpl.Close);

            context.SetParameter("advert", (Func<string, string>)endPoint.advert);

            context.SetParameter("clrload", (Func<string, Assembly>)endPoint.clrload);
            context.SetParameter("clrtype", (Func<string, Type>)endPoint.clrtype);

            context.SetParameter("valueof", (Func<object, string>)endPoint.valueof);

            context.SetParameter("clear", (Action)delegate
            {
                shellImpl.Results.Clear();
                lastReturns.Clear();
            });

            context.SetParameter("prev", (Func<int, object>)delegate(int index)
            {
                if (lastReturns.Count < 1)
                    return null;
                else
                {
                    if (index < 0)
                        return lastReturns.ToArray();
                    else if (index >= lastReturns.Count)
                        return null;
                    else
                        return lastReturns[lastReturns.Count - index - 1];
                }
            });
        }

        #region IDisposable Members

        ~ApplicationHost()
        {
            Dispose(false);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (context != null)
                    context.Dispose();
            }

            GC.SuppressFinalize(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

        int maxLastReturns = 50;
        List<object> lastReturns = new List<object>();

        public ExecutionResult ExecuteScript(string script)
        {
            if (String.IsNullOrEmpty(script))
                return null;
            else
            {
                var result = new ExecutionResult();
                result.Source = script;
                result.Success = false;

                try
                {
                    lastReturns.Add(result.Result = context.Run(script));

                    while (lastReturns.Count > maxLastReturns)
                        lastReturns.RemoveAt(0);

                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.Error = ex.ToString();
                }

                try
                {
                    shellImpl.Results.Add(result);
                    shellImpl.ScrollToLastResult();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }

                return result;
            }
        }
    }
}
