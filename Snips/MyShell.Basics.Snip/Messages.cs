using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyShell.Application.Snips;
using System.Windows;

namespace MyShell.Basics.Snip
{
    [CLRSnipEnabled]
    public class Messages : CLRSnip
    {
        protected override void Initialize()
        {
            base.Initialize();

            Host.RegisterFunction("alert", (Action<string>)delegate(string msg) { MessageBox.Show(msg); });
        }
    }
}
