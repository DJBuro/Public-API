<%@ Page Title="Remove Card From Account" Language="C#" MasterPageFile="~/Shared/DemoSite.master" AutoEventWireup="true" CodeFile="RemoveCard.aspx.cs" Inherits="RemoveCard" %>

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
            <dt>Remove <%=LoyaltyCardNumber%> from your loyalty account?</dt>
            <dd>Are you sure you want to remove this card from your account?</dd>
            <dd>
                <asp:Button ID="imgRemoveCard" runat="server" 
                    onclick="imgRemoveCard_Click" Text="Remove this card" />

            </dd>
            <dd>note: Removing this card from your account does not remove points on your account</dd>
            <dd>note: Removing this card will also disable the card and it will no longer be usable</dd>
        </dl>
    </div>
</div>
</asp:Content>

