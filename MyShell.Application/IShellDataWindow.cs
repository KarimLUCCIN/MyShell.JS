using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShell.Application
{
    public interface IShellDataWindow
    {
        string Data { get; set; }

        void Close();
    }
}
