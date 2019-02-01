using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Aooshi.Web
{
    /// <summary>
    /// Xmlõ��������
    /// </summary>
    public class WebEnum :XmlEnum
    {
        /// <summary>
        /// Ĭ�ϵ������ļ�·��
        /// </summary>
        public const string FilePath = "~/Aooshi/XmlEnum.Config";
        /// <summary>
        /// ��ʼ����ʵ��
        /// </summary>
        /// <param name="enumName">õ����������</param>
        public WebEnum(string enumName) : base(HttpContext.Current.Server.MapPath(FilePath), enumName) { }


        /// <summary>
        /// ����Bind���ݵĿؼ�
        /// </summary>
        /// <param name="ctl">�󶨶���</param>
        public void Bind(ListControl ctl)
        {
            ctl.DataSource = this.EnumList;
            ctl.DataTextField = "Name";
            ctl.DataValueField = "Num";
            ctl.DataBind();
        }

        /// <summary>
        /// ���б��
        /// </summary>
        /// <param name="rep">�б����</param>
        public void Bind(Repeater rep)
        {
            rep.DataSource = this.EnumList;
            rep.DataBind();
        }
        /// <summary>
        /// �����б��
        /// </summary>
        /// <param name="dli">�б����</param>
        public void Bind(BaseDataList dli)
        {
            dli.DataSource = this.EnumList;
            dli.DataBind();
        }
        /// <summary>
        /// �����б��
        /// </summary>
        /// <param name="dbc">�б����</param>
        public void Bind(BaseDataBoundControl dbc)
        {
            dbc.DataSource = this.EnumList;
            dbc.DataBind();
        }
        /// <summary>
        /// �����б��
        /// </summary>
        /// <param name="slt">�б����</param>
        public void Bind(HtmlSelect slt)
        {
            slt.DataSource = this.EnumList;
            slt.DataTextField = "Name";
            slt.DataValueField = "Num";
            slt.DataBind();
        }
    }
}


//ListControl -> DataBoundControl -> BaseDataBoundControl
//GridView -> CompositeDataBoundControl -> DataBoundControl 
//DetailsView -> CompositeDataBoundControl
//FormView -> CompositeDataBoundControl
//DataList -> BaseDataList
//DataGrid -> BaseDataList 
//Repeater