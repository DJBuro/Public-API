<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TblCountryForm.ascx.cs" Inherits="Loyalty.Data.TblCountryForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTblCountryregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("ISOCodeLabel")%><asp:TextBox ID="ISOCodeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ISOCodeErrors" runat="server" />
		</div>	
	</div>






