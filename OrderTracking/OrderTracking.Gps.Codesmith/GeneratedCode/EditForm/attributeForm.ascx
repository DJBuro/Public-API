<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AttributeForm.ascx.cs" Inherits="OrderTracking.Gps.Data.AttributeForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editAttributeregion fields">
				<%# GetMessage("AttributenameLabel")%><asp:TextBox ID="AttributenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AttributenameErrors" runat="server" />
				<%# GetMessage("AttributetypeLabel")%><asp:TextBox ID="AttributetypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AttributetypeErrors" runat="server" />
				<%# GetMessage("IntvalueLabel")%><asp:TextBox ID="IntvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IntvalueErrors" runat="server" />
				<%# GetMessage("DoublevalueLabel")%><asp:TextBox ID="DoublevalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DoublevalueErrors" runat="server" />
				<%# GetMessage("StringvalueLabel")%><asp:TextBox ID="StringvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StringvalueErrors" runat="server" />
				<%# GetMessage("BoolvalueLabel")%><asp:TextBox ID="BoolvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BoolvalueErrors" runat="server" />
		</div>	
	</div>






