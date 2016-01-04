<%@ Page Title="Welcome" Language="C#"  MasterPageFile="~/Shared/DemoSite.master"  AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<%@ Register src="Shared/uc/Upsells.ascx" tagname="Upsells" tagprefix="uc1" %>
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
            <dt>Starters</dt>
            <dd class="featuresImage"><img src="css/imgs/default/starters.jpg"/></dd>
            <dd>Why settle for only one? Cheesy Jalapeno Bites, BBQ Wings, Chicken Dippers, the list is almost endless</dd>
            <dd><a href="#">more</a></dd>
        </dl>
        <dl>
            <dt>Desserts</dt>
            <dd class="featuresImage"><img src="css/imgs/default/dessert.jpg"/></dd>
            <dd>Do you have the strength to resist our fresh desserts? We have a wide range of freshly made desserts including pastries, cakes and fresh fruit. Our cakes include mixed fruit cheesecake, caramel cheesecake, raspberry cheesecake, lemon cake, carrot cake, ginger slice, orange slice, apple & almond cake and white chocolate cake.</dd>
            <dd><a href="#">more</a></dd>
        </dl>
        <dl>
            <dt>Our secret is...</dt>
            <dd class="featuresImage"><img src="css/imgs/default/pizzaToss.jpg"/></dd>
            <dd>Tradition...We toss our dough in the tradition of our founder. The trick is to get the dough right to start with - then you can toss away. Without proper gluten development, the dough will not be stretchy enough and will tear.</dd>
            <dd><a href="#">more</a></dd>
        </dl>
    </div>

    <div id="tracker">
        <dl>
            <dt>Track your order</dt>
            <dd>Just need your order number</dd>
            <dd><input id="Text2" type="text" /></dd>
            <dd><a href="Location/Default.aspx">find</a></dd>
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
