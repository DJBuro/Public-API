<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GatemessagerecordForm.ascx.cs" Inherits="OrderTracking.Gps.Data.GatemessagerecordForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editGatemessagerecordregion fields">
				<%# GetMessage("DataboolLabel")%><asp:TextBox ID="DataboolText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DataboolErrors" runat="server" />
				<%# GetMessage("DataintLabel")%><asp:TextBox ID="DataintText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DataintErrors" runat="server" />
				<%# GetMessage("DatadoubleLabel")%><asp:TextBox ID="DatadoubleText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DatadoubleErrors" runat="server" />
				<%# GetMessage("DatatextLabel")%><asp:TextBox ID="DatatextText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DatatextErrors" runat="server" />
				<%# GetMessage("DatalongtextLabel")%><asp:TextBox ID="DatalongtextText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DatalongtextErrors" runat="server" />
				<%# GetMessage("DatadatetimeLabel")%><asp:TextBox ID="DatadatetimeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DatadatetimeErrors" runat="server" />
				<%# GetMessage("SavechangesonlyLabel")%><asp:TextBox ID="SavechangesonlyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="SavechangesonlyErrors" runat="server" />
		</div>	
	</div>






