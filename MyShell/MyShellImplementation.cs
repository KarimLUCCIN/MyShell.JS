using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyShell.Application;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace MyShell
{
    public class MyShellImplementation : IShellImplementation, INotifyPropertyChanged
    {
        private ApplicationHost host;

        public ApplicationHost Host
        {
            get { return host; }
        }
        
        public MyShellImplementation()
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

        #endregion
    }
}
