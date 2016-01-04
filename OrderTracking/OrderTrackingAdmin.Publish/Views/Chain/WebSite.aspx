<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<OrderTrackingAdminViewData.ClientViewData>" validateRequest="false" %>
<%@ Import Namespace="OrderTrackingAdmin.Mvc.Extensions"%>
<%@ Import Namespace="OrderTracking.Dao.Domain"%>
<%@ Import Namespace="OrderTrackingAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Website
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
<% 
    bool addColourPicker = false;
    bool addTextEditor = false;

    foreach (KeyValuePair<string, List<InjectionPoint>> keyValuePair in Model.InjectionPoints)
    {
        foreach (InjectionPoint injectionPoint in keyValuePair.Value)
        {
            if (injectionPoint.Type == "RichText")
            {
                addTextEditor = true;
            }
            else if (injectionPoint.Type == "Colour")
            {
                addColourPicker = true;
            }
        }
    }
    
    if (addColourPicker)
    { 
%>
    <script type="text/javascript" src="../../Content/farbtastic.js"></script>
    <link rel="stylesheet" href="../../Content/farbtastic.css" type="text/css" />
<%
    } 
    
    if (addTextEditor)
    {
%>
    <script type="text/javascript" src="../../Content/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript">
      <!--
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
      //-->
   </script>
<%
    }
%>    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Customize Website</h2>
    <table>
        <tr>
            <td style="width:150px;"><%=Html.Resource("Master, WebSiteTemplate")%>:</td>
            <td><%=Model.Client.WebsiteTemplateName%></td>
        </tr>
        <tr>
            <td style="width:150px;">Live Url:</td>
            <td>
                <% 
                    if (Model.Url == null || Model.Url.Length == 0)
                    {
                %>
                    <%=Html.Resource("Master, NoWebSite")%>
                <%
                    }
                    else
                    {
                        string link = "<a href=\"" + Model.Url + "\" target=\"_blank\">" + Model.Url + "</a>"; 
                %>
                    <%= link%>
                <%
                    } 
                %>
            </td>
        </tr>
    </table>
    
<%
    string readyScript = "";
    bool addColourPicker = false;

    using (Html.BeginForm("Website", "Chain", FormMethod.Post, new { enctype = "multipart/form-data" }))
    {
%>
<%
            foreach (KeyValuePair<string, List<InjectionPoint>> keyValuePair in Model.InjectionPoints)
            {
                %>
                <h3 style="margin-top:5px;"><%=keyValuePair.Key%></h3>
        <table>
            <%
                
                foreach (InjectionPoint injectionPoint in keyValuePair.Value)
                {
                %>
                <tr>
                    <td style="vertical-align:top; width:200px;"><%= injectionPoint.Prompt%></td>
                    <td>
                <%
                if (injectionPoint.Type == "Text")
                {
                %>
                    <%= Html.TextBox("inj_" + injectionPoint.Id, injectionPoint.Value, new { style = "width:450px;" })%>
                <%
                }
                else if (injectionPoint.Type == "Number")
                {
                %>
                    <%= Html.TextBox("inj_" + injectionPoint.Id, injectionPoint.Value, new { style = "width:450px;", onkeypress = "return isNumberKey(event);" })%>
                <%
                }
                else if (injectionPoint.Type == "RichText")
                {
                    readyScript += "$('#" + "inj_" + injectionPoint.Id + "').tinymce({\r\n" +
                        "script_url : '../../Content/tiny_mce/tiny_mce.js',\r\n" +
                        "mode : 'exact',\r\n" +
                        "theme : 'advanced',\r\n" +
                        "theme_advanced_buttons1 : 'bold,italic,underline,fontselect,fontsizeselect,forecolor,backcolor,|,undo,redo,|,newdocument',\r\n" +
                        "theme_advanced_buttons2 : '',\r\n" +
                        "theme_advanced_buttons3 : '',\r\n" +
                        "theme_advanced_buttons4 : '',\r\n" +
                        "theme_advanced_toolbar_location : 'top',\r\n" +
                        "theme_advanced_toolbar_align : 'left'\r\n" +
                        "});\r\n";                    
                %>
                    <%= Html.TextArea("inj_" + injectionPoint.Id, injectionPoint.Value, 1, 80, new { style = "width:450px; height:50px;" })%>
                <% 
                }
                else if (injectionPoint.Type == "Colour")
                {
                    %>
                       <%= Html.TextBox("inj_" + injectionPoint.Id, injectionPoint.Value, new { style = "width:100px; background-color:" + injectionPoint.Value + ";" })%>
                    <% 

                    readyScript += "$('#inj_" + injectionPoint.Id + "').click(function() { $.farbtastic('#colorpicker').linkTo('#inj_" + injectionPoint.Id + "'); });\r\n";
                    addColourPicker = true;
                }
                else if (injectionPoint.Type == "Image")
                {
                    string fileUpload = "<span>" + injectionPoint.Value + "</span><br /><input type=\"file\" name=\"" + "inj_" + injectionPoint.Id + "\" />\r\n";
                    %>
                       <%= fileUpload%>
                    <% 

                //readyScript += "$('#inj_" + injectionPoint.Id + "').click(function() { $.farbtastic('#colorpicker').linkTo('#inj_" + injectionPoint.Id + "'); });\r\n";
                //addColourPicker = true;
                }
                else if (injectionPoint.Type == "Border")
                {
                    string[] borderChunks = injectionPoint.Value.Split(' ');

                    string colour = borderChunks[0];
                    string top = borderChunks[1];
                    string right = borderChunks[2];
                    string bottom = borderChunks[3];
                    string left = borderChunks[4];
                    
                    %>
                        Colour:
                        <%= Html.TextBox("inj_" + injectionPoint.Id + "_Colour", colour, new { style = "width:80px; background-color:" + colour + ";" })%>
                    <% 

                    readyScript += "$('#inj_" + injectionPoint.Id + "_Colour" + "').click(function() { $.farbtastic('#colorpicker').linkTo('#inj_" + injectionPoint.Id + "_Colour'); });\r\n";
                    addColourPicker = true;
                    %>
                    
                    Top:
                    <%= Html.TextBox("inj_" + injectionPoint.Id + "_Top", top, new { style = "width:30px;", onkeypress = "return isNumberKey(event);" })%>
                    Right:
                    <%= Html.TextBox("inj_" + injectionPoint.Id + "_Right", right, new { style = "width:30px;", onkeypress = "return isNumberKey(event);" })%>
                    Bottom:
                    <%= Html.TextBox("inj_" + injectionPoint.Id + "_Bottom", bottom, new { style = "width:30px;", onkeypress = "return isNumberKey(event);" })%>
                    Left:
                    <%= Html.TextBox("inj_" + injectionPoint.Id + "_Left", left, new { style = "width:30px;", onkeypress = "return isNumberKey(event);" })%>
                <%
                }
                else if (injectionPoint.Type == "Font")
                {
                    List<SelectListItem> fonts = new List<SelectListItem>();
                    
                    SelectListItem selectListItem = new SelectListItem();
                    selectListItem.Text = "Arial";
                    selectListItem.Value = "Arial";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Arial Black";
                    selectListItem.Value = "Arial Black";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Comic Sans MS";
                    selectListItem.Value = "Comic Sans MS";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Courier New";
                    selectListItem.Value = "Courier New";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Georgia";
                    selectListItem.Value = "Georgia";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Impact";
                    selectListItem.Value = "Impact";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Lucida Console";
                    selectListItem.Value = "Lucida Console";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Lucida Sans Unicode";
                    selectListItem.Value = "Lucida Sans Unicode";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Palatino Linotype";
                    selectListItem.Value = "Palatino Linotype";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Tahoma";
                    selectListItem.Value = "Tahoma";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Times New Roman";
                    selectListItem.Value = "Times New Roman";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Trebuchet MS";
                    selectListItem.Value = "Trebuchet MS";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Verdana";
                    selectListItem.Value = "Verdana";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Symbol";
                    selectListItem.Value = "Symbol";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Webdings";
                    selectListItem.Value = "Webdings";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "Wingdings";
                    selectListItem.Value = "Wingdings";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "MS Sans Serif";
                    selectListItem.Value = "MS Sans Serif";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);

                    selectListItem = new SelectListItem();
                    selectListItem.Text = "MS Serif";
                    selectListItem.Value = "MS Serif";
                    selectListItem.Selected = selectListItem.Value == injectionPoint.Value ? true : false;
                    fonts.Add(selectListItem);
                %>
                   <%= Html.DropDownList("inj_" + injectionPoint.Id, fonts, new { style = "width:450px;" })%>
                <%
                }
                %>
                    </td>
                </tr>
            <%
                }
                %>
                </table>
                <%
            }%>
        
            <div style="position:fixed; bottom:0; left:0; width:375px; text-align:justify; margin:0 0 10px 10px;">
                
                <div>
                    <input type="submit" value="<%=Html.Resource("Master, GenerateSite")%>" />
                </div>
                
                <div style="margin-top:10px;">
                    <%= Html.ActionLink(Html.Resource("Master, Return"), "Edit/" + Model.Client.Id.ToString(), "Chain", (object)Model.Client.Id)%> <br />
                </div>
                
                <h3 style="margin-top:15px;">Note</h3>
                <p>Any modifications to the web site may not appear immediately.  This is because web browsers cache web pages on disk.  If you don't see your changes in the live site immediately, you may need to click the refresh button in your web browser a couple of times.</p>
                
            <% if (addColourPicker)
            { 
            %>
                <div style="margin-top:10px;">
                    <h3>To change a colour</h3>
                    <p>Click in any colour text box on the right and then use the colour picker below to change the colour. Click the outer ring first and then the middle square to select a colour.</p>
                    <div id="colorpicker" style="color: red; font-size: 1.4em; margin-top:10px;">jQuery.js is not present. You must install jQuery in this folder for the demo to work.</div>
                </div>                
            <%
            } 
            %>
            </div>
<%
    }
    
    if (addColourPicker)
    {
        readyScript += "$('#colorpicker').farbtastic('#color');\r\n";
    }
        
    if (readyScript.Length > 0)
    {
        readyScript = 
            "<script type=\"text/javascript\" charset=\"utf-8\">\r\n" +
                "$(document).ready(function() {\r\n" +
                readyScript +
                "});\r\n" +
            "</script>\r\n";
    }
%>

<%= readyScript%>
    
</asp:Content>
