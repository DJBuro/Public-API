<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScriptpluginForm.ascx.cs" Inherits="OrderTracking.Gps.Data.ScriptpluginForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editScriptpluginregion fields">
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("CategoryLabel")%><asp:TextBox ID="CategoryText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CategoryErrors" runat="server" />
				<%# GetMessage("DescriptionLabel")%><asp:TextBox ID="DescriptionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DescriptionErrors" runat="server" />
				<%# GetMessage("FilepathLabel")%><asp:TextBox ID="FilepathText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="FilepathErrors" runat="server" />
				<%# GetMessage("VersionLabel")%><asp:TextBox ID="VersionText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="VersionErrors" runat="server" />
				<%# GetMessage("LoadorderLabel")%><asp:TextBox ID="LoadorderText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LoadorderErrors" runat="server" />
				<%# GetMessage("DeletedLabel")%><asp:TextBox ID="DeletedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DeletedErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
		</div>	
	</div>






