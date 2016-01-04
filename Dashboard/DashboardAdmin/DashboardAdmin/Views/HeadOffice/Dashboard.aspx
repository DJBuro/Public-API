<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardAdminViewData.IndexViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="DashboardAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
/*    $(document).ready(function() {


        var section = new Array('html');
        section = section.join(',');

        var originalFontSize = $('html').css('font-size');

        $('html').css('font-size', originalFontSize * 2);
        

        var newFontSize = ($('html').css('font-size') * 1.2);

        $('html').css('font-size', 20);


alert($('html').css('font-size'));
*/
        //global variable, this will store the highest height value
/*        var maxHeight = 0;
        function setHeight(col) {
            //Get all the element with class = col
            col = $(col);

            //Loop all the col
            col.each(function() {

                //Store the highest value
                if ($(this).height() > maxHeight) {
                    maxHeight = $(this).height(); ;
                }
            });

            //Set the height
            col.height(maxHeight);
        }
        setHeight('.regionalColumns');

    });

    $(document).ready(function() {
        $("input[type=checkbox][checked]").each(
            function(i) {
                if (i != 4) {
                    $("#gaugeWarn").html("You can only have 5 gauges displayed");
                    $("input[type=submit]").attr("disabled", true);
                }
                else {
                    $("#gaugeWarn").html("&nbsp;");
                    $("input[type=submit]").attr("disabled", false);
                }
     });

    $("input[type=checkbox]").click(
        function() {
            $("input[type=checkbox][checked]").each(
            function(i) {
                if (i != 4) {
                    $("#gaugeWarn").html("You can only have 5 gauges displayed");
                    $("input[type=submit]").attr("disabled", true);
                }
                else {
                    $("#gaugeWarn").html("&nbsp;");
                    $("input[type=submit]").attr("disabled", false);
                 }
            }
        );
      });
});
*/
</script>

    <h2><%= Html.ActionLink("Home", "/", "HeadOffice")%> > <%= Html.ActionLink(Model.HeadOffice.HeadOfficeName, "/Details/" + Model.HeadOffice.Id, "HeadOffice")%>  > Dashboard</h2>

<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
  <%


  foreach (var indicatorDefinition in Model.IndicatorDefinitions)
  {
          var blah = (IndicatorTranslation) indicatorDefinition.IndicatorTranslations[0];
      using (Html.BeginForm("UpdateDashboard", "HeadOffice", FormMethod.Post))
      {
          %>
        <input id="Id" name="Id" type="hidden" value="<%=indicatorDefinition.Id.Value%>" />

       <tr>
            <td colspan="4"><strong><%=Html.ActionLink(blah.TranslatedIndicatorName,
                                                "/Translation/" + Model.HeadOffice.Id + "/" + blah.Id, "HeadOffice")%></strong> (<%=indicatorDefinition.IndicatorName%>)</td>
       </tr>
        <tr>
            <td>Benchmark</td><td>Divisor</td><td></td><td><%=Html.Hidden("Id", indicatorDefinition.Id.Value)%></td>
       </tr>
    <tr>
       <td><%=Html.TextBox("BenchMark", indicatorDefinition.BenchMark)%></td><td><%=Html.TextBox("Divisor", indicatorDefinition.Divisor)%></td><td><%=Html.CheckBox("ReverseSortingOrder",
                                              indicatorDefinition.ReverseSortingOrder)%> Reverse Order</td><td><%=Html.CheckBox("ShowGauge", indicatorDefinition.ShowGauge)%> Show as Gauge</td>
    </tr>
      <tr>
            <td colspan ="3"></td><td><input type="submit" value="Update" /></td>
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
