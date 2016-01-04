<%@ page title="Edit User Details" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="EditUserDetails, App_Web_l2ek-tnr" %>

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
            <dt>Your Details</dt>
            <dd>
                <table>
                    <tr>
                        <td>Title</td><td><asp:DropDownList ID="ddlTitle" runat="server">
                </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>First name</td><td><asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Middle Initial</td><td><asp:TextBox ID="tbMiddleInitial" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Surname</td><td><asp:TextBox ID="tbSurname" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Email Address</td><td><asp:TextBox ID="tbEmailAddress" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Password</td><td><asp:TextBox ID="tbPassword" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td><td><asp:Button ID="btnUpdate" runat="server" Text="Update" 
                    onclick="btnUpdate_Click"/></td>
                    </tr>
                </table>
            </dd>
        </dl>
    </div>
</div>
</asp:Content>

