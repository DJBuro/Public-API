<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeocodercacheForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GeocodercacheForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGeocodercacheregion fields">
				<%# GetMessage("LonLabel")%><asp:TextBox ID="LonText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LonErrors" runat="server" />
				<%# GetMessage("LatLabel")%><asp:TextBox ID="LatText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatErrors" runat="server" />
				<%# GetMessage("CountrynameLabel")%><asp:TextBox ID="CountrynameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CountrynameErrors" runat="server" />
				<%# GetMessage("CitynameLabel")%><asp:TextBox ID="CitynameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CitynameErrors" runat="server" />
				<%# GetMessage("PostalcodenumberLabel")%><asp:TextBox ID="PostalcodenumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PostalcodenumberErrors" runat="server" />
				<%# GetMessage("StreetnameLabel")%><asp:TextBox ID="StreetnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StreetnameErrors" runat="server" />
				<%# GetMessage("StreetnumberLabel")%><asp:TextBox ID="StreetnumberText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StreetnumberErrors" runat="server" />
				<%# GetMessage("StreetboxLabel")%><asp:TextBox ID="StreetboxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StreetboxErrors" runat="server" />
				<%# GetMessage("AddressLabel")%><asp:TextBox ID="AddressText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AddressErrors" runat="server" />
				<%# GetMessage("LonlathashLabel")%><asp:TextBox ID="LonlathashText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LonlathashErrors" runat="server" />
		</div>	
	</div>






