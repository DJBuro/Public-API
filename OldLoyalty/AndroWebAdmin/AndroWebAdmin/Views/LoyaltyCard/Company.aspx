<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyCardViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Company.Name %> Loyalty Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                   <%=Model.LoyaltyCards.Count %> <%= Model.Company.Name %> Loyalty Cards
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
                                <td><h2>Status</h2></td><td><h2>Card Number</h2></td><td><h2>Transactions</h2></td><td></td>
                            </tr>
 <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                          </tr>
                          <%
                                 foreach (var loyaltyCard in Model.LoyaltyCards)
                                 {
                                     var cardStatus = "";
                                     
                                     foreach (LoyaltyCardStatus loyaltyCardStatus in loyaltyCard.LoyaltyCardStatus)
                                     {
                                         cardStatus = loyaltyCardStatus.Status.Name;
                                     }
                            %>
                            <tr>
                                <td class="article_indent text-page text-page-indent" ><%=cardStatus%></td><td><%=loyaltyCard.CardNumber %></td><td><%=loyaltyCard.TransactionHistory.Count%></td><td><%= Html.ActionLink("View Details", "Details/" + loyaltyCard.Id.Value, "LoyaltyCard")%></td>
                            </tr>
                              <%
                            }//foreach loyaltyCard
                                  %>
                                  
                          </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>

</asp:Content>
