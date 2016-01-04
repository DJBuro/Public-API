<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GridForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGridregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("AlgorithmLabel")%><asp:TextBox ID="AlgorithmText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AlgorithmErrors" runat="server" />
				<%# GetMessage("FalseeastingLabel")%><asp:TextBox ID="FalseeastingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="FalseeastingErrors" runat="server" />
				<%# GetMessage("FalsenorthingLabel")%><asp:TextBox ID="FalsenorthingText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="FalsenorthingErrors" runat="server" />
				<%# GetMessage("OrigolongitudeLabel")%><asp:TextBox ID="OrigolongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OrigolongitudeErrors" runat="server" />
				<%# GetMessage("OrigolatitudeLabel")%><asp:TextBox ID="OrigolatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="OrigolatitudeErrors" runat="server" />
				<%# GetMessage("ScaleLabel")%><asp:TextBox ID="ScaleText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ScaleErrors" runat="server" />
				<%# GetMessage("Latitudesp1Label")%><asp:TextBox ID="Latitudesp1Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Latitudesp1Errors" runat="server" />
				<%# GetMessage("Latitudesp2Label")%><asp:TextBox ID="Latitudesp2Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Latitudesp2Errors" runat="server" />
		</div>	
	</div>






