<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyAccountViewData>"%>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.AccountStatus.Name %> Loyalty Accounts
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.AccountStatus.AccountStatuses.Count %> <%= Model.AccountStatus.Name %> Loyalty Accounts
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
                                <td><h2>Account Id</h2></td><td><h2>Company</h2></td><td><h2>Points</h2></td><td></td>
                            </tr>
                            <tr >
                                <td colspan="4"><span class="article_separator"></span></td>
                            </tr>
                            <%
                                foreach (var loyaltyAccount in Model.LoyaltyAccounts)
                               {
                               %>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= loyaltyAccount.Id %></td><td><%=loyaltyAccount.Site.Company.Name%></td><td><%= loyaltyAccount.Points%></td><td><%= Html.ActionLink("View Details", "Details/" + loyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
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
