using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace Aooshi
{
    /// <summary>
    /// Xml用玫举配置
    /// </summary>
    public class XmlEnum
    {
        #region 玫举项
        /// <summary>
        /// 玫举项
        /// </summary>
        public class XmlEnumItem
        {
            XmlNode node;
            string _name;
            int _num;
            /// <summary>
            /// 初始化
            /// </summary>
            internal XmlEnumItem(XmlNode node)
            {
                this.node = node;
                _name = Attr("Name");
                _num = Convert.ToInt32(Attr("Num"));
            }

            /// <summary>
            /// 获取当前玫举的指定属性
            /// </summary>
            /// <param name="Name">玫举名称</param>
            public string Attr(string Name)
            {
                return node.Attributes[Name].InnerText;
            }

            /// <summary>
            /// 获取玫举数值
            /// </summary>
            public int num
            {
                get { return _num; }
            }

            /// <summary>
            /// 获取玫举名称
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
        /// 初始化
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="enumName">玫举项名称</param>
        public XmlEnum(string path,string enumName)
        {
            if (!File.Exists(path)) throw new FileNotFoundException(string.Format("Not Find File {0};",path));
            doc = new XmlDocument();
            doc.Load(path);
            SetEnum(enumName);
        }

        /// <summary>
        /// 玫举列表
        /// </summary>
        public List<XmlEnumItem> EnumList
        {
            get
            {
                return list;
            }
        }

        /// <summary>
        /// 获取指定玫举名称的序号,如果未找到则返回为-1
        /// </summary>
        /// <param name="Name">玫举名称</param>
        public int GetEnumNum(string Name)
        {
            foreach (XmlEnumItem item in list)
            {
                if (item.Name == Name) return item.num;
            }

            return -1;
        }

        /// <summary>
        /// 获取指定玫举值的名称,未找到则返回为空字符串
        /// </summary>
        /// <param name="num">玫举序号</param>
        public string GetEnumName(int num)
        {
            foreach (XmlEnumItem item in list)
            {
                if (item.num == num) return item.Name;
            }

            return "";
        }

        /// <summary>
        /// 将对象设置到指定玫举
        /// </summary>
        /// <param name="enumName">玫举名称</param>
        public void SetEnum(string enumName)
        {
            XmlNode n = doc.DocumentElement.SelectSingleNode(enumName);
            if (n == null) throw new XmlException(string.Format("未找到节点 {0} ;",enumName));
            XmlNodeList en = n.SelectNodes("item");
            list = new List<XmlEnumItem>();
            foreach (XmlNode nd in en)
                list.Add(new XmlEnumItem(nd));
        }

        /// <summary>
        /// 获取指定玫举的玫举列表
        /// </summary>
        /// <param name="enumName">玫举名</param>
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
