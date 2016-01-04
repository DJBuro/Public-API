<%@ Page Title="Register" Language="C#" MasterPageFile="~/Shared/DemoSite.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" Runat="Server">
<div id="siteLeftBox">
    <dl>
        <dt>Register</dt>
        <dd>Current Points: </dd>
        <dd><a href="#">Redemption Value: £</a></dd>
    </dl>
</div>
<div id="siteContent">
    <div id="siteFeatures">
        <dl>
            <dt>Register</dt>
            <dd>
            <table>
                <tr>
                    <td></td><td><strong>Your Details</strong></td>
                </tr>
                <tr>
                    <td>Title</td><td><asp:DropDownList ID="ddlTitle" runat="server">
                </asp:DropDownList></td>
                </tr>
                <tr>
                    <td>First name</td><td><asp:TextBox ID="tbFirstName" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Middle initial</td><td><asp:TextBox ID="tbMiddleInitial" runat="server" MaxLength="1"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Surname</td><td><asp:TextBox ID="tbSurname" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Email address</td><td><asp:TextBox ID="tbEmailAddress" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Password</td><td><asp:TextBox ID="tbPassword" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td><td><strong>Your Loyalty Card</strong></td>
                </tr>
                <tr>
                    <td>Loyalty card number</td><td><asp:TextBox ID="txtLCNumber" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td></td><td><strong>Your Address</strong></td>
                </tr>
                <tr>
                    <td>First line of your address</td><td><asp:TextBox ID="txtAddressLn1" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Second line of your address</td><td><asp:TextBox ID="txtAddressLn2" runat="server" Wrap="False"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Third line of your address</td><td><asp:TextBox ID="txtAddressLn3" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Forth line of your address</td><td><asp:TextBox ID="txtAddressLn4" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Town or City</td><td><asp:TextBox ID="txtTownCity" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>PostCode or ZipCode</td><td><asp:TextBox ID="txtPostCode" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>County, Province or State</td><td><asp:TextBox ID="txtCountyProvince" runat="server"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>Country</td><td><asp:DropDownList ID="ddlCountry" runat="server"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td></td><td><asp:Button ID="btnStep1" runat="server" onclick="btnStep1_Click" 
                    Text="Register" /></td>
                </tr>
                <tr>
                    <td colspan="2"><strong><asp:Literal ID="ltError" runat="server"></asp:Literal></strong></td>
                </tr>
            </table>
            </dd>
        </dl>
    </div>

</div>
</asp:Content>

