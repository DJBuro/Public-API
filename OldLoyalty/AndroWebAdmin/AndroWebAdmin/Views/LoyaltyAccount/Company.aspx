<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyAccountViewData>"  %>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Company.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.Company.Name %> Loyalty Account Administration
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
                            <td class="article_indent text-page text-page-indent"></td><td></td><td></td><td><a href="#">Create New Account</a></td>
                        </tr>
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                        <tr>
                            <td class="article_indent text-page text-page-indent" colspan="4"><h2>Account Id</h2></td>
                        </tr>                         
                    <%
                        foreach (var loyaltyAccount in Model.LoyaltyAccounts)
                       {
                       %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= loyaltyAccount.Id %></h2></td><td>Points <%= loyaltyAccount.Points%></td><td>Cards <%= loyaltyAccount.LoyaltyCards.Count%></td><td><%= Html.ActionLink("View Details", "Details/" + loyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
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
