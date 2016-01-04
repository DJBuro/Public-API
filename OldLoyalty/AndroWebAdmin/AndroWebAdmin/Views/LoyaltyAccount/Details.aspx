<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyAccountViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Loyalty Account <%= Model.LoyaltyAccount.Id %> Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.LoyaltyAccount.Site.Company.Name%> Account #<%= Model.LoyaltyAccount.Id %>
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
                    <%
                        LoyaltyAccountStatus accountStatus = (LoyaltyAccountStatus) Model.LoyaltyAccount.AccountStatus[0];
                        %>
                        <tr>
                            <td class="article_indent text-page text-page-indent" colspan="4"><h2>Account</h2></td>
                        </tr>                         
                        <tr>
                            <td class="article_indent text-page text-page-indent">Account Status: <%=accountStatus.AccountStatus.Name%></td><td>Points: <%= Model.LoyaltyAccount.Points%></td><td>Cards: <%= Model.LoyaltyAccount.LoyaltyCards.Count%></td><td><%= Html.ActionLink("Suspend Account", "Details/" + Model.LoyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
                        </tr>  
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                          </tr>
                          <% if (Model.LoyaltyAccount.LoyaltyUser.Count > 0)
                             { %>
                          <tr>
                            <td class="article_indent text-page text-page-indent" colspan="4"><h2>User Details</h2></td>
                        </tr> 
                        <tr>
                            <td class="article_indent text-page text-page-indent" ></td><td>User Id</td><td>Name</td><td></td>
                        </tr>
                        
                        <%
                              foreach (LoyaltyUser loyaltyUser in Model.LoyaltyAccount.LoyaltyUser)
                              {
                                %>
                            <tr>
                                <td class="article_indent text-page text-page-indent" ></td><td><%= loyaltyUser.Id%></td><td><%= loyaltyUser.UserTitle.Title%> <%= loyaltyUser.FirstName%> <%= loyaltyUser.MiddleInitial%> <%= loyaltyUser.SurName%></td><td><%= Html.ActionLink("Edit User Details", "Details/" + Model.LoyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
                            </tr>
                                <%
                              } //foreach loyaltyUser%>
                            <tr >
                                <td colspan="4"><span class="article_separator"></span></td>
                            </tr>
                          <%}//loyalty user count %>
                          
                          <% if (Model.LoyaltyAccount.AccountAddress.Count > 0)
                            {%>
                          <tr>
                            <td class="article_indent text-page text-page-indent" colspan="4"><h2>Address Details</h2></td>
                        </tr> 
                        
                        <%
                             foreach (AccountAddress accountAddress in Model.LoyaltyAccount.AccountAddress)
                             {
                        %>

                            <tr>
                                <td></td><td></td><td></td><td><%= Html.ActionLink("Edit Address Details", "Details/" + Model.LoyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.AddressLineOne%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.AddressLineTwo%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.AddressLineThree%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.AddressLineFour%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.TownCity%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.CountyProvince%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td><%=accountAddress.PostCode%></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent" ></td><td><%=accountAddress.Country.Name%></td><td></td><td></td>
                            </tr>
                            <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                          </tr>
                            <%
                                 } //foreach address
                            %>
                                 <%
                            } //address count
                            %>
                            <tr>
                                <td><h2>Loyalty Cards</h2></td><td></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td></td><td>Card Number</td><td>Transactions</td><td></td>
                            </tr>
                          <%
                                 foreach (LoyaltyCard loyaltyCard in Model.LoyaltyAccount.LoyaltyCards)
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
