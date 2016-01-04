<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmailnotifierForm.ascx.cs" Inherits="OrderTracking.Gps.Data.EmailnotifierForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editEmailnotifierregion fields">
				<%# GetMessage("SubjectLabel")%><asp:TextBox ID="SubjectText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SubjectErrors" runat="server" />
				<%# GetMessage("IshtmlLabel")%><asp:TextBox ID="IshtmlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="IshtmlErrors" runat="server" />
		</div>	
	</div>






