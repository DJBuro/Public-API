<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.CompanyViewData>" %>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit <%=Model.Site.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%=Model.Site.Name %> Administration
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
                    <% using (Html.BeginForm("SaveSite","Company",FormMethod.Post))
                           {
                            %>
                    <%= Html.Hidden("Id",Model.Site.Id) %>
                            <tr>
                                <td>Site Name</td><td>Rameses Site Id </td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("Name",Model.Site.Name) %></td><td><%= Html.TextBox("RamesesSiteId", Model.Site.RamesesSiteId)%></td><td></td>
                            </tr>
                            <tr>
                                <td>Key</td><td>Password</td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("SiteKey", Model.Site.SiteKey)%></td><td><%= Html.TextBox("SitePassword", Model.Site.SitePassword)%></td><td><a>Delete Site</a></td>
                            </tr>
                            
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.DropDownList("Country.Id", Model.CountryListItems) %></td><td></td><td><input type="submit" value="save" /></td>
                            </tr>
                            <%} %>
                          <tr >
                            <td colspan="3"><span class="article_separator"></span></td>
                          </tr>
                           <tr>
                                <td class="article_indent text-page text-page-indent"><%= Model.Site.Company.Name%></td><td><%= Model.Site.SiteLoyaltyAccounts.Count%> Loyalty Accounts</td><td><%= Model.Site.SiteTransactionHistories.Count%> transactions</td>
                            </tr>
                      </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>
</asp:Content>
