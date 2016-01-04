<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoadabletypeForm.ascx.cs" Inherits="OrderTracking.Gps.Data.LoadabletypeForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editLoadabletyperegion fields">
				<%# GetMessage("AssemblynameLabel")%><asp:TextBox ID="AssemblynameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="AssemblynameErrors" runat="server" />
				<%# GetMessage("TypenameLabel")%><asp:TextBox ID="TypenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TypenameErrors" runat="server" />
				<%# GetMessage("TypedescriptionLabel")%><asp:TextBox ID="TypedescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TypedescriptionErrors" runat="server" />
				<%# GetMessage("BasetypenameLabel")%><asp:TextBox ID="BasetypenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BasetypenameErrors" runat="server" />
				<%# GetMessage("BasetypedescriptionLabel")%><asp:TextBox ID="BasetypedescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BasetypedescriptionErrors" runat="server" />
				<%# GetMessage("DeletedLabel")%><asp:TextBox ID="DeletedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeletedErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("VersionLabel")%><asp:TextBox ID="VersionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="VersionErrors" runat="server" />
		</div>	
	</div>






