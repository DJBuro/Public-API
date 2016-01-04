<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardViewData.HeadOfficeViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Regional Dashboard
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="siteMessage">
    <%= Model.HeadOfficeMessage%>
</div>
<div id="upsells" >
    <%foreach (var dd in Model.Regions)
     
      {%>
      <dl>
                <dt><%=Html.ActionLink(dd.RegionName, "Region/" + dd.Id.Value.ToString())%></dt>
                    <%foreach (SitesRegion sr in dd.SitesRegions)
                      {%>        
                    <dd><%=Html.ActionLink(sr.Site.SiteName, "Site/" + sr.Site.Id.Value.ToString()) %></dd>
                    <%
                    }%>
      </dl>
  <%} %>
   
<%--      <dl>
                <dt>dt</dt>
                    <dd>
                    <img src="http://chart.apis.google.com/chart?chtt=Bar+Chart&amp;chts=000000,12&amp;chs=178x150&amp;chf=bg,s,D8CEB2|c,s,D8CEB2&amp;chxt=x,y&amp;chxl=0:|zw|nw|zo|no|1:|0.00|17.50|35.00&amp;cht=bvs&amp;chd=t:100.00,60.00,48.57,54.28&amp;chco=FF5914&amp;chbh=25" alt="Google Chart"/>
                    </dd>
      
      </dl>--%>
     
</div>
         <div class="demo">
            <marquee behavior="scroll" direction="left" scrollamount="3" width="927"><%= Model.Scroller%></marquee>
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
