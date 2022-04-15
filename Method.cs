using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml;

namespace ModMaker
{
    public class All
    {
        public static List<Class> LoadAll()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string file = asm.GetName().Name;
            file = file.Substring(0, file.Length - 8);
            XmlDocument xd = new XmlDocument();
            xd.Load(file + "TMLMethods.xml");
            XmlNode xn = xd.DocumentElement;
            List<Class> ls = new List<Class>();
            if (xn.HasChildNodes)
            {
                foreach (XmlNode i in xn.ChildNodes)
                {
                    List<Method> lsm = new List<Method>();
                    foreach (XmlNode ii in i.ChildNodes)
                    {
                        XmlNode xe = ii.SelectSingleNode("KeyWords");
                        List<string> lss = new List<string>();
                        List<Parameter> lsp = new List<Parameter>();
                        foreach (XmlNode item in xe.ChildNodes)
                        {
                            lss.Add(item.InnerText);
                        }
                        XmlNode xe2 = ii.SelectSingleNode("Parameter");
                        if (xe2.HasChildNodes)
                        {
                            foreach (XmlNode item in xe2.ChildNodes)
                            {
                                Parameter p = new Parameter(item.Attributes[1].Value, item.Attributes[0].Value, item.Attributes[2].Value);
                                lsp.Add(p);
                            }
                        }
                        Method me = new Method(ii.Attributes[0].Value, lss, lsp);
                        lsm.Add(me);
                    }
                    Class cl = new Class(i.Attributes[0].Value, lsm);
                    ls.Add(cl);
                }
            }
            return ls;
        }
    }
    public class Class
    {
        internal Class() { }
        internal Class(string Name, List<Method> methods)
        {
            this.Name = Name;
            this.methods = methods;
        }
        public List<Method> methods = new List<Method>();
        public string Name = "";
        public List<Method> LoadClass()
        {
            return new List<Method>();
        }
    }
    public class Method
    {
        internal Method() { }
        internal Method(string name, List<string> keyWords, List<Parameter> parameters)
        {
            this.Name = name;
            this.keyWords = keyWords;
            this.parameters = parameters;
        }
        public string Name = "";
        public List<string> keyWords = new List<string>();
        public List<Parameter> parameters = new List<Parameter>();
        public bool CanOverride => keyWords.Contains("override");
        public Method LoadMethod()
        {
            return new Method();
        }
    }
    public class Parameter
    {
        internal Parameter() { }
        internal Parameter(string ownerClass, string Name, string Ref)
        {
            this.OwnerClass = ownerClass;
            this.Name = Name;
            this.Ref = Ref;
        }
        public string OwnerClass = "";
        public string Name = "";
        public string Ref = "";
    }
}
