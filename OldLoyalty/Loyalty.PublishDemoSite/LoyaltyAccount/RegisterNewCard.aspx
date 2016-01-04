<%@ page title="Register a new card" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="RegisterNewCard, App_Web_l2ek-tnr" %>

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
    <div id="siteFeatures">
        <dl>
            <dt>Register a new card</dt>
            <dd>Loyalty Card Number</dd>
            <dd><asp:TextBox ID="tbLoyaltyCard" runat="server"></asp:TextBox></dd>
            <dd><asp:Button ID="btnRegisterCard" runat="server"  Text="Register" onclick="btnRegisterCard_Click" /></dd>
            <dd><strong><asp:Literal ID="ltFinished" runat="server" Visible=false></asp:Literal></strong></dd>
            <dd>Register other loyalty cards so they can all collect on the same account</dd>
            <dd>If the loyalty card already has points, they will be transferred to your account</dd>
        </dl>
    </div>
</div>            
</asp:Content>

