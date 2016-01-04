<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GateeventargumentgenericForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GateeventargumentgenericForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGateeventargumentgenericregion fields">
				<%# GetMessage("IntvalueLabel")%><asp:TextBox ID="IntvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IntvalueErrors" runat="server" />
				<%# GetMessage("DblvalueLabel")%><asp:TextBox ID="DblvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DblvalueErrors" runat="server" />
				<%# GetMessage("BoolvalueLabel")%><asp:TextBox ID="BoolvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BoolvalueErrors" runat="server" />
				<%# GetMessage("StrvalueLabel")%><asp:TextBox ID="StrvalueText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StrvalueErrors" runat="server" />
		</div>	
	</div>






