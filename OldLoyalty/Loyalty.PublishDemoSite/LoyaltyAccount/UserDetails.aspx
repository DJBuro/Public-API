<%@ page title="Account Details" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="UserDetails, App_Web_l2ek-tnr" %>

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
                        <td>Title</td><td><%=loyaltyAccount.LoyaltyUser.UserTitle.Title%></td>
                    </tr>
                    <tr>
                        <td>First Name</td><td><%=loyaltyAccount.LoyaltyUser.FirstName%></td>
                    </tr>
                    <tr>
                        <td>Middle Initial</td><td><%=loyaltyAccount.LoyaltyUser.MiddleInitial%></td>
                    </tr>
                    <tr>
                        <td>Surname</td><td><%=loyaltyAccount.LoyaltyUser.SurName%></td>
                    </tr>
                    <tr>
                        <td>Email Address</td><td><%=loyaltyAccount.LoyaltyUser.EmailAddress%></td>
                    </tr>
                    <tr>
                        <td>Password</td><td><%=loyaltyAccount.LoyaltyUser.Password%></td>
                    </tr>
                    <tr>
                        <td></td><td><asp:Button ID="btnEditUserDetails" runat="server" Text="Edit Your Details" 
                    onclick="btnEditUserDetails_Click" /></td>
                    </tr>
                </table>
            </dd>
        </dl>
        <dl>
            <dt>Account Address</dt>
            <dd>
                <table>
                    <tr>
                        <td>First Line</td><td><%=loyaltyAccount.AccountAddress.AddressLineOne%></td>
                    </tr>
                     <tr>
                        <td>Second Line</td><td><%=loyaltyAccount.AccountAddress.AddressLineTwo%></td>
                    </tr>
                     <tr>
                        <td>Third Line</td><td><%=loyaltyAccount.AccountAddress.AddressLineThree%></td>
                    </tr>
                     <tr>
                        <td>Forth Line</td><td><%=loyaltyAccount.AccountAddress.AddressLineFour%></td>
                    </tr>
                     <tr>
                        <td>Town/City</td><td><%=loyaltyAccount.AccountAddress.TownCity%></td>
                    </tr>
                     <tr>
                        <td>PostCode</td><td><%=loyaltyAccount.AccountAddress.PostCode%></td>
                    </tr>
                     <tr>
                        <td>County/Province</td><td><%=loyaltyAccount.AccountAddress.CountyProvince%></td>
                    </tr>
                     <tr>
                        <td>Country</td><td><%=loyaltyAccount.AccountAddress.Country.Name%></td>
                    </tr>
                     <tr>
                        <td></td><td><asp:Button ID="btnEditAccountDetails" runat="server" Text="Edit Your Address" 
                    onclick="btnEditAccountDetails_Click" /></td>
                    </tr>
                </table>
            </dd>
        </dl>
    </div>
</div>
</asp:Content>

