using System;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace Aooshi.Web
{
    /// <summary>
    /// Xml玫举配置项
    /// </summary>
    public class WebEnum :XmlEnum
    {
        /// <summary>
        /// 默认的配置文件路径
        /// </summary>
        public const string FilePath = "~/Aooshi/XmlEnum.Config";
        /// <summary>
        /// 初始化新实例
        /// </summary>
        /// <param name="enumName">玫举配置名称</param>
        public WebEnum(string enumName) : base(HttpContext.Current.Server.MapPath(FilePath), enumName) { }


        /// <summary>
        /// 绑定有Bind数据的控件
        /// </summary>
        /// <param name="ctl">绑定对象</param>
        public void Bind(ListControl ctl)
        {
            ctl.DataSource = this.EnumList;
            ctl.DataTextField = "Name";
            ctl.DataValueField = "Num";
            ctl.DataBind();
        }

        /// <summary>
        /// 简单列表绑定
        /// </summary>
        /// <param name="rep">列表对象</param>
        public void Bind(Repeater rep)
        {
            rep.DataSource = this.EnumList;
            rep.DataBind();
        }
        /// <summary>
        /// 数据列表绑定
        /// </summary>
        /// <param name="dli">列表对象</param>
        public void Bind(BaseDataList dli)
        {
            dli.DataSource = this.EnumList;
            dli.DataBind();
        }
        /// <summary>
        /// 数据列表绑定
        /// </summary>
        /// <param name="dbc">列表对象</param>
        public void Bind(BaseDataBoundControl dbc)
        {
            dbc.DataSource = this.EnumList;
            dbc.DataBind();
        }
        /// <summary>
        /// 数据列表绑定
        /// </summary>
        /// <param name="slt">列表对象</param>
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