<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoreForm.ascx.cs" Inherits="OrderTracking.Data.StoreForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editStoreregion fields">
				<%# GetMessage("ExternalStoreIdLabel")%><asp:TextBox ID="ExternalStoreIdText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExternalStoreIdErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DeliveryRadiusLabel")%><asp:TextBox ID="DeliveryRadiusText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeliveryRadiusErrors" runat="server" />
		</div>	
	</div>






