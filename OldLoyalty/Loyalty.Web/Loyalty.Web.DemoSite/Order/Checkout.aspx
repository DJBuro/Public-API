<%@ Page Title="Checkout" Language="C#" MasterPageFile="~/Shared/DemoSite.master" AutoEventWireup="true" CodeFile="Checkout.aspx.cs" Inherits="Checkout" %>

<%@ Register src="../Shared/uc/Upsells.ascx" tagname="Upsells" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" Runat="Server">
    <div id="upsells">
        <uc1:Upsells ID="Upsells1" runat="server" />
    
</div>
    

    
<div id="pageContent">
    <div id="storeFinder">
        <dl>
            <dt>Store finder</dt>
            <dd>Just type in your postcode</dd>
            <dd><input id="Text1" type="text" /></dd>
            <dd><a href="#">search</a></dd>
        </dl>
    </div>
 
    <div id="features">
        <dl>
            <dt>Checkout</dt>
            <dd>You have ordered a few things</dd>
            <dd>
            <table>
                <tr>
                    <td></td><td><strong>Your Order</strong></td>
                </tr>
                <tr>
                    <td>Order Reference</td><td><asp:TextBox ID="tbOrderReference" runat="server">123</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Order Type</td><td><asp:DropDownList ID="ddlOrderType" runat="server">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td></td><td><h4>First Example Item</h4></td>
                </tr>
                <tr>
                    <td>First Item Name</td><td><asp:TextBox ID="tbItem1Name" runat="server">Cheesy Pizza</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Loyalty Points Value</td><td><asp:TextBox ID="tbItem1Points" runat="server">600</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Total Value</td><td><asp:TextBox ID="tbItem1Value" runat="server">14.99</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td><td><h4>Second Example Item</h4></td>
                </tr>
                <tr>
                    <td>Second Item Name</td><td><asp:TextBox ID="tbItem2Name" runat="server">Garlic bread</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Loyalty Points Value</td><td><asp:TextBox ID="tbItem2Points" runat="server">100</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Total Value</td><td><asp:TextBox ID="tbItem2Value" runat="server">2.99</asp:TextBox></td>
                </tr>
                <tr>
                    <td></td><td><h4>Delivery Address</h4></td>
                </tr>
                <tr>
                    <td>Address Line 1</td><td><asp:TextBox ID="tbAddressLine1" runat="server">First Line</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Address Line 2</td><td><asp:TextBox ID="tbAddressLine2" runat="server">Second Line</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Address Line 3</td><td><asp:TextBox ID="tbAddressLine3" runat="server">Third Line</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Town</td><td><asp:TextBox ID="tbTown" runat="server">Horsham</asp:TextBox></td>
                </tr>
                <tr>
                    <td>County</td><td><asp:TextBox ID="tbProvince" runat="server">West Sussex</asp:TextBox></td>
                </tr>
                <tr>
                    <td>PostCode</td><td><asp:TextBox ID="tbPostCode" runat="server">RH1 9TF</asp:TextBox></td>
                </tr>
                <tr>
                    <td>Country</td><td>
                        <asp:DropDownList ID="ddlCountry" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Directions (optional)</strong></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:TextBox ID="tbDirections" runat="server" MaxLength="500" Rows="6" 
                    TextMode="MultiLine" Width="287px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="2"><strong>Your loyalty cards number</strong></td>
                </tr>
                <tr>
                    <td><asp:TextBox ID="tbLoyaltyCard" runat="server"></asp:TextBox></td><td><asp:Button ID="btnGetAvailablePoints" runat="server" Text="Get available points" onclick="btnGetAvailablePoints_Click" /></td>
                </tr>
                <tr>
                    <td colspan="2"><asp:Literal ID="ltPoints" runat="server"></asp:Literal></td>
                </tr>
                <tr >
                    <td runat="server" id="tdPoints" visible=false>apply discount of: £</td><td><asp:TextBox ID="tbDiscount" runat="server" Visible=false></asp:TextBox></td>
                </tr>
            </table>

            <dd><asp:Button ID="btnOrder" runat="server" Text="Place Your Order" onclick="btnOrder_Click" /></dd>
        </dl>
    </div>
    <div id="tracker">
        <dl>
            <dt>Track your order</dt>
            <dd>Just need your order number</dd>
            <dd><input id="Text2" type="text" /></dd>
            <dd><a href="#">find</a></dd>
        </dl>
    </div>
        <div id="gifts">
        <dl>
            <dt>Gift certificates</dt>
            <dd>Know somebody poor</dd>
            <dd><a href="#">find out why</a></dd>
        </dl>
    </div>
</div>

</asp:Content>

