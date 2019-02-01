using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Aooshi
{
    /// <summary>
    /// 简单配置项
    /// </summary>
    /// <remarks>
        /// <code>
        ///     <root>
        ///         <a>内容</a>
        ///         <b>内容</b>
        ///     </root>
        /// </code>
    /// </remarks>
    public class EasyConfig
    {

        /// <summary>
        /// 创建一个简易配置
        /// </summary>
        public class CreateConfig
        {
            XmlDocument document;

            internal CreateConfig(Encoding enocding)
            {
                document = new XmlDocument();
                document.AppendChild(document.CreateXmlDeclaration("1.0",enocding.BodyName, null));
                document.AppendChild(document.CreateElement("Root"));
            }

            /// <summary>
            /// 添加一组配置
            /// </summary>
            /// <param name="list">配置列表</param>
            public void AddRange(Dictionary<string,string> list)
            {
                if (list == null) throw new ArgumentNullException("list");
                Dictionary<string,string>.Enumerator e = list.GetEnumerator();
                while (e.MoveNext())
                {
                    this.AddElement(e.Current.Key, e.Current.Value);
                }
            }

            /// <summary>
            /// 添加一个配置节
            /// </summary>
            /// <param name="name">名称</param>
            /// <param name="value">配置值</param>
            public XmlElement AddElement(string name, string value)
            {
                XmlElement element = this.document.CreateElement(name);
                element.InnerXml = value;
                this.document.DocumentElement.AppendChild(element);
                return element;
            }

            /// <summary>
            /// 获取当前Xml对象
            /// </summary>
            public XmlDocument Xml
            {
                get
                {
                    return this.document;
                }
            }

            /// <summary>
            /// 保存文档至指定的文件
            /// </summary>
            /// <param name="path">文件</param>
            public void Save(string path)
            {
                this.Xml.Save(path);
            }

            /// <summary>
            /// 保存文档至指定的流
            /// </summary>
            /// <param name="outstream">输出流</param>
            public void Save(Stream outstream)
            {
                this.Xml.Save(outstream);
            }
        }


        XmlDocument doc;
        XmlElement root;

        /// <summary>
        /// 初始化一个配置
        /// </summary>
        /// <param name="path">配置路径</param>
        public EasyConfig(string path):this(path,true)
        {
        }

        /// <summary>
        /// 初始化一个配置
        /// </summary>
        /// <param name="str">xml字符或xml文件路径</param>
        /// <param name="ispath">当str为Xml文件路径时，该参数为true,否则str参数传入的应该为xml字符串</param>
        public EasyConfig(string str, bool ispath)
        {
            doc = new XmlDocument();

            if (ispath)
            {
                if (string.IsNullOrEmpty(str)) throw new ArgumentNullException("Path");
                if (!File.Exists(str)) throw new FileNotFoundException(string.Format("Not Find '{0}';", str));
                doc.Load(str);
            }
            else
            {
                doc.LoadXml(str);
            }
            root = doc.DocumentElement;
        }

        
        /// <summary>
        /// 以UTF-8编码创建一个简易配置
        /// </summary>
        public static CreateConfig Create()
        {
            return EasyConfig.Create(Encoding.UTF8);
        }

        /// <summary>
        /// 创建一个简易配置
        /// </summary>
        /// <param name="encoding">配置编码</param>
        public static CreateConfig Create(Encoding encoding)
        {
            CreateConfig cc = new CreateConfig(encoding);
            return cc;
        }

        /// <summary>
        /// 创建一个简易配置，并添加一组数据
        /// </summary>
        /// <param name="items">数组</param>
        /// <param name="encoding">编辑</param>
        public static CreateConfig Create(Dictionary<string, string> items, Encoding encoding)
        {
            CreateConfig cc = new CreateConfig(encoding);
            cc.AddRange(items);
            return cc;
        }


        /// <summary>
        /// 获取一个配置值
        /// </summary>
        /// <param name="name">配置项名称</param>
        public string this[string name]
        {
            get
            {
                XmlNode xn = root.SelectSingleNode(name);
                if (xn == null) return "";
                return xn.InnerText;
            }
        }

        /// <summary>
        /// 获取一个配置的一个属性，如果未有属性则返回 string.Empty
        /// </summary>
        /// <param name="name">配置项名称</param>
        /// <param name="attrname">属性名</param>
        public string Attribute(string name, string attrname)
        {
            XmlNode node = root.SelectSingleNode(name);
            if (node == null || node.Attributes[attrname] == null) return "";
            return node.Attributes[attrname].InnerText;
        }

        /// <summary>
        /// 获取一个配置节
        /// </summary>
        /// <param name="name">名称</param>
        public XmlElement GetElement(string name)
        {
            return (XmlElement)root.SelectSingleNode(name);
        }

        /// <summary>
        /// 判断是否已配置一个指定的配置
        /// </summary>
        /// <param name="name">配置名称</param>
        public bool IsExists(string name)
        {
            return root.SelectSingleNode(name) != null;
        }
    }
}
