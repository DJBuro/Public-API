<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupreferrerlogForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GroupreferrerlogForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGroupreferrerlogregion fields">
				<%# GetMessage("RefurlLabel")%><asp:TextBox ID="RefurlText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="RefurlErrors" runat="server" />
				<%# GetMessage("HitsLabel")%><asp:TextBox ID="HitsText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="HitsErrors" runat="server" />
				<%# GetMessage("TimestampLabel")%><asp:TextBox ID="TimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TimestampErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






