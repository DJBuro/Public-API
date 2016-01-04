<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SchedulertaskForm.ascx.cs" Inherits="OrderTracking.Gps.Data.SchedulertaskForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editSchedulertaskregion fields">
				<%# GetMessage("TasknameLabel")%><asp:TextBox ID="TasknameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TasknameErrors" runat="server" />
				<%# GetMessage("TaskgroupLabel")%><asp:TextBox ID="TaskgroupText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TaskgroupErrors" runat="server" />
				<%# GetMessage("ExeccountLabel")%><asp:TextBox ID="ExeccountText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ExeccountErrors" runat="server" />
				<%# GetMessage("LastexectimestampLabel")%><asp:TextBox ID="LastexectimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LastexectimestampErrors" runat="server" />
				<%# GetMessage("NextexectimestampLabel")%><asp:TextBox ID="NextexectimestampText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NextexectimestampErrors" runat="server" />
				<%# GetMessage("StateLabel")%><asp:TextBox ID="StateText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="StateErrors" runat="server" />
				<%# GetMessage("TaskassemblynameLabel")%><asp:TextBox ID="TaskassemblynameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TaskassemblynameErrors" runat="server" />
				<%# GetMessage("TasktypenameLabel")%><asp:TextBox ID="TasktypenameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="TasktypenameErrors" runat="server" />
		</div>	
	</div>






