<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardViewData.HeadOfficeViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Regional Dashboard for <%= Model.Site.SiteName%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="siteMessage">
    <%= Model.HeadOfficeMessage%>
</div>
    <div id="upsells">

    <%foreach (var dd in Model.Dash.DashData) 
  {%>
        <dl>
                <dt><%=dd.IndicatorName%></dt>
                <dd class="upsellImage">
<%--                <% if (dd.ChartType == 1)
                   {%>--%>
                
                
                <img src="http://chart.apis.google.com/chart?chts=000000,12&amp;chs=140x150&amp;chf=bg,s,D8CEB2|c,s,D8CEB2&amp;chxt=x,y&amp;chxl=0:|<%=dd.ChartIndicatorValue%>/<%=dd.IndicatorBenchmark%>|1:|0.00|100&amp;cht=bvs&amp;chd=t:<%=dd.IndicatorValue%>&amp;chco=<%=dd.ChartColor%>&amp;chbh=40" alt="AltTag"/>
<%--                <%}
                   else
                   { %>
                   <img src="http://chart.apis.google.com/chart?chs=170x150&amp;chf=bg,s,D8CEB2&amp;cht=gom&amp;chd=t:<%=dd.IndicatorValue%>&amp;chco=ff0000,ff6600,ffff00,00ff00" alt="Google Chart"/>
                    <%} %>
                    --%>
                    </dd>
                <dd>Site Ranking: <%= dd.SiteRank%></dd>
                <dd>Region Ranking: <%= dd.RegionRank%></dd>

                <dd><p>Top Sites</p></dd>
                

                    <%foreach (Dashboard.Dao.Domain.Helpers.RankSite topSite in dd.TopSites)
                      {  %>
                        <dd><%= topSite.Rank%>. <%=Html.ActionLink(topSite.Site.SiteName, "Site/" + topSite.Site.Id.Value.ToString())%> </dd>
                    <%} %>
                
                <dd><p>Bottom Sites</p></dd>
                    <%foreach (Dashboard.Dao.Domain.Helpers.RankSite bottomSite in dd.BottomSites)
                      {  %>
                        <dd><%= bottomSite.Rank%>. <%=Html.ActionLink(bottomSite.Site.SiteName, "Site/" + bottomSite.Site.Id.Value.ToString())%></dd>
                    <%} %>
               

        </dl>
        <%} %>

    </div>
    <div id="league">
    <dl>
        <dt>Performance League Table</dt>
        <dd>OTD/OPR/Instore</dd>
    </dl>
    </div>
<%--    <div class="demo">
            <marquee behavior="scroll" direction="left" scrollamount="3" width="927"><%= Model.Scroller%></marquee>
    </div>
    --%>
        <div class="demo">
            <marquee behavior="scroll" direction="left" scrollamount="3" width="927">hello</marquee>
    </div>
     <div id="tickets">Tickets: </div>
    <div class="tickerTape"><marquee behavior="scroll" direction="left" scrollamount="4" width="868"><%= Model.ScrollTickets %></marquee>
         
    </div>
        <div id="tickets">OPD: </div>
         <div class="tickerTape"><marquee behavior="scroll" direction="left" scrollamount="4" width="892"><%= Model.ScrollOPD %></marquee>
        </div>
        <div id="tickets">OTD: </div>
         <div class="tickerTape"><marquee behavior="scroll" direction="left" scrollamount="4" width="892"><%= Model.ScrollOTD %></marquee>
   </div>
</asp:Content>
