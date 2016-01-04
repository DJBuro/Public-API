<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Dashboard.Master"  Inherits="System.Web.Mvc.ViewPage<DashboardViewData.DisplayViewData>" %>
<%@ Import Namespace="Dashboard.Dao.Domain"%>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	<%= Model.Site.SiteName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script type="text/javascript" src="http://www.google.com/jsapi"></script>
    
    <script type="text/javascript">
        google.load('visualization', '1', { packages: ['gauge'] });
    </script>
    <script type="text/javascript">
        $(document).ready(function() {
            $('div.siteScroller marquee').marquee('pointer');
            $('div.tickerTape marquee').marquee('pointer');
        });
    </script>
    <script src="../../Scripts/jquery.gauges.js" type="text/javascript"></script>
    <%--<script src="../../Scripts/jquery.gauges.compressed.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        var timer = setInterval("autoRefresh()", 1000 * 60 * 300);//force a refresh every 5 hours
        function autoRefresh(){self.location.reload(true);}
    </script>    
   
 <script type="text/javascript">

        //<![CDATA[
$(function() {
         /*Page Load - Grab the data right away*/
        $(this).oneTime(10, function() {
            var fontSize = ((document.documentElement.clientWidth * 0.94) / 70);
            makeCalls(fontSize);   
         });


     /*Grab the data every x seconds */
      $(this).everyTime(10000, function() {
          var fontSize = ((document.documentElement.clientWidth * 0.94) / 70);
          makeCalls(fontSize);
     });


     function makeCalls(fontSize) {
          $.ajaxSetup({ cache: false }); //IE Cache Fix
         $.getJSON('/Dashboard/Display/UpdateDisplayData/', { siteId: "<%= Model.Site.Id %>" }, function(data) {
             updateDashboard(data);
         });
/*
         $.getJSON('/Dashboard/Display/UpdateScrollingOtdData/', { siteId: "<%= Model.Site.Id %>" }, function(scrollingData) {
             updateScollingOTDData(scrollingData);
         });
         $.getJSON('/Dashboard/Display/UpdateScrollingTicketsData/', { siteId: "<%= Model.Site.Id %>" }, function(scrollingData) {
             UpdateScrollingTicketsData(scrollingData);
         });

         $.getJSON('/Dashboard/Display/UpdateScrollingOpdData/', { siteId: "<%= Model.Site.Id %>" }, function(scrollingData) {
             updateScollingOPDData(scrollingData);
         });
*/
         var alignment = getRes(document.documentElement.clientWidth, document.documentElement.clientHeight);

         $('#containerId0').toggleClass("verticalContainer", alignment == 'right');
         $('#containerId0').toggleClass("horizontalContainer", alignment != 'right');

         $('html').css('font-size', fontSize);

         if (alignment == 'right') {
            $('.gaugeColumns').css('width', '16%');
            $('.verticalContainer').css('font-size', fontSize * .74 );
            $('.verticalContainer').css('width', '13%');
        }        

        setHeight('.gauge');
        setPointerWidth('.pointer', document.documentElement.clientWidth * 0.94);
     }

 });
    //]]>
  </script>
  
  
  
    <div class="siteMessage">
       
       <%= Model.Site.SiteName %>
       <%if (Model.HeadOfficeMessage != null)
         {%>
        -  <%= Model.HeadOfficeMessage%>
        <%} %>
       <div id="siteDisplayText"> </div>
      <div id="siteLoading"> <%--Last Updated: 
        <%
         var xy = (DashboardData) Model.Site.DashboardData[0];  %>
         <%= String.Format("{0:F}", xy.LastUpdated.Value) %>--%>
        </div>
        
    </div>
    <div class="gaugeColumns">
   
        <div class="gauge" id="gauge00" >Loading Gauge...</div>
        <div class="gaugeRanking">
            <dl>
                <dt>Rankings</dt>
                <dd id="siteRank00">no recent data</dd>
                <dd id="regionRank00">no recent data</dd>
                <dd id="gauge00Rank">no recent data</dd>
            </dl>
        </div>
       <div class="topSite">
            <dl>
                <dt>Leaders</dt>
                <dd id="top000">no recent data</dd>
                <dd id="top001">no recent data</dd>
                <dd id="top002">no recent data</dd>
            </dl>
            <dl>
                <dt>Laggers</dt>
                <dd id="bot000">no recent data</dd>
                <dd id="bot001">no recent data</dd>
                <dd id="bot002">no recent data</dd>
            </dl>
        </div>
     </div>
    
    <div class="gaugeColumns">
        <div class="gauge" id="gauge01">Loading Gauge...</div>
        <div class="gaugeRanking">
            <dl>
                <dt>Rankings</dt>
                <dd id="siteRank01">no recent data</dd>
                <dd id="regionRank01">no recent data</dd>
                <dd id="gauge01Rank">no recent data</dd>
            </dl>
        </div>
        <div class="topSite">
            <dl>
                <dt>Leaders</dt>
                <dd id="top100">no recent data</dd>
                <dd id="top101">no recent data</dd>
                <dd id="top102">no recent data</dd>
            </dl>
            <dl>
                <dt>Laggers</dt>
                <dd id="bot100">no recent data</dd>
                <dd id="bot101">no recent data</dd>
                <dd id="bot102">no recent data</dd>
            </dl>
        </div>
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge02">Loading Gauge...</div>
        <div class="gaugeRanking">
            <dl>
                <dt>Rankings</dt>
                <dd id="siteRank02">no recent data</dd>
                <dd id="regionRank02">no recent data</dd>
                <dd id="gauge02Rank">no recent data</dd>
            </dl>
        </div>
        <div class="topSite">
            <dl>
                <dt>Leaders</dt>
                <dd id="top200">no recent data</dd>
                <dd id="top201">no recent data</dd>
                <dd id="top202">no recent data</dd>
            </dl>
            <dl>
                <dt>Laggers</dt>
                <dd id="bot200">no recent data</dd>
                <dd id="bot201">no recent data</dd>
                <dd id="bot202">no recent data</dd>
            </dl>
        </div> 
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge03">Loading Gauge...</div>
        <div class="gaugeRanking">
            <dl>
                <dt>Rankings</dt>
                <dd id="siteRank03">no recent data</dd>
                <dd id="regionRank03">no recent data</dd>
                <dd id="gauge03Rank">no recent data</dd>
            </dl>
        </div>
        <div class="topSite">
            <dl>
                <dt>Leaders</dt>
                <dd id="top300">no recent data</dd>
                <dd id="top301">no recent data</dd>
                <dd id="top302">no recent data</dd>
            </dl>
            <dl>
                <dt>Laggers</dt>
                <dd id="bot300">no recent data</dd>
                <dd id="bot301">no recent data</dd>
                <dd id="bot302">no recent data</dd>
            </dl>
        </div> 
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge04">Loading Gauge...</div>
        <div class="gaugeRanking">
            <dl>
                <dt>Rankings</dt>
                <dd id="siteRank04">no recent data</dd>
                <dd id="regionRank04">no recent data</dd>
                <dd id="gauge04Rank">no recent data</dd>
            </dl>
        </div>
        <div class="topSite">
            <dl>
                <dt>Leaders</dt>
                <dd id="top400">no recent data</dd>
                <dd id="top401">no recent data</dd>
                <dd id="top402">no recent data</dd>
            </dl>
            <dl>
                <dt>Laggers</dt>
                <dd id="bot400">no recent data</dd>
                <dd id="bot401">no recent data</dd>
                <dd id="bot402">no recent data</dd>
            </dl>
        </div> 
    </div>
    
   <div id="containerId0">
        <div class="containerRanking">
            <dl>
                <dt id="cntTitle50">no recent data</dt>
                <dd id="cnRank5">no recent data</dd>
                <dd id="cnRegion5">no recent data</dd>
                <dd id="containerId5Rank">no recent data</dd>
            </dl>
         </div>
         <div class="containerRanking">
            <dl>
                <dt>Leaders</dt>
                <dd id="cnTop500">no recent data</dd>
                <dd id="cnTop501">no recent data</dd>
                <dd id="cnTop502">no recent data</dd>
            </dl>
         </div>
         <div class="containerRanking">
            <dl>
                <dt>Laggers</dt>
                <dd id="cnBot500">no recent data</dd>
                <dd id="cnBot501">no recent data</dd>
                <dd id="cnBot502">no recent data</dd>
            </dl>
          </div>

        <div class="containerRanking">
            <dl>
                <dt id="cntTitle60">no recent data</dt>
                <dd id="cnRank6">no recent data</dd>
                <dd id="cnRegion6">no recent data</dd>
                <dd id="containerId6Rank">no recent data</dd>
            </dl>
         </div>
         <div class="containerRanking">
            <dl>
                <dt>Leaders</dt>
                <dd id="cnTop600">no recent data</dd>
                <dd id="cnTop601">no recent data</dd>
                <dd id="cnTop602">no recent data</dd>
            </dl>
         </div>
         <div class="containerRanking">
            <dl>
                <dt>Laggers</dt>
                <dd id="cnBot600">no recent data</dd>
                <dd id="cnBot601">no recent data</dd>
                <dd id="cnBot602">no recent data</dd>
            </dl>
          </div>
    </div>
 <%--   <div class="tickerTape">
        <div class="scrollTitle">OTD:</div> 
        <marquee behavior="scroll" direction="left" scrollamount="3"><div id="scrollOTD"></div></marquee>
    </div>
    <div class="tickerTape" >
        <div class="scrollTitle">OPD:</div>
        <marquee behavior="scroll" direction="left" scrollamount="4"><div id="scrollOPD"></div></marquee>
    </div>
    <div class="tickerTape" >
        <div class="scrollTitle">Tick:</div>
        <marquee behavior="scroll" direction="left" scrollamount="3"><div id="scrollTickets"></div></marquee>
    </div>--%>
</asp:Content>
