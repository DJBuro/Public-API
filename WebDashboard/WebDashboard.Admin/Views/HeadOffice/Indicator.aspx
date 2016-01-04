<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%=Html.Resource("Master, AdvancedSettings")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > <%= Html.ActionLink("Dashboard", "/Dashboard/" + Model.HeadOffice.Id, "HeadOffice")%> > <%=Html.Resource("Master, AdvancedSettings") %></h2>
  <%
      using (Html.BeginForm("UpdateAdvancedIndicator", "HeadOffice", FormMethod.Post))
      {
          %>
          <input id="Id" name="Id" type="hidden" value="<%=Model.Indicator.Id.Value%>" /> 
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <tr>
        <td><strong><%= Model.Indicator.Definition.Name%></strong></td>
    </tr>
        <tr>
        <td>Column Type</td><td><%=Html.Resource("Master, ValueType")%></td><td></td><td><%=Html.CheckBox("AllowZero", Model.Indicator.AllowZero)%> <%=Html.Resource("Master, AllowZeroValues")%></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("IndicatorType.Id", Model.IndicatorTypeListItems)%></td><td><%=Html.DropDownList("ValueType.Id", Model.ValueTypeListItems)%></td><td></td><td><%=Html.CheckBox("ReverseSort", Model.Indicator.ReverseSort)%> <%=Html.Resource("Master, BetterLower")%></td>
    </tr>
    <tr>
        <td>Column</td><td></td><td></td><td><%=Html.CheckBox("DisplayAsScroller", Model.Indicator.DisplayAsScroller)%> <%=Html.Resource("Master, DisplayAsScroller")%></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("Definition.Id", Model.DefinitionListItems)%></td><td></td><td></td><td></td>
    </tr>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <tr>
       <td><strong><%=Html.Resource("Master, Calculations")%></strong></td><td></td><td></td><td></td>
    </tr>
    <tr>
       <td align="right">Divisor</td><td><%=Html.DropDownList("DivisorType.Id", Model.DivisorTypeListItems, new { @id = "divType" })%></td><td></td><td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td>
    </tr>
    <tr id="divisorColumn">
       <td align="right">Divisor Column</td><td><%=Html.TextBox("DivisorColumn", Model.Indicator.DivisorColumn, new { @size = "10" })%></td><td></td><td></td>
    </tr>    
    <tr id="customValue">
       <td align="right">Custom Value</td><td><%=Html.TextBox("CustomDivisorValue", Model.Indicator.CustomDivisorValue, new { @size = "10" })%></td><td></td><td></td>
    </tr>    
  </table>
<%
      } %>

      <script type="text/javascript">
          $(document).ready(function() {
              $('#divisorColumn').hide();
              $('#customValue').hide();

              $("#divType").change(displayVals);
              displayVals();

              function displayVals() {

                  if ($('#divType').val() == 1) {
                      $('#divisorColumn').show();
                      $('#customValue').hide();
                  }

                  if ($('#divType').val() == 2) {
                      $('#divisorColumn').hide();
                      $('#customValue').hide();
                  }

                  if ($('#divType').val() == 3) {
                      $('#divisorColumn').hide();
                      $('#customValue').hide();
                  }

                  if ($('#divType').val() == 4) {
                      $('#divisorColumn').hide();
                      $('#customValue').show();
                  }
              }
          });
      </script>
</asp:Content>
