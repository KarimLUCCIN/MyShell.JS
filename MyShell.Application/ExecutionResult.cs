using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShell.Application
{
    /// <summary>
    /// Après chaque exécution de script, contient les différents éléments du résultat
    /// </summary>
    public class ExecutionResult
    {
        private string source;

        public string Source
        {
            get { return source; }
            set { source = value; }
        }

        private bool success;

        public bool Success
        {
            get { return success; }
            set { success = value; }
        }
        
        private string error;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }

        private object result;

        public object Result
        {
            get { return result; }
            set { result = value; }
        }

        public string StringResult
        {
            get { return result == null ? "{null}" : result.ToString(); }
        }

        public bool NullResult
        {
            get { return result == null; }
        }

        public bool HaveResult
        {
            get { return !NullResult; }
        }
    }
}
