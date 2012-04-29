using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyShell.Application.Snips
{
    public class JavaScriptSnip : Snip
    {
        private string script;

        public string Script
        {
            get { return script; }
        }

        public JavaScriptSnip(string script)
        {
            this.script = script;
        }

        protected override void Initialize()
        {
            base.Initialize();

            if (!String.IsNullOrEmpty(Script))
                Host.ExecuteScript(Script);
        }
    }
}
