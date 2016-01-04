<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MsgfielddictionaryentryForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MsgfielddictionaryentryForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMsgfielddictionaryentryregion fields">
				<%# GetMessage("MultiplicatorLabel")%><asp:TextBox ID="MultiplicatorText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MultiplicatorErrors" runat="server" />
				<%# GetMessage("ConstantLabel")%><asp:TextBox ID="ConstantText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ConstantErrors" runat="server" />
				<%# GetMessage("EnabledLabel")%><asp:TextBox ID="EnabledText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="EnabledErrors" runat="server" />
				<%# GetMessage("SavewithposLabel")%><asp:TextBox ID="SavewithposText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SavewithposErrors" runat="server" />
				<%# GetMessage("SavechangesonlyLabel")%><asp:TextBox ID="SavechangesonlyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SavechangesonlyErrors" runat="server" />
				<%# GetMessage("MultiplicatorformulaLabel")%><asp:TextBox ID="MultiplicatorformulaText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MultiplicatorformulaErrors" runat="server" />
		</div>	
	</div>






