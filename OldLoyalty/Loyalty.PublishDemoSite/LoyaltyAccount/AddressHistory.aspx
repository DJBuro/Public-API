<%@ page title="Address History" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="AddressHistory, App_Web_l2ek-tnr" %>
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
            <dt>Card Address History for <%= LoyaltyCardNumber%></dt>
            <%
                if (this.address.Count == 0)
                {
                    %>
                    <dd>There are no addresses associated with this card</dd>
                    <%
               } 
                foreach (RamesesAddress ra in this.address)
               { %>
                  <dd>
                  <div class="ramAddress" id="<%= ra.Id%>">    
                  <dl>        
                  <dt>ramAddressID: <%= ra.AddressID %></dt>
                  
                  <dd>
                    <table>
                        <tr>
                            <td>Prem1:</td><td><%= ra.Prem1 %></td>
                        </tr>
                        <tr>
                            <td>Prem2:</td><td><%= ra.Prem2 %></td>
                        </tr>
                        <tr>
                            <td>Prem3:</td><td><%= ra.Prem3 %></td>
                        </tr>
                        <tr>
                            <td>Road Num:</td><td><%= ra.RoadNum %></td>
                        </tr>
                        <tr>
                            <td>Road Name:</td><td><%= ra.RoadName %></td>
                        </tr>
                        <tr>
                            <td>Town:</td><td><%= ra.Town%></td>
                        </tr>
                        <tr>
                            <td>County:</td><td><%= ra.County%></td>
                        </tr>
                        <tr>
                            <td>PostCode:</td><td><%= ra.PostCode %></td>
                        </tr>

                    </table>
                  
                  </dd>

                <dd class="removeAddress" id="<%= ra.Id%>"><input type=button value="Remove this address" /></dd>
                </dl>    
                </div>
                </dd>

            <%} %>
             
        </dl>
    </div>
</div>
</asp:Content>

