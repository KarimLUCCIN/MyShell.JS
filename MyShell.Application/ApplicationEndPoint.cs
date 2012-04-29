using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MyShell.Application
{
    /// <summary>
    /// La classe d'application exposée via JavaScript
    /// </summary>
    public class ApplicationEndPoint
    {
        public Assembly clrload(string assemblyName)
        {
            return Assembly.LoadWithPartialName(assemblyName);
        }

        public Type clrtype(string name)
        {
            return Type.GetType(name);
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
