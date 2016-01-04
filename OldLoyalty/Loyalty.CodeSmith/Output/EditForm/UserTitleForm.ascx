<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserTitleForm.ascx.cs" Inherits="Loyalty.Data.UserTitleForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editUserTitleregion fields">
				<%# GetMessage("TitleLabel")%><asp:TextBox ID="TitleText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TitleErrors" runat="server" />
		</div>	
	</div>






