using System;
using System.Collections.Generic;
using System.Text;
using Aooshi.Web;

namespace Aooshi.Web
{
    /// <summary>
    /// ҳ����
    /// </summary>
    public class PageNumber
    {

        int totalrows;
        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public int Totalrows
        {
            get { return totalrows; }
            set { totalrows = value; }
        }
        int pagesize;
        /// <summary>
        /// ��ȡ������ҳ��С
        /// </summary>
        public int PageSize
        {
            get { return pagesize; }
            set { pagesize = value; ; }
        }
        int pageindex;
        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public int PageIndex
        {
            get { return pageindex; }
            set { pageindex = value; }
        }
        int lastindex;
        /// <summary>
        /// ��ȡ��������ҳ��
        /// </summary>
        public int LastIndex
        {
            get { return lastindex; }
            set { lastindex = value; }
        }
        int nextindex;
        /// <summary>
        /// ��ȡ��������һҳ����
        /// </summary>
        public int NextIndex
        {
            get { return nextindex; }
            set { nextindex = value; }
        }
        int preindex;
        /// <summary>
        /// ��ȡ��������һҳ����
        /// </summary>
        public int PrevIndex
        {
            get { return preindex; }
            set { preindex = value; }
        }

        long recordindex;
        /// <summary>
        /// ��ȡ������ʼ��¼��
        /// </summary>
        public long RecordIndex
        {
            get { return recordindex; }
        }

        int btncount = 8;

        /// <summary>
        /// ��ȡ������һ�����ݱ������ñ�����ʾ���������ʾ�İ�ť������ע�⣬��ֵӦ��Ϊż����Ĭ��Ϊ8
        /// </summary>
        public int BtnCount
        {
            get { return this.btncount;}
            set { this.btncount = value;}
        }

        int _Style = 0;
        /// <summary>
        /// ��ȡ������һ�����ݱ������ñ�����ʾ���������ʾ����ʽ
        /// </summary>
        public int Style
        {
            get { return this._Style; }
            set { this._Style = value; }
        }

        int _GoStyle = 0;
        /// <summary>
        /// ��ȡ������һ�����ݱ������ñ�����ʾ���������ʾ��ת��ʽ
        /// </summary>
        public int GoStyle
        {
            get { return this._GoStyle; }
            set { this._GoStyle = value; }
        }

        bool _NoDataDisplay = false;

        /// <summary>
        /// ��δ������ʱ�Ƿ����������ʾ
        /// </summary>
        public bool NoDataDisplay
        {
            get { return this._NoDataDisplay; }
            set { this._NoDataDisplay = value; }
        }

        string _QueryNameIndex = "PageIndex"; 
        /// <summary>
        /// ��ȡ������URL���ݲ�����ҳ���������ƣ�Ĭ��Ϊ: PageIndex
        /// </summary>
        public string QueryNameIndex
        {
            get { return this._QueryNameIndex; }
            set { this._QueryNameIndex = value; }
        }

        string _QueryNameTotalrows = "Totalrows";
        /// <summary>
        /// ��ȡ������URL���ݲ��������������ƣ�Ĭ��Ϊ��Totalrows
        /// </summary>
        public string QueryNameTotalrows
        {
            get { return _QueryNameTotalrows; }
            set { _QueryNameTotalrows = value; }
        }


        /// <summary>
        /// ��ʼ����ʵ��
        ///    --�÷�:
        ///    PageNumber pn = new PageNumber();
        ///    --���ݴ���
        ///    pn.DisposePageIndex('����');
        ///    --��ȡֵ
        /// </summary>
        public PageNumber() : this(10, null,null) { }
        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        /// <param name="PageSize">ҳ��С</param>
        public PageNumber(int PageSize) : this(PageSize, null,null) { }
        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        /// <param name="PageSize">ҳ��С</param>
        /// <param name="QueryNameIndex">��ѯ������������ѯ��,��ΪNullʱĬ��ΪPageIndex</param>
        /// <param name="QueryNameTotalrows">��ѯ������������ѯ��,��ΪNullʱĬ��ΪTotalrows</param>
        public PageNumber(int PageSize, string QueryNameIndex,string QueryNameTotalrows)
        {
            if (!string.IsNullOrEmpty(QueryNameIndex)) this.QueryNameIndex = QueryNameIndex;
            else this.QueryNameIndex = "PageIndex";

            if (!string.IsNullOrEmpty(QueryNameTotalrows)) this.QueryNameTotalrows = QueryNameTotalrows;
            else this.QueryNameTotalrows = "Totalrows";


            if (!int.TryParse(WebCommon.Query(this.QueryNameIndex), out pageindex)) this.pageindex = 1;
            if (!int.TryParse(WebCommon.Query(this.QueryNameTotalrows), out totalrows)) this.totalrows = 0;

            pagesize = PageSize;
        }

        string _link = "";

        /// <summary>
        /// ����URL�ַ���
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="request">ҳ����</param>
        public string CreateLink(int index, System.Web.HttpRequest request)
        {
            return this.CreateLink(index, request, null);
        }

        /// <summary>
        /// ����URL�ַ���
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="request">ҳ����</param>
        /// <param name="exclude">ԭʼ���Ӳ�ѯ�е����ų�</param>
        public string CreateLink(int index,System.Web.HttpRequest request,string[] exclude)
        {
            if (this._link == "")
            {
                this._link = string.Format("?{0}={1}",this.QueryNameTotalrows,this.totalrows);

                if (exclude == null) exclude = new string[] { };
                System.Collections.Specialized.NameValueCollection qs = request.QueryString;
                foreach(string key in qs.AllKeys)
                {
                    if (string.IsNullOrEmpty(key)) continue;
                    if (key == this.QueryNameIndex) continue;
                    if (key == this.QueryNameTotalrows) continue;
                    if (Array.IndexOf(exclude, key) != -1) continue;
                    this._link += string.Format("&{0}={1}",key,System.Web.HttpUtility.UrlEncode(qs[key] ?? ""));
                }
            }

            return this._link + string.Format("&{0}={1}",this.QueryNameIndex,index);
        }

        string _vlink = "";

        /// <summary>
        /// ���ݴ����querys���������鴴��һ������URL��ַ
        /// </summary>
        /// <param name="index">����</param>
        /// <param name="querys">Ҫ�����Ĳ�ѯ���</param>
        /// <param name="compart">�ָ������磺 - �� _ ��</param>
        /// <param name="request">��ѯ����</param>
        public string CreateLink(int index, string[] querys,string compart, System.Web.HttpRequest request)
        {
            if (_vlink == "")
            {
                System.Collections.Specialized.NameValueCollection qs = request.QueryString;
                foreach (string key in querys)
                {
                    if (key == this.QueryNameIndex)
                    {
                        _vlink += "{PageIndex}";
                        continue; 
                    }
                    _vlink += compart + System.Web.HttpUtility.UrlEncode(qs[key] ?? "");
                }

                if (_vlink != "")
                    _vlink = _vlink.Substring(1);
            }

            return _vlink.Replace("{PageIndex}",index.ToString());
        }


        /// <summary>
        /// ����ҳ��
        /// </summary>
        /// <param name="totalrows">��ҳ��</param>
        public void DisposePageIndex(int totalrows)
        {
            lastindex = Convert.ToInt32(Math.Ceiling((double)totalrows / pagesize));

            if (pageindex < 1) pageindex = 1;
            if (pageindex > lastindex) pageindex = lastindex;

            nextindex = pageindex + 1;
            preindex = pageindex - 1;
            if (nextindex > lastindex) nextindex = lastindex;
            if (preindex < 1) preindex = 1;


            this.totalrows = totalrows;

            recordindex = (pageindex - 1) * pagesize;
        }


        /// <summary>
        /// ����ҳ��
        /// </summary>
        public void DisposePageIndex()
        {
            this.DisposePageIndex(this.totalrows);
        }
    }
}