using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyShell.Application.Snips;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace MyShell.Parsers.Snip
{
    [CLRSnipEnabled]
    public class Messages : CLRSnip
    {
        protected override void Initialize()
        {
            base.Initialize();

            Host.RegisterFunction("regex", (Func<string, string, string[][]>)delegate(string pattern, string text)
            {
                var reg = new Regex(pattern, RegexOptions.Multiline);
                return (from Match match in reg.Matches(text) select (from Group gr in match.Groups select gr.Value).ToArray()).ToArray();
            });

            Host.RegisterFunction("htmlXNode", (Func<string, string, HtmlNodeCollection>)delegate(string htmlCode, string path)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(htmlCode);

                return doc.DocumentNode.SelectNodes(path);
            });

            Host.RegisterFunction("nodeXNode", (Func<HtmlNode, string, HtmlNodeCollection>)delegate(HtmlNode htmlNode, string path)
            {
                return htmlNode.SelectNodes(path);
            });
        }
    }
}
