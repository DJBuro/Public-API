<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplatecmdstepForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TemplatecmdstepForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTemplatecmdstepregion fields">
				<%# GetMessage("TemplateLabel")%><asp:TextBox ID="TemplateText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TemplateErrors" runat="server" />
				<%# GetMessage("TransportLabel")%><asp:TextBox ID="TransportText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TransportErrors" runat="server" />
				<%# GetMessage("StepdescriptionLabel")%><asp:TextBox ID="StepdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StepdescriptionErrors" runat="server" />
		</div>	
	</div>






