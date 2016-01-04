<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CountryForm.ascx.cs" Inherits="OrderTracking.Data.CountryForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editCountryregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
		</div>	
	</div>






