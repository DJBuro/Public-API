<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ApptemplateForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ApptemplateForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editApptemplateregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
		</div>	
	</div>






