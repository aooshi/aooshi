using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

namespace Aooshi
{
    /// <summary>
    /// ��������
    /// </summary>
    /// <remarks>
        /// <code>
        ///     <root>
        ///         <a>����</a>
        ///         <b>����</b>
        ///     </root>
        /// </code>
    /// </remarks>
    public class EasyConfig
    {

        /// <summary>
        /// ����һ����������
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
            /// ���һ������
            /// </summary>
            /// <param name="list">�����б�</param>
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
            /// ���һ�����ý�
            /// </summary>
            /// <param name="name">����</param>
            /// <param name="value">����ֵ</param>
            public XmlElement AddElement(string name, string value)
            {
                XmlElement element = this.document.CreateElement(name);
                element.InnerXml = value;
                this.document.DocumentElement.AppendChild(element);
                return element;
            }

            /// <summary>
            /// ��ȡ��ǰXml����
            /// </summary>
            public XmlDocument Xml
            {
                get
                {
                    return this.document;
                }
            }

            /// <summary>
            /// �����ĵ���ָ�����ļ�
            /// </summary>
            /// <param name="path">�ļ�</param>
            public void Save(string path)
            {
                this.Xml.Save(path);
            }

            /// <summary>
            /// �����ĵ���ָ������
            /// </summary>
            /// <param name="outstream">�����</param>
            public void Save(Stream outstream)
            {
                this.Xml.Save(outstream);
            }
        }


        XmlDocument doc;
        XmlElement root;

        /// <summary>
        /// ��ʼ��һ������
        /// </summary>
        /// <param name="path">����·��</param>
        public EasyConfig(string path):this(path,true)
        {
        }

        /// <summary>
        /// ��ʼ��һ������
        /// </summary>
        /// <param name="str">xml�ַ���xml�ļ�·��</param>
        /// <param name="ispath">��strΪXml�ļ�·��ʱ���ò���Ϊtrue,����str���������Ӧ��Ϊxml�ַ���</param>
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
        /// ��UTF-8���봴��һ����������
        /// </summary>
        public static CreateConfig Create()
        {
            return EasyConfig.Create(Encoding.UTF8);
        }

        /// <summary>
        /// ����һ����������
        /// </summary>
        /// <param name="encoding">���ñ���</param>
        public static CreateConfig Create(Encoding encoding)
        {
            CreateConfig cc = new CreateConfig(encoding);
            return cc;
        }

        /// <summary>
        /// ����һ���������ã������һ������
        /// </summary>
        /// <param name="items">����</param>
        /// <param name="encoding">�༭</param>
        public static CreateConfig Create(Dictionary<string, string> items, Encoding encoding)
        {
            CreateConfig cc = new CreateConfig(encoding);
            cc.AddRange(items);
            return cc;
        }


        /// <summary>
        /// ��ȡһ������ֵ
        /// </summary>
        /// <param name="name">����������</param>
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
        /// ��ȡһ�����õ�һ�����ԣ����δ�������򷵻� string.Empty
        /// </summary>
        /// <param name="name">����������</param>
        /// <param name="attrname">������</param>
        public string Attribute(string name, string attrname)
        {
            XmlNode node = root.SelectSingleNode(name);
            if (node == null || node.Attributes[attrname] == null) return "";
            return node.Attributes[attrname].InnerText;
        }

        /// <summary>
        /// ��ȡһ�����ý�
        /// </summary>
        /// <param name="name">����</param>
        public XmlElement GetElement(string name)
        {
            return (XmlElement)root.SelectSingleNode(name);
        }

        /// <summary>
        /// �ж��Ƿ�������һ��ָ��������
        /// </summary>
        /// <param name="name">��������</param>
        public bool IsExists(string name)
        {
            return root.SelectSingleNode(name) != null;
        }
    }
}
