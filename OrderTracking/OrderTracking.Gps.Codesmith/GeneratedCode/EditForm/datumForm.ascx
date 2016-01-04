<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DatumForm.ascx.cs" Inherits="OrderTracking.Gps.Data.DatumForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editDatumregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("SemimajoraxisLabel")%><asp:TextBox ID="SemimajoraxisText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SemimajoraxisErrors" runat="server" />
				<%# GetMessage("E2Label")%><asp:TextBox ID="E2Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="E2Errors" runat="server" />
				<%# GetMessage("DeltaxLabel")%><asp:TextBox ID="DeltaxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeltaxErrors" runat="server" />
				<%# GetMessage("DeltayLabel")%><asp:TextBox ID="DeltayText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeltayErrors" runat="server" />
				<%# GetMessage("DeltazLabel")%><asp:TextBox ID="DeltazText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeltazErrors" runat="server" />
				<%# GetMessage("RotxLabel")%><asp:TextBox ID="RotxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RotxErrors" runat="server" />
				<%# GetMessage("RotyLabel")%><asp:TextBox ID="RotyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RotyErrors" runat="server" />
				<%# GetMessage("RotzLabel")%><asp:TextBox ID="RotzText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RotzErrors" runat="server" />
				<%# GetMessage("ScaleLabel")%><asp:TextBox ID="ScaleText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ScaleErrors" runat="server" />
		</div>	
	</div>






