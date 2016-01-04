<%@ Page Title="Loyalty Card Transactions" Language="C#" MasterPageFile="~/Shared/DemoSite.master" AutoEventWireup="true" CodeFile="LoyaltyCardTransactions.aspx.cs" Inherits="LoyaltyCardTransactions" %>
<%@ Import Namespace="LoyaltyWS"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" Runat="Server">
<div id="siteLeftBox">
    <dl>
        <dt>Account Details</dt>
        <dd>Current Points: <%=this.loyaltyAccount.Points %></dd>
        <dd><a href="#">Redemption Value: £<%= this.pointsValue%></a></dd>
    </dl>
    <dl>
        <dt>Account Maintenance</dt>
        <dd><a href="MyAccount.aspx">Loyalty Cards</a></dd>
        <dd><a href="RegisterNewCard.aspx">Register new card</a></dd>
        <dd><a href="UserDetails.aspx">User Account Details</a></dd>
    </dl>
</div>

<div id="siteContent">
     <ul id="cardNav">
        <li><a href="LoyaltyCardTransactions.aspx">Transaction History</a></li>
        <li><a href="AddressHistory.aspx">Address History</a></li>
        <li><a href="LostOrStolen.aspx">Lost / Stolen</a></li>
        <li><a href="RemoveCard.aspx">Remove Card</a></li>
        <li><a href="Pin.aspx">Pin</a></li>
    </ul>
    <div id="siteFeatures">
        <dl>
            <dt>Transaction History for <%=loyaltyCardNumber%></dt>
            
            <%if (transactionHistory == null | transactionHistory.Count == 0)
              {%>
                <dd>This card has not been used.</dd>
            <%}
              else
              {%>
           <%
                foreach (serviceTransactionHistory history in transactionHistory)
                { 
            %> <dd>
                <div id="transactionHistory">
                    <dl>
                        <dt>Order Number <%= history.OrderId%></dt>
                        <dd>
                            <table>
                                <tr>
                                    <td>Order Type</td><td><%=lib.GetOrderTypeById(history.OrderTypeId.Value).Name%></td>
                                </tr>
                                <%if (history.LoyaltyPointsRedeemed > 0)
                                  {%>
                                <tr>
                                    <td>Points Redeemed</td><td><%= history.LoyaltyPointsRedeemed%></td>
                                </tr>
                                <%} %>
                            </table>
                        </dd>
                        <dd id="<%=history.Id.Value %>">
                        <table>
                            <tr>
                                <td align="center"><strong>Items</strong></td><td><strong>Price</strong></td><td><strong>Points Gained</strong></td>
                            </tr>
                           
                        
                        
                        <%foreach (serviceOrderItemHistory item in history.OrderItemHistory)
                          {
                        %>
                         <tr>
                                <td><%=item.Name%></td><td align="right"><%=string.Format("{0:C}",item.ItemPrice)%></td><td align="right"><%=item.ItemLoyaltyPoints%></td>
                        </tr>
                        
                         <%
                        } //end item
                            %>
                            <tr>
                                <td><strong>Order Total</strong></td><td align="right"><strong><%=string.Format("{0:C}", history.OrderTotal)%></strong></td><td></td>
                            </tr>
                            <tr>
                                <td><strong>Total Points Added</strong></td><td></td><td align="right"><strong><%= history.LoyaltyPointsAdded%></strong></td>
                            </tr>
                              <%if (history.LoyaltyPointsRedeemed > 0)
                                {%>
                            <tr>
                                <td><strong>Loyalty Discount</strong></td><td align="right"><strong>-<%= string.Format("{0:C}", history.LoyaltyPointsRedeemed / history.LoyaltyPointsValue)%></strong></td><td align="right"><strong>-<%= history.LoyaltyPointsRedeemed%></strong></td>
                            </tr>
                            <tr>
                                <td><strong>You Paid</strong></td><td align="right"><strong><%= string.Format("{0:C}", history.OrderTotal - history.LoyaltyPointsRedeemed / history.LoyaltyPointsValue)%></strong></td><td></td>
                            </tr>
                            <%} %>
                            </table>
                        </dd>
                    </dl>
                </div>               
                </dd>
            <%
                }//end history
              }
            %></dl>
    </div>
</div>
</asp:Content>

