<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MapdataForm.ascx.cs" Inherits="OrderTracking.Gps.Data.MapdataForm" %>

	<div class="contentForm">
		<h4><asp:Label ID="caption" runat="server"></asp:Label></h4>
        <Cacd:CacdValidationSummary ID="ValidationSummary" runat="server" />
        <div class="editMapdataregion fields">
				<%# GetMessage("BotypeLabel")%><asp:TextBox ID="BotypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BotypeErrors" runat="server" />
				<%# GetMessage("NameLabel")%><asp:TextBox ID="NameText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="NameErrors" runat="server" />
				<%# GetMessage("BmtilewidthLabel")%><asp:TextBox ID="BmtilewidthText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BmtilewidthErrors" runat="server" />
				<%# GetMessage("BmtileheightLabel")%><asp:TextBox ID="BmtileheightText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BmtileheightErrors" runat="server" />
				<%# GetMessage("BmtotalwidthLabel")%><asp:TextBox ID="BmtotalwidthText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BmtotalwidthErrors" runat="server" />
				<%# GetMessage("BmtotalheightLabel")%><asp:TextBox ID="BmtotalheightText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="BmtotalheightErrors" runat="server" />
				<%# GetMessage("GrididLabel")%><asp:TextBox ID="GrididText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GrididErrors" runat="server" />
				<%# GetMessage("DatumidLabel")%><asp:TextBox ID="DatumidText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="DatumidErrors" runat="server" />
				<%# GetMessage("GeomineLabel")%><asp:TextBox ID="GeomineText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeomineErrors" runat="server" />
				<%# GetMessage("GeominnLabel")%><asp:TextBox ID="GeominnText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeominnErrors" runat="server" />
				<%# GetMessage("GeomaxeLabel")%><asp:TextBox ID="GeomaxeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeomaxeErrors" runat="server" />
				<%# GetMessage("GeomaxnLabel")%><asp:TextBox ID="GeomaxnText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="GeomaxnErrors" runat="server" />
				<%# GetMessage("ProjtypeLabel")%><asp:TextBox ID="ProjtypeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjtypeErrors" runat="server" />
				<%# GetMessage("ProjorigoeLabel")%><asp:TextBox ID="ProjorigoeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjorigoeErrors" runat="server" />
				<%# GetMessage("ProjorigonLabel")%><asp:TextBox ID="ProjorigonText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjorigonErrors" runat="server" />
				<%# GetMessage("ProjdedxLabel")%><asp:TextBox ID="ProjdedxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjdedxErrors" runat="server" />
				<%# GetMessage("ProjdedyLabel")%><asp:TextBox ID="ProjdedyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjdedyErrors" runat="server" />
				<%# GetMessage("ProjdndxLabel")%><asp:TextBox ID="ProjdndxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjdndxErrors" runat="server" />
				<%# GetMessage("ProjdndyLabel")%><asp:TextBox ID="ProjdndyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjdndyErrors" runat="server" />
				<%# GetMessage("XmlfilepathLabel")%><asp:TextBox ID="XmlfilepathText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="XmlfilepathErrors" runat="server" />
				<%# GetMessage("VirtualpathLabel")%><asp:TextBox ID="VirtualpathText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="VirtualpathErrors" runat="server" />
				<%# GetMessage("CreatedLabel")%><asp:TextBox ID="CreatedText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="CreatedErrors" runat="server" />
				<%# GetMessage("ProjorigoxLabel")%><asp:TextBox ID="ProjorigoxText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjorigoxErrors" runat="server" />
				<%# GetMessage("ProjorigoyLabel")%><asp:TextBox ID="ProjorigoyText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjorigoyErrors" runat="server" />
				<%# GetMessage("ProjdvdeLabel")%><asp:TextBox ID="ProjdvdeText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjdvdeErrors" runat="server" />
				<%# GetMessage("ProjdrdnLabel")%><asp:TextBox ID="ProjdrdnText" runat="server"></asp:TextBox><Cacd:CacdValidationError id="ProjdrdnErrors" runat="server" />
		</div>	
	</div>






