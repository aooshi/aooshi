/*===========================================================================
 * 
 *   Author £º  KaiSin
 *   Name   £º  Ajax
 *   Link   £º  http://x.Aooshi.Com/Script/eAjax.1.2.js
 *   Create £º  2007/8/16
 *   Modify £º  2008/1/30
 *   Description £º debug to is url index of "ajaxdebug=open" or set property debug=true
 *  
 ===========================================================================*/
 var _eAjaxDebug = false
function eAjax()
{
   var Pms           = new Array();
   var ContentType   = "";
   var HttpXml       = null;
   var p_callBack    = null;
   var p_isPost      = false;
   var p_Url         = "";
   var p_callError   = null;
   var isDebug       = false;
   this.version       = 1.2;
   
   function eAjaxDebug(msg){if (isDebug || _eAjaxDebug){ alert(msg)};}
   function Create()
   {
      if (window.XMLHttpRequest)
      {
		 eAjaxDebug("new XMLHttpRequest();");
         return new XMLHttpRequest();
      }
      else if (ActiveXObject)
      {
        var XmlList = new Array("MSXML4","MSXML3","MSXML2","MSXML","Microsoft");
        for(var i=0;i<XmlList.length;i++)
        {
           try
           {
             return new ActiveXObject(XmlList[i] + ".XMLHTTP");
           }
           catch(e)
           {
		     eAjaxDebug(XmlList[i] + "\n" + e.toString());
           }
        }
      }
      return undefined;
   }
   function GetParms()
   {
     var Pm = "";
     if (Pms && Pms.length>0)
     {
          Pm = Pms[0][0] + "="  + encodeURIComponent(Pms[0][1]);//.replace("+","%2b")
          for(var i=1;i<Pms.length;i++)
              Pm += "&" + Pms[i][0] + "=" + encodeURIComponent(Pms[i][1]);
      }
      return Pm;
   }
   function Ajax_GetUrl(URL,Query)
   {
      if (!Query) return URL;
      if (URL.indexOf("?") != -1)
      {
        if (URL.lastIndexOf("?") == (URL.length - 1)) return URL+Query;
        return URL + "&" + Query;
      }
      return URL + "?" + Query;
   }
   function cAjaxObject(hName,hType,Txt)
   {
	 eAjaxDebug(Txt);
     var obj;
     if (hName && hName.length > 0)
     {
        eval(Txt);
        if (hType == "ARRAY") return eval(hName);
        if (hType == "CLASS") return eval("new " + hName);
        return Txt;
     }
     return Txt;
   }
   function eAjax_Error(em,txt){
        eAjaxDebug(txt || em);
        if (p_callError)
        {
            p_callError(em,txt);
            return;
        }
        else
        {
            if (p_callBack) p_callBack(em);
            else return null;
        }
   }
   function eAjax_Response()
   {
      if (HttpXml.readyState == 4)
      { 
            if (HttpXml.status == 200)
            {
                  var hName = HttpXml.getResponseHeader("X-Aooshi.Ajax-NAME");
                  var hType = HttpXml.getResponseHeader("X-Aooshi.Ajax-TYPE");
                  var cType = HttpXml.getResponseHeader("Content-Type");
                  var isXML = (cType && cType.indexOf('text/xml') != -1);  //is have xml
                  
		          if (cType) eAjaxDebug("Head Content-type:" + cType);
		          
                  if (p_callBack)
                  {
		                if (isXML) p_callBack(HttpXml.responseXML);
                        else p_callBack(cAjaxObject(hName,hType,HttpXml.responseText));
                  }
                  else
                  {
		                if (isXML) return (HttpXml.responseXML);
                        else return cAjaxObject(hName,hType,HttpXml.responseText);
                  }
            }
            else
            {
                var em = "";
                var txt = HttpXml.responseText;
                switch(HttpXml.status)    
                {
                    case 404:
                        //em = "Î´ÕÒµ½Òª²Ù×÷µÄÍøÒ³,´úÂë:404!";
                        em = "not find remote page,404!";
                        break;
                    case 500:
                        //em = "ÍøÒ³³ÌÐò´íÎó,´úÂë:500!";
                        em = "web application error,500!";
                        break;
                    default:
                        //em = "ÍøÒ³´íÎó,´íÎó´úÂë:" + HttpXml.status + "!";
                        em = "web application error:" + HttpXml.status + "!";
                        break;
                }
	            return eAjax_Error(em,txt);
	        }
       }
   }
   this.callBack     = null;
   this.callError    = null;
   this.debug        = null;
   this.Add = function(Name,Value){ Pms[Pms.length] = new Array(Name,Value); }
   this.Send = function(url,method){
       p_Url          = url;
       p_isPost       = (method == "POST");
       p_callBack     = this.callBack;
       p_callError    = this.callError;
       isDebug        = this.debug || (top.location.href.indexOf('ajaxdebug=open') != -1);
       var sData;
       
       
       if (!p_Url) return eAjax_Error("not set ajax action url!");
       if (!(HttpXml = Create())) return eAjax_Error("is browser not sustain ajax;");
       
       if (p_isPost)
       {
           ContentType   = "application/x-www-form-urlencoded";
           sData = GetParms();
           eAjaxDebug("post datas:" + sData);
       }
       else
       {
           ContentType   = "text/xml";
           this.Add("AjaxTempSessionID",Math.random());
           p_Url = Ajax_GetUrl(p_Url,GetParms());
           sData = null;
           eAjaxDebug("get url:" + p_Url);
       }
         if (p_callBack)
         {
            HttpXml.onreadystatechange = eAjax_Response;
            HttpXml.open(method,p_Url,true);
            HttpXml.setRequestHeader("Content-Type", ContentType);
            HttpXml.send(sData);
         }
         else
         {
            HttpXml.open(method,p_Url,false);
            HttpXml.setRequestHeader("Content-Type", ContentType);
            HttpXml.send(sData);
            return eAjax_Response();
         }
   }
   this.get = function(url){return this.Send(url,"GET");}
   this.post = function(url){return this.Send(url,"POST");}
}