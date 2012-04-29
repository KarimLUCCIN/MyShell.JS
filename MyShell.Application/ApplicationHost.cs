using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Noesis.Javascript;
using System.Diagnostics;

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

            endPoint = new ApplicationEndPoint();

            context = new JavascriptContext();
            context.SetParameter("app", endPoint);
            context.SetParameter("shell", shellImpl);
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
                    result.Result = context.Run(script);

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
