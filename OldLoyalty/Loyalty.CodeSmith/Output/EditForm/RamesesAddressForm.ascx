<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RamesesAddressForm.ascx.cs" Inherits="Loyalty.Data.RamesesAddressForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editRamesesAddressregion fields">
				<%# GetMessage("ContactIDLabel")%><asp:TextBox ID="ContactIDText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ContactIDErrors" runat="server" />
				<%# GetMessage("ContactLabel")%><asp:TextBox ID="ContactText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ContactErrors" runat="server" />
				<%# GetMessage("AddressTypeLabel")%><asp:TextBox ID="AddressTypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressTypeErrors" runat="server" />
				<%# GetMessage("OptFlagLabel")%><asp:TextBox ID="OptFlagText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OptFlagErrors" runat="server" />
				<%# GetMessage("AddressIDLabel")%><asp:TextBox ID="AddressIDText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressIDErrors" runat="server" />
				<%# GetMessage("PostOfficeIDLabel")%><asp:TextBox ID="PostOfficeIDText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PostOfficeIDErrors" runat="server" />
				<%# GetMessage("SubAddressLabel")%><asp:TextBox ID="SubAddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SubAddressErrors" runat="server" />
				<%# GetMessage("Org1Label")%><asp:TextBox ID="Org1Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Org1Errors" runat="server" />
				<%# GetMessage("Org2Label")%><asp:TextBox ID="Org2Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Org2Errors" runat="server" />
				<%# GetMessage("Org3Label")%><asp:TextBox ID="Org3Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Org3Errors" runat="server" />
				<%# GetMessage("Prem1Label")%><asp:TextBox ID="Prem1Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Prem1Errors" runat="server" />
				<%# GetMessage("Prem2Label")%><asp:TextBox ID="Prem2Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Prem2Errors" runat="server" />
				<%# GetMessage("Prem3Label")%><asp:TextBox ID="Prem3Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Prem3Errors" runat="server" />
				<%# GetMessage("RoadNumLabel")%><asp:TextBox ID="RoadNumText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RoadNumErrors" runat="server" />
				<%# GetMessage("RoadNameLabel")%><asp:TextBox ID="RoadNameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RoadNameErrors" runat="server" />
				<%# GetMessage("LocalityLabel")%><asp:TextBox ID="LocalityText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LocalityErrors" runat="server" />
				<%# GetMessage("TownLabel")%><asp:TextBox ID="TownText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TownErrors" runat="server" />
				<%# GetMessage("CountyLabel")%><asp:TextBox ID="CountyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CountyErrors" runat="server" />
				<%# GetMessage("PostCodeLabel")%><asp:TextBox ID="PostCodeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PostCodeErrors" runat="server" />
				<%# GetMessage("GridLabel")%><asp:TextBox ID="GridText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GridErrors" runat="server" />
				<%# GetMessage("RefnoLabel")%><asp:TextBox ID="RefnoText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RefnoErrors" runat="server" />
				<%# GetMessage("DirectionsLabel")%><asp:TextBox ID="DirectionsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DirectionsErrors" runat="server" />
				<%# GetMessage("DpsLabel")%><asp:TextBox ID="DpsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DpsErrors" runat="server" />
				<%# GetMessage("PafTypeLabel")%><asp:TextBox ID="PafTypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PafTypeErrors" runat="server" />
				<%# GetMessage("FlagsLabel")%><asp:TextBox ID="FlagsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="FlagsErrors" runat="server" />
				<%# GetMessage("TimesOrderedLabel")%><asp:TextBox ID="TimesOrderedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimesOrderedErrors" runat="server" />
		</div>	
	</div>






