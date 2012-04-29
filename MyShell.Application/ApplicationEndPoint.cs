using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Dynamic;

namespace MyShell.Application
{
    /// <summary>
    /// La classe d'application exposée via JavaScript
    /// </summary>
    public class ApplicationEndPoint
    {
        public ApplicationHost Host { get; private set; }

        public ApplicationEndPoint(ApplicationHost host)
        {
            Host = host;
        }

        public string data(string id)
        {
            var wnd = Host.ShellImpl.GetDataWindow(id, false);

            if (wnd == null)
                return null;
            else
                return wnd.Data;
        }

        public string data(string id, string value)
        {
            var wnd = Host.ShellImpl.GetDataWindow(id, true);

            if (wnd != null)
                return wnd.Data = value;
            else
                return null;
        }

        public void closedata(string id)
        {
            var wnd = Host.ShellImpl.GetDataWindow(id, false);

            if (wnd != null)
                wnd.Close();
        }

        public Assembly clrload(string assemblyName)
        {
            return Assembly.LoadWithPartialName(assemblyName);
        }

        public Type clrtype(string name)
        {
            return Type.GetType(name);
        }

        public object clrcall(int type, object obj, string method, object[] args)
        {
            if (obj == null)
                return null;
            else
            {
                if (type == 0)
                {
                    /* this call */
                    return obj.GetType().InvokeMember(method, BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod, null, obj, args);
                }
                else
                {
                    /* static call */
                    var myType = (Type)obj;
                    return myType.InvokeMember(method, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Static, null, null, args);
                }
            }
        }

        public string valueof(object a)
        {
            if (a == null)
                return "{null}";
            else if (a is string)
                return (string)a;
            else if (a is Array)
            {
                return "Array: " + String.Concat(from object item in ((Array)a) select String.Format("[{0}] ", valueof(item)));
            }
            else if (a is IDictionary)
            {
                var dic = (IDictionary)a;
                return "Dictionary: " + String.Concat(from object key in dic.Keys select String.Format("[{0}:{1}] ", key, valueof(dic[key])));
            }
            else
                return a.ToString();
        }

        public string advert(object a)
        {
            if (a == null)
                return "{null}";
            else
            {
                var type = a.GetType();
                string result = type.AssemblyQualifiedName + "\n";


                var properties = type.GetProperties();
                var methods = type.GetMethods();



                if (properties.Length > 0)
                {
                    result += "Properties:\n";
                    foreach (var prop in properties)
                        result += String.Format("{0} ({1})", prop.Name, prop.PropertyType.FullName);

                    result += "\n";
                }

                if (methods.Length > 0)
                {
                    result += "Methods:\n";
                    foreach (var method in methods)
                        result += method.ToString() + "\n";

                    result += "\n";
                }

                return result;
            }
        }
    }
}
