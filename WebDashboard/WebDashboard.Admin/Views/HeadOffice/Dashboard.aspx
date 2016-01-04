<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.Name, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > Dashboard</h2>

<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
  <%


  foreach (var indicator in Model.Indicators)
  {
      using (Html.BeginForm("UpdateIndicator", "HeadOffice", FormMethod.Post))
      {
          %>
    <input id="Id" name="Id" type="hidden" value="<%=indicator.Id.Value%>" />       
    <tr>
        <td colspan="4"><strong><%= indicator.Definition.Name %></strong></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, LongName")%></td><td></td><td align="center" colspan="2"><strong><%= Html.ActionLink(Html.Resource("Master, AdvancedSettings"), "/Dashboard/Indicator/" + Model.HeadOffice.Id + "/" + indicator.Id, "HeadOffice")%></strong></td>
    </tr>
    <tr>
        <td><input id="LongName" name="LongName" type="text" value="<%= indicator.LongName %>" /></td><td></td><td>Column Type</td><td><%= indicator.IndicatorType.Name %></td>
    </tr> 
        <tr>
        <td><%=Html.Resource("Master, ShortName")%></td><td></td><td><%=Html.Resource("Master, ValueType")%></td><td><%= indicator.ValueType.Name%></td>
    </tr>
    <tr>
        <td><input id="ShortName" name="ShortName" size="10" type="text" value="<%=indicator.ShortName %>" /></td><td></td><td></td><td></td>
    </tr> 
    <tr>
        <td><%=Html.Resource("Master, MaximumValue")%></td><td></td><td>Column</td><td><%=indicator.Definition.ColumnNumber%></td>
    </tr>
    <tr>
        <td><input id="MaxValue" name="MaxValue" size="10" type="text" value="<%=indicator.MaxValue%>" /></td><td></td><td colspan="2"><%= (indicator.AllowZero) ? Html.Resource("Master, AllowZeroValues") : Html.Resource("Master, ZeroValuesIgnored")%></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, Benchmark")%></td><td></td><td colspan="2"><%= (indicator.ReverseSort) ? Html.Resource("Master, BetterLower") : Html.Resource("Master, BetterHigher")%></td>
    </tr>
    <tr>
       <td><input id="BenchMark" name="BenchMark" size="10" type="text" value="<%=indicator.BenchMark %>" /></td><td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td><td colspan="2"><%= (indicator.DisplayAsScroller) ? Html.Resource("Master, DisplayAsScroller") : ""%></td>
    </tr>   
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
          }//end foreach
 }//end form 
        %>
</table>

</asp:Content>