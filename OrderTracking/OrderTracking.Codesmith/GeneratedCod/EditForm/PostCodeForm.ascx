<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PostCodeForm.ascx.cs" Inherits="OrderTracking.Data.PostCodeForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editPostCoderegion fields">
				<%# GetMessage("PostCodeLabel")%><asp:TextBox ID="PostCodeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="PostCodeErrors" runat="server" />
				<%# GetMessage("Del1Label")%><asp:TextBox ID="Del1Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del1Errors" runat="server" />
				<%# GetMessage("LongitudeLabel")%><asp:TextBox ID="LongitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LongitudeErrors" runat="server" />
				<%# GetMessage("LatitudeLabel")%><asp:TextBox ID="LatitudeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="LatitudeErrors" runat="server" />
				<%# GetMessage("Del2Label")%><asp:TextBox ID="Del2Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del2Errors" runat="server" />
				<%# GetMessage("Del3Label")%><asp:TextBox ID="Del3Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del3Errors" runat="server" />
				<%# GetMessage("Del4Label")%><asp:TextBox ID="Del4Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del4Errors" runat="server" />
				<%# GetMessage("Del5Label")%><asp:TextBox ID="Del5Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del5Errors" runat="server" />
				<%# GetMessage("Del6Label")%><asp:TextBox ID="Del6Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del6Errors" runat="server" />
				<%# GetMessage("Del7Label")%><asp:TextBox ID="Del7Text" runat="server"></asp:TextBox><Cacd:CacdValidationError id="Del7Errors" runat="server" />
		</div>	
	</div>






