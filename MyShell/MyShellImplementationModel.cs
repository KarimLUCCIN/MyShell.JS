using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyShell.Application;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MyShell
{
    public class MyShellImplementationModel : IShellImplementation, INotifyPropertyChanged
    {
        private ApplicationHost host;

        public ApplicationHost Host
        {
            get { return host; }
        }
        
        public MyShellImplementationModel()
        {
            host = new ApplicationHost(this);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        public bool Execute(string script)
        {
            var result = Host.ExecuteScript(script);

            return result != null && result.Success;
        }

        #region IShellImplementation Members

        private ObservableCollection<ExecutionResult> results = new ObservableCollection<ExecutionResult>();

        public IList<ExecutionResult> Results
        {
            get { return results; }
        }

        public event EventHandler RequestScrollToLastResultEventHandler;
        public void ScrollToLastResult()
        {
            if (RequestScrollToLastResultEventHandler != null)
                RequestScrollToLastResultEventHandler(this, EventArgs.Empty);
        }

        public event EventHandler RequestCloseEventHandler;
        public void Close()
        {
            if (RequestCloseEventHandler != null)
                RequestCloseEventHandler(this, EventArgs.Empty);
        }

        Dictionary<string, IShellDataWindow> runningDataWindows = new Dictionary<string, IShellDataWindow>();
        public IShellDataWindow GetDataWindow(string id, bool canCreate = true)
        {
            if (String.IsNullOrEmpty(id))
                return null;
            else
            {
                IShellDataWindow wnd;

                if (!runningDataWindows.TryGetValue(id, out wnd))
                {
                    if (!canCreate)
                        return null;
                    else
                    {
                        var n_wnd = new ShellDataWindow(id);
                        wnd = runningDataWindows[id] = n_wnd;

                        n_wnd.Show();
                        n_wnd.Title = String.Format("Data: {0}", id);
                        n_wnd.Closed += delegate
                        {
                            runningDataWindows.Remove(id);
                        };
                    }
                }

                return wnd;
            }
        }

        #endregion
    }
}
