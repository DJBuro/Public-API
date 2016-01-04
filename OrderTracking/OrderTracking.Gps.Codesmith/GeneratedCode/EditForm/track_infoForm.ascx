<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackinfoForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackinfoForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackinforegion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("MinlongitudeLabel")%><asp:TextBox ID="MinlongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinlongitudeErrors" runat="server" />
				<%# GetMessage("MaxlongitudeLabel")%><asp:TextBox ID="MaxlongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxlongitudeErrors" runat="server" />
				<%# GetMessage("MinlatitudeLabel")%><asp:TextBox ID="MinlatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinlatitudeErrors" runat="server" />
				<%# GetMessage("MaxlatitudeLabel")%><asp:TextBox ID="MaxlatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxlatitudeErrors" runat="server" />
				<%# GetMessage("MinaltitudeLabel")%><asp:TextBox ID="MinaltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinaltitudeErrors" runat="server" />
				<%# GetMessage("MaxaltitudeLabel")%><asp:TextBox ID="MaxaltitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxaltitudeErrors" runat="server" />
				<%# GetMessage("MintimestampLabel")%><asp:TextBox ID="MintimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MintimestampErrors" runat="server" />
				<%# GetMessage("MinmillisecondsLabel")%><asp:TextBox ID="MinmillisecondsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MinmillisecondsErrors" runat="server" />
				<%# GetMessage("MaxtimestampLabel")%><asp:TextBox ID="MaxtimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxtimestampErrors" runat="server" />
				<%# GetMessage("MaxmillisecondsLabel")%><asp:TextBox ID="MaxmillisecondsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="MaxmillisecondsErrors" runat="server" />
				<%# GetMessage("TotaldistanceLabel")%><asp:TextBox ID="TotaldistanceText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TotaldistanceErrors" runat="server" />
				<%# GetMessage("DeletedLabel")%><asp:TextBox ID="DeletedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeletedErrors" runat="server" />
				<%# GetMessage("DirtycountLabel")%><asp:TextBox ID="DirtycountText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DirtycountErrors" runat="server" />
		</div>	
	</div>






