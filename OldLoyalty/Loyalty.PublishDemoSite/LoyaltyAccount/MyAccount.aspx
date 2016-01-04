<%@ page title="Loyalty Account" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="MyAccount, App_Web_l2ek-tnr" %>
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
    <div id="siteFeatures">
        <dl>
            <dt>Welcome <%= this.loyaltyAccount.LoyaltyUser.UserTitle.Title %>&nbsp;<%= this.loyaltyAccount.LoyaltyUser.SurName%> </dt>

            <dd>Your loyalty cards:</dd>   
            <% if (this.loyaltyAccount.LoyaltyCards != null)
               {%>
            <asp:Panel runat="server" ID="PnlButtons"></asp:Panel>
            </dd>
            <%}
               else
               {%>
               <dd>Sorry, you don't have any registed cards</dd>
            <%} %>
        </dl>
    </div>

</div>
    
</asp:Content>

