<%@ page title="" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="EditAccountDetails, App_Web_l2ek-tnr" %>

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
            <dt>Account Address</dt>
            <dd>
                <table>
                    <tr>
                        <td>First Line</td><td><asp:TextBox ID="txtAddressLn1" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Second Line</td><td><asp:TextBox ID="txtAddressLn2" runat="server" Wrap="False"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Third Line</td><td><asp:TextBox ID="txtAddressLn3" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Forth Line</td><td><asp:TextBox ID="txtAddressLn4" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Town/City</td><td><asp:TextBox ID="txtTownCity" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>PostCode</td><td><asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>County/Province</td><td><asp:TextBox ID="txtCountyProvince" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>Country</td><td><asp:DropDownList ID="ddlCountry" runat="server">
                </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td></td><td><dd><asp:Button ID="btnUpdate" runat="server" Text="Update" 
                    onclick="btnUpdate_Click"/></td>
                    </tr>
                    
                </table>
            </dd>
        </dl>
    </div>
</div>


</asp:Content>

