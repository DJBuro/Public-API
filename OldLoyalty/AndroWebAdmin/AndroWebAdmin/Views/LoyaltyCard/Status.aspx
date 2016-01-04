<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyCardViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Status.Name %> Loyalty Cards 
	</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.Status.Name %> Loyalty Cards 
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
                        <td><h2>Card Number</h2></td><td><h2>Transactions</h2><td><td></td>
                    </tr>
                    <tr >
                        <td colspan="3"><span class="article_separator"></span></td>
                    </tr>
                    <%
                        foreach (LoyaltyCard loyaltyCard in Model.LoyaltyCards)
                       {
                       %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><%= loyaltyCard.CardNumber %></td><td><%= loyaltyCard.TransactionHistory.Count%></td><td><%= Html.ActionLink("View Details", "Details/" + loyaltyCard.Id.Value, "LoyaltyCard")%></td>
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
