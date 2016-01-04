<%@ page title="" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="LoyaltyAccount_Pin, App_Web_l2ek-tnr" %>

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
                <dt>Add or View the Loyalty Card Pin</dt>
                <dd>The current Pin for this Card is:</dd>
                <dd><strong><%=Pin %></strong></dd>
                <dd><asp:TextBox ID="tbPin" runat="server"></asp:TextBox></dd>
                <dd><asp:Button ID="btnPin" runat="server" Text="Update" onclick="btnPin_Click"/></dd>
                <dd>If you add a Pin to this Loyalty Card You will need to quote this Pin each time you wish to redeem points</dd> 
                <dd>If you wish to remove a Pin, leave the text box empty, and click the 'Update' </dd>
            </dl>
    </div>
</div>


</asp:Content>

