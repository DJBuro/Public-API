<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TrackpostprocessorlogForm.ascx.cs" Inherits="OrderTracking.Gps.Data.TrackpostprocessorlogForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editTrackpostprocessorlogregion fields">
				<%# GetMessage("TimestampstartedLabel")%><asp:TextBox ID="TimestampstartedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampstartedErrors" runat="server" />
				<%# GetMessage("TimestampstartedmsLabel")%><asp:TextBox ID="TimestampstartedmsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampstartedmsErrors" runat="server" />
				<%# GetMessage("TimestampdoneLabel")%><asp:TextBox ID="TimestampdoneText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampdoneErrors" runat="server" />
				<%# GetMessage("TimestampdonemsLabel")%><asp:TextBox ID="TimestampdonemsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampdonemsErrors" runat="server" />
				<%# GetMessage("TrackinfomaxtimeLabel")%><asp:TextBox ID="TrackinfomaxtimeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TrackinfomaxtimeErrors" runat="server" />
				<%# GetMessage("TrackinfomaxtimemsLabel")%><asp:TextBox ID="TrackinfomaxtimemsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TrackinfomaxtimemsErrors" runat="server" />
				<%# GetMessage("DirtycountLabel")%><asp:TextBox ID="DirtycountText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DirtycountErrors" runat="server" />
		</div>	
	</div>






