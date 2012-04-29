using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShell.Application.Snips
{
    public abstract class Snip
    {
        public ApplicationHost Host { get; private set; }

        public void Load(ApplicationHost host)
        {
            if (host == null)
                throw new ArgumentNullException("host");

            Host = host;

            Initialize();
        }

        protected virtual void Initialize()
        {

        }
    }
}
