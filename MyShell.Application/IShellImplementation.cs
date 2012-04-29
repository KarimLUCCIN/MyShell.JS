using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShell.Application
{
    public interface IShellImplementation
    {
        IList<ExecutionResult> Results { get; }
        void ScrollToLastResult();
    }
}
