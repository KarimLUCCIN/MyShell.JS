using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShell.Application.Snips
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CLRSnipEnabledAttribute : Attribute
    {

    }

    public abstract class CLRSnip : Snip
    {

    }
}
