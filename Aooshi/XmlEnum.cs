using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Aooshi
{
    /// <summary>
    /// Xml��õ������
    /// </summary>
    public class XmlEnum
    {
        #region õ����
        /// <summary>
        /// õ����
        /// </summary>
        public class XmlEnumItem
        {
            XmlNode node;
            string _name;
            int _num;
            /// <summary>
            /// ��ʼ��
            /// </summary>
            internal XmlEnumItem(XmlNode node)
            {
                this.node = node;
                _name = Attr("Name");
                _num = Convert.ToInt32(Attr("Num"));
            }

            /// <summary>
            /// ��ȡ��ǰõ�ٵ�ָ������
            /// </summary>
            /// <param name="Name">õ������</param>
            public string Attr(string Name)
            {
                return node.Attributes[Name].InnerText;
            }

            /// <summary>
            /// ��ȡõ����ֵ
            /// </summary>
            public int num
            {
                get { return _num; }
            }

            /// <summary>
            /// ��ȡõ������
            /// </summary>
            public string Name
            {
                get { return _name; }
            }

        }

        #endregion

        XmlDocument doc;
        List<XmlEnumItem> list;
        /// <summary>
        /// ��ʼ��
        /// </summary>
        /// <param name="path">·��</param>
        /// <param name="enumName">õ��������</param>
        public XmlEnum(string path,string enumName)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(string.Format("Not Find File {0};",path));
            doc = new XmlDocument();
            doc.Load(path);
            SetEnum(enumName);
        }

        /// <summary>
        /// õ���б�
        /// </summary>
        public List<XmlEnumItem> EnumList
        {
            get
            {
                return list;
            }
        }

        /// <summary>
        /// ��ȡָ��õ�����Ƶ����,���δ�ҵ��򷵻�Ϊ-1
        /// </summary>
        /// <param name="Name">õ������</param>
        public int GetEnumNum(string Name)
        {
            foreach (XmlEnumItem item in list)
            {
                if (item.Name == Name) return item.num;
            }

            return -1;
        }

        /// <summary>
        /// ��ȡָ��õ��ֵ������,δ�ҵ��򷵻�Ϊ���ַ���
        /// </summary>
        /// <param name="num">õ�����</param>
        public string GetEnumName(int num)
        {
            foreach (XmlEnumItem item in list)
            {
                if (item.num == num) return item.Name;
            }

            return "";
        }

        /// <summary>
        /// ���������õ�ָ��õ��
        /// </summary>
        /// <param name="enumName">õ������</param>
        public void SetEnum(string enumName)
        {
            XmlNode n = doc.DocumentElement.SelectSingleNode(enumName);
            if (n == null) throw new XmlException(string.Format("δ�ҵ��ڵ� {0} ;",enumName));
            XmlNodeList en = n.SelectNodes("item");
            list = new List<XmlEnumItem>();
            foreach (XmlNode nd in en)
                list.Add(new XmlEnumItem(nd));
        }

        /// <summary>
        /// ��ȡָ��õ�ٵ�õ���б�
        /// </summary>
        /// <param name="enumName">õ����</param>
        public List<XmlEnumItem> GetEnumList(string enumName)
        {
            XmlNodeList en = doc.DocumentElement.SelectSingleNode(enumName).SelectNodes("item");
            List<XmlEnumItem>  list = new List<XmlEnumItem>();
            foreach (XmlNode nd in en)
                list.Add(new XmlEnumItem(nd));

            return list;
        }
    }
}
