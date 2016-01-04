<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DevicedefForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DevicedefForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editDevicedefregion fields">
				<%# GetMessage("DevicenameLabel")%><asp:TextBox ID="DevicenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DevicenameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("TemplatemsgfielddictidLabel")%><asp:TextBox ID="TemplatemsgfielddictidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TemplatemsgfielddictidErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("UpgradableLabel")%><asp:TextBox ID="UpgradableText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="UpgradableErrors" runat="server" />
		</div>	
	</div>






