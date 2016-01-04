<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserattributenotifierForm.ascx.cs" Inherits="OrderTracking.Gps.Data.UserattributenotifierForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editUserattributenotifierregion fields">
				<%# GetMessage("HeaderLabel")%><asp:TextBox ID="HeaderText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="HeaderErrors" runat="server" />
				<%# GetMessage("TypeLabel")%><asp:TextBox ID="TypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TypeErrors" runat="server" />
				<%# GetMessage("AttributekeyLabel")%><asp:TextBox ID="AttributekeyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AttributekeyErrors" runat="server" />
				<%# GetMessage("AttributevaluestartLabel")%><asp:TextBox ID="AttributevaluestartText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AttributevaluestartErrors" runat="server" />
				<%# GetMessage("AttributevalueendLabel")%><asp:TextBox ID="AttributevalueendText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AttributevalueendErrors" runat="server" />
				<%# GetMessage("ApplicationidLabel")%><asp:TextBox ID="ApplicationidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ApplicationidErrors" runat="server" />
		</div>	
	</div>






