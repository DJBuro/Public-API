<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountAddressForm.ascx.cs" Inherits="Loyalty.Data.AccountAddressForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editAccountAddressregion fields">
				<%# GetMessage("AddressLineOneLabel")%><asp:TextBox ID="AddressLineOneText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressLineOneErrors" runat="server" />
				<%# GetMessage("AddressLineTwoLabel")%><asp:TextBox ID="AddressLineTwoText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressLineTwoErrors" runat="server" />
				<%# GetMessage("AddressLineThreeLabel")%><asp:TextBox ID="AddressLineThreeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressLineThreeErrors" runat="server" />
				<%# GetMessage("AddressLineFourLabel")%><asp:TextBox ID="AddressLineFourText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressLineFourErrors" runat="server" />
				<%# GetMessage("TownCityLabel")%><asp:TextBox ID="TownCityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TownCityErrors" runat="server" />
				<%# GetMessage("CountyProvinceLabel")%><asp:TextBox ID="CountyProvinceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CountyProvinceErrors" runat="server" />
				<%# GetMessage("PostCodeLabel")%><asp:TextBox ID="PostCodeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PostCodeErrors" runat="server" />
		</div>	
	</div>






