<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyAccountViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Loyalty Accounts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.LoyaltyAccounts.Count %> Loyalty Accounts
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
                            <td class="article_indent text-page text-page-indent"></td><td></td><td></td><td></td>
                        </tr>
                         <%
                        foreach (Company company in Model.Companies)
                        {%>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= company.Name %></h2></td><td><%= Html.ActionLink("View Cards", "Company/" + company.Id.Value, "LoyaltyCard")%></td><td></td><td><%= Html.ActionLink("View Accounts", "Company/" + company.Id.Value, "LoyaltyCard")%></td>
                        </tr>
                        <%} %>
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                        <%
                            foreach (AccountStatus status in Model.AccountStatuses)
                       {
                       %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= status.AccountStatuses.Count%> <%= status.Name %></h2></td><td><%= status.Description %></td><td></td><td><%= Html.ActionLink("View Accounts", "Status/" + status.Id.Value, "LoyaltyAccount")%></td>
                        </tr>
                        
                          <%} %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"></td><td></td><td></td><td></td>
                        </tr>

                          </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>
</asp:Content>
