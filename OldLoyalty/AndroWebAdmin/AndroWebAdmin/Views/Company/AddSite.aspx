<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.CompanyViewData>" %>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add New Site For <%=Model.Company.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%=Model.Company.Name %> New Site Administration
                </div>
            </div>
        </div>
    </div>

    <table cellpadding="0" cellspacing="0">
        <tr>
	        <td valign="top">
		        <div>
                    <div class="content-text">
                    <table class="contentpaneopen">
                            <tr>
                                <td class="article_indent text-page text-page-indent"><h2>New Site</h2></td><td></td><td></td>
                            </tr>
                            <% using (Html.BeginForm("SaveSite","Company",FormMethod.Post))
                               {
                                   
                            %>
                            <%= Html.Hidden("Site.Company.Id", Model.Company.Id)%>
                            <%= Html.Hidden("Site.Country.Id", Model.Company.Country.Id)%>
                            <tr>
                                <td>Site Name</td><td>Rameses Site Id </td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("Name") %></td><td><%= Html.TextBox("RamesesSiteId")%></td><td></td>
                            </tr>
                            <tr>
                                <td>Key</td><td>Password</td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("SiteKey")%></td><td><%= Html.TextBox("SitePassword")%></td><td></td>
                            </tr>
                            
                            <tr>
                                <td class="article_indent text-page text-page-indent"></td><td></td><td><input type="submit" value="save" /></td>
                            </tr>
                            <%} %>
                          <tr >
                            <td colspan="3"><span class="article_separator"></span></td>
                          </tr>
                          <tr>
                                <td colspan="3">By default new sites inherit <%= Model.Company.Name%> country (<%= Model.Company.Country.Name%>)</td>
                            </tr>
                      </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>
</asp:Content>
