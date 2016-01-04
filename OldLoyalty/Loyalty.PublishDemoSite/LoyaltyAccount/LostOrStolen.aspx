<%@ page title="" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="LoyaltyAccount_LostOrStolen, App_Web_l2ek-tnr" %>

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
            <dt>Report <%=LoyaltyCardNumber%> as lost or stolen.</dt>
            <dd>Are you sure you want to report this loyalty card?</dd>
            <dd><strong><asp:Literal ID="LtResponse" runat="server" Visible=false></asp:Literal></strong></dd>
            <dd>
                <asp:Button ID="btnLostCard" runat="server" 
                    onclick="btnLost_Click" Text="Report this card lost" />
            </dd>
            <dd>
                <asp:Button ID="btnStolenCard" runat="server" 
                    onclick="btnStolen_Click" Text="Report this card stolen" />
            </dd>
            <dd>note: Reported loyalty cards do not remove points on your account</dd>
            <dd>note: Reported loyalty cards will be disabled and it will no longer be usable</dd>
            
        </dl>
    </div>
</div>

</asp:Content>

