<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardViewData.IndexViewData>"  %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Dashboard for <%= Model.Site.SiteName%>
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div id="siteMessage">
    <%= Model.HeadOfficeMessage%>
</div>
    <div id="upsells">
    <%foreach (var dd in Model.Dash.DashData)
  {%>
        <dl>
                <dt><%=dd.IndicatorName%></dt>
                <dd class="upsellImage"><img src="http://chart.apis.google.com/chart?chts=000000,12&amp;chs=140x150&amp;chf=bg,s,D8CEB2|c,s,D8CEB2&amp;chxt=x,y&amp;chxl=0:|<%=dd.ChartIndicatorValue%>/<%=dd.IndicatorBenchmark%>|1:|0.00|100&amp;cht=bvs&amp;chd=t:<%=dd.IndicatorValue%>&amp;chco=<%=dd.ChartColor%>&amp;chbh=40" alt="AltTag"/></dd>
                <dd><ul>
                <li>Site Ranking: <%= dd.SiteRank%></li>
                <li>Region Ranking: <%= dd.RegionRank%></li></ul>
                </dd>
                <dd><p>Top Sites</p></dd>
                
                <%foreach (Dashboard.Dao.Domain.Helpers.RankSite topSite in dd.TopSites)
                  {  %>
                    <dd><%= topSite.Rank %>. <%= topSite.Site.SiteName %></dd>
                <%} %>

                
                
                <dd><p>Bottom Sites</p></dd>
                
                   
                <%foreach (Dashboard.Dao.Domain.Helpers.RankSite bottomSite in dd.BottomSites)
                  {  %>
                    <dd><%= bottomSite.Rank %>. <%= bottomSite.Site.SiteName%></dd>
                <%} %>

                


        </dl>
        <%} %>

    </div>
<%--         <div class="demo">
            <marquee behavior="scroll" direction="left" scrollamount="3" width="900"><%= Model.Scroller%></marquee>
        </div>--%>
         <div class="demo">
            <marquee behavior="scroll" direction="left" scrollamount="3" width="900">Hello</marquee>
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
<%--<p><img src="http://chart.apis.google.com/chart?chs=400x180&amp;chtt=<%=Model.Dash.IndicatorName%>&amp;cht=gom&amp;chd=t:80&amp;chl=£80&amp;chco=00ff00,00ff00,ff0000,ff0000" alt="Google Chart"/></p>
    <p>
<%=Model.Dash.IndicatorName%>
<%=Model.Dash.IndicatorValue%>
<%=Model.Dash.IndicatorBenchmark%>--%>

<%--<p><img src="http://chart.apis.google.com/chart?chtt=Delivery Times&amp;chts=000000,12&amp;chs=300x150&amp;chf=bg,s,ffffff|c,s,ffffff&amp;chxt=x,y&amp;chxl=0:|%>20min|%>30min|1:|0.00|100&amp;cht=bvs&amp;chd=t:15,25|75,80&amp;chdl=Benchmark&amp;chco=ff0000,00ff00&amp;chbh=40,20" alt="AltTag"/></p>

<p><img src="http://chart.apis.google.com/chart?chtt=Delivery Times&amp;chts=000000,12&amp;chs=300x150&amp;chf=bg,s,ffffff|c,s,ffffff&amp;chxt=x,y&amp;chxl=0:|%>20min|1:|0.00|100&amp;cht=bvs&amp;chd=t:15&amp;chdl=Benchmark&amp;chco=ff0000&amp;chbh=40" alt="AltTag"/></p>
--%>

<%--<p><img src="http://chart.apis.google.com/chart?chtt=<%=dd.IndicatorName%>&amp;chts=000000,12&amp;chs=300x150&amp;chf=bg,s,ffffff|c,s,ffffff&amp;chxt=x,y&amp;chxl=0:|<%=dd.ChartIndicatorValue%>|1:|0.00|100&amp;cht=bvs&amp;chd=t:<%=dd.ChartIndicatorValue%>&amp;chdl=Benchmark <%=dd.IndicatorBenchmark%>&amp;chco=<%=dd.ChartColor%>&amp;chbh=40" alt="AltTag"/></p>
--%>
<%--    <p><img src="http://chart.apis.google.com/chart?chtt=<%=dd.IndicatorName%>&amp;chts=000000,12&amp;chs=200x150&amp;chf=bg,s,ffffff|c,s,ffffff&amp;chxt=x,y&amp;chxl=0:|Money|1:|0.00|100%&amp;cht=bvs&amp;chd=t:<%=dd.IndicatorBenchmark%>|<%=dd.ChartIndicatorValue%>&amp;chdl=Benchmark&amp;chco=ff0000,00ff00&amp;chbh=40" alt="<%=dd.IndicatorName%>"/></p>--%>


<%--<%foreach (Site topSite in dd.TopSites)
  {  %>

<%= topSite.SiteName %> siteID:<%= topSite.Id%>
<br />
<%} %>
bottom<br />
<%foreach (Site bottomSite in dd.BottomSites)
  {  %>

<%= bottomSite.SiteName%> siteID:<%= bottomSite.Id%>
<br />
<%} %>--%>


<%--
IndicatorValue: <%=dd.IndicatorValue%>
<br />
IndicatorBenchmark: <%=dd.IndicatorBenchmark%>
<br />
ChartIndicatorValue: <%=dd.ChartIndicatorValue%>
<br/>
SiteRank: <%= dd.SiteRank%>
<br/>
RegionRank: <%= dd.RegionRank%>--%>


</asp:Content>
