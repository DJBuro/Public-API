<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <h2>Dashboard</h2>
    <%
        using (Html.BeginForm("SaveMessage", "Home", FormMethod.Post))
        {
%><%= Html.HiddenFor(c => c.HeadOffice.Id) %>
    <table>
        <tr>
            <td colspan="3"><strong>Head Office Message</strong></td>
        </tr>
        <tr>
            <td colspan="3"><%= Html.TextAreaFor(c => c.HeadOffice.Message, new { @cols = "60" })%></td>
        </tr>
        <tr>
            <td colspan="2"></td><td style="text-align:right"><input type="submit" value="Update" /></td>
        </tr>
        <tr class="separator">
       <td colspan="3"></td>
    </tr>
    <%} %>
   </table>
<h2>Dashboard Columns</h2>
<table>
  <%
        var i = 0;
  foreach (var indicator in Model.Indicators)
  {
      i++;
      using (Html.BeginForm("Dashboard", "Home", FormMethod.Post, new { @id = "form" + i }))
      {
          %>
    <tr>
        <td colspan="2"><input id="Id" name="Id" type="hidden" value="<%=indicator.Id.Value%>" /><strong><%= indicator.Definition.Name %></strong></td><td></td>
    </tr>
    <tr>
        <td>Display Name</td><td>Scroller Name</td><td><%= indicator.IndicatorType.Id.Value == 2 ? "Benchmark" :""%></td>
    </tr>    
    <tr>
        <td><input id="LongName" name="LongName" type="text" value="<%= indicator.LongName %>" class="{validate:{required:true,minlength:2,messages:{required:'Please enter a Translated Name'}}}" /></td><td><input id="ShortName" name="ShortName" size="5" type="text" value="<%= indicator.ShortName%>"  class="{validate:{required:true,maxlength:5,messages:{required:'Please enter a Short Name'}}}" /></td><td <%= indicator.IndicatorType.Id.Value == 2 ? "" :"style='display:none;'" %>><input id="BenchMark" name="BenchMark" size="3" type="text" value="<%=indicator.BenchMark %>" />/<%= indicator.MaxValue %><%= indicator.ValueType.Value%></td>
    </tr> 
    <tr>
        <td><%= indicator.IndicatorType.Name%></td><td><%=Html.CheckBox("DisplayAsScroller", indicator.DisplayAsScroller)%> Display as Scroller</td><td></td>
    </tr>
   <tr>
        <td colspan="2"></td><td><input type="submit" value="Update" /></td>
    </tr> 
        <tr class="separator">
       <td colspan="3"></td>
    </tr>
    <%
          }//end foreach
 }//end form 
        %>
</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
                        <li>The Head Office Message is prefixed by the store's name, make sure its not too long</li>
                        <li>Gauge Columns have a gauge and a benchmark</li>
                        <li>Simple Columns don't have a benchmark or a gauge, and are on the far right of the dashboard</li>
                        <li>Display Name is what will be displayed on the dashboard, translate this to your language</li>
                        <li>Scroller Name should be as short as possible</li>
                        <li>The dashboard can only have 3 scrollers</li>
                        <li>Changes may take up to 30 seconds to display on the dashboard</li>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
<script type="text/javascript">
    $(document).ready(function() {

        var container = $('div.container');

        var validator = $("#form1").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
        var validator = $("#form2").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
        var validator = $("#form3").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
        var validator = $("#form4").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
        var validator = $("#form5").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
        var validator = $("#form6").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
        var validator = $("#form7").validate({
            errorContainer: container,
            errorLabelContainer: $("ul", container),
            wrapper: 'li',
            meta: "validate"
        });
    });
</script>
</asp:Content>