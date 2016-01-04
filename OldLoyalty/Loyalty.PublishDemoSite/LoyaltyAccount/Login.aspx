<%@ page title="Login" language="C#" masterpagefile="~/Shared/DemoSite.master" autoeventwireup="true" inherits="Login, App_Web_l2ek-tnr" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentBody" Runat="Server">
<div id="upsells">
    <a href="#">
        <dl>
            <dt>Loyalty cards</dt>
            <dd class="upsellImage"><img src="../css/imgs/upsells/loyalty.jpg" /></dd>
            <dd><p>You really need one</p></dd>
        </dl>
    </a>
</div>
<div id="siteContent">
    <div id="siteFeatures">
            <dl>
                <dt>Existing Users</dt>
                <dd>email address</dd>
                <dd><asp:TextBox ID="tbEmailAddress" runat="server"></asp:TextBox></dd>
                <dd>password</dd>
                <dd><asp:TextBox ID="tbPassword" runat="server"></asp:TextBox></dd>
                <dd><asp:Button ID="btnLogin" runat="server" Text="Login"/></dd>
                <dd><a href="ForgottenPassword.aspx">forgotten your password?</a></dd>
            </dl>
            <dl>
                <dt>New Users</dt>
                <dd>Please register</dd>
                <dd>
                    <asp:Button ID="btnRegister" runat="server" Text="Register" 
                        onclick="btnRegister_Click" /></dd>
            </dl>
            <dl>
                <dt>Company Administration</dt>
                <dd>View the company administration area</dd>
                <dd><asp:Button ID="btnAdmin" runat="server" Text="Admin area" 
                        onclick="btnAdmin_Click" /></dd>
            </dl>
    </div>
</div>
</asp:Content>

