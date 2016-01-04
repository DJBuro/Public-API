<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GroupForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GroupForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGroupregion fields">
				<%# GetMessage("GroupnameLabel")%><asp:TextBox ID="GroupnameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GroupnameErrors" runat="server" />
				<%# GetMessage("GroupdescriptionLabel")%><asp:TextBox ID="GroupdescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GroupdescriptionErrors" runat="server" />
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("PublicflagLabel")%><asp:TextBox ID="PublicflagText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PublicflagErrors" runat="server" />
		</div>	
	</div>






