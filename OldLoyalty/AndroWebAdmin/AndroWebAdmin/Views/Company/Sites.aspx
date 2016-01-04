<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.CompanyViewData>" %>
<%@ Import Namespace="AndroWebAdmin.Mvc.Utilities"%>
<%@ Import Namespace="AndroWebAdmin.Controllers"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Company Site Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.Company.Name %> Site Administration
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
                            <td class="article_indent text-page text-page-indent"></td><td></td><td></td><td><%= Html.ActionLink("Add New Site", "AddSite/" + Model.Company.Id, "Company")%></td>
                        </tr>
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                    <% foreach (var site in Model.Sites)
                           //todo: site Loyalty accounts link
                           {%>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><h2><%= site.Name%></h2></td><td><strong>Loyalty Id: </strong><%=site.Id.Value %></td><td><strong>Rameses Site Id: </strong><%= site.RamesesSiteId %></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= site.Country.Name%></td><td><%= site.SiteLoyaltyAccounts.Count%> Loyalty Accounts</td><td><%= site.SiteTransactionHistories.Count%> Transactions</td><td><%= Html.ActionLink("Site Logs", "SiteLog/" + site.Id.Value, "Global")%></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"></td><td></td><td></td><td><%= Html.ActionLink("Edit Site", "EditSite/" + site.Id.Value, "Company")%></td>
                            </tr>
                          <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                          </tr>
                          <%} %>
                          
                          </table>

                        
                    </div>	

                </div>
	        </td>
        </tr>
    </table>
</div>

</asp:Content>
