<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Dashboard.Master"  Inherits="System.Web.Mvc.ViewPage<DashboardViewData.HeadOfficeViewData>" %>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Display
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<!--
  copyright (c) 2009 Google inc.

  You are free to copy and use this sample.
  License can be found here: http://code.google.com/apis/ajaxsearch/faq/#license
-->
    <script type="text/javascript" src="http://www.google.com/jsapi"></script>
    
    <script type="text/javascript">
      google.load('visualization', '1', {packages: ['gauge']});
    </script>
    
    <script type="text/javascript">

        var gauge00;
        var gauge01;
        var gauge02;
        var gauge03;
        var gauge04;

        var gauge00DataTable;
        var gauge01DataTable;
        var gauge02DataTable;
        var gauge03DataTable;
        var gauge04DataTable;

   
    function drawGauge() {
       
       gauge00DataTable = new google.visualization.DataTable();     
       gauge01DataTable = new google.visualization.DataTable();
       gauge02DataTable = new google.visualization.DataTable();
       gauge03DataTable = new google.visualization.DataTable();
       gauge04DataTable = new google.visualization.DataTable();
    
      gauge00 = new google.visualization.Gauge(document.getElementById('gauge00'));
      gauge01 = new google.visualization.Gauge(document.getElementById('gauge01'));
      gauge02 = new google.visualization.Gauge(document.getElementById('gauge02'));
      gauge03 = new google.visualization.Gauge(document.getElementById('gauge03'));
      gauge04 = new google.visualization.Gauge(document.getElementById('gauge04'));
  }
    
    google.setOnLoadCallback(drawGauge);

    function updateGauge00(data) {

        gauge00DataTable.addColumn('number', data.Name);
        gauge00DataTable.addRows(1);
        gauge00DataTable.setValue(0, 0, data.CurrentValue);
        
        gaugeOptions00 = {
            min: 0,
            max: data.MaxValue,
            redFrom: 0,
            redTo: data.Benchmark -10,
            yellowFrom: data.Benchmark - 10,
            yellowTo: data.Benchmark,
            greenFrom: data.Benchmark,
            greenTo: data.MaxValue,
            minorTicks: 5
        };
        
        gauge00.draw(gauge00DataTable, gaugeOptions00);
    }
    
    
    function updateGauge01(data) {

        gauge01DataTable.addColumn('number', data.Name);
        gauge01DataTable.addRows(1);
        gauge01DataTable.setValue(0, 0, data.CurrentValue);

        gaugeOptions01 = {
            min: 0,
            max: data.MaxValue,
            redFrom: 0,
            redTo: data.Benchmark - 10,
            yellowFrom: data.Benchmark - 10,
            yellowTo: data.Benchmark,
            greenFrom: data.Benchmark,
            greenTo: data.MaxValue,
            minorTicks: 5
        };
        
        gauge01.draw(gauge01DataTable, gaugeOptions01);
    }

    function updateGauge02(data) {

        gauge02DataTable.addColumn('number', data.Name);
        gauge02DataTable.addRows(1);
        gauge02DataTable.setValue(0, 0, data.CurrentValue);

        gaugeOptions02 = {
            min: 0,
            max: data.MaxValue,
            redFrom: 0,
            redTo: data.Benchmark - 10,
            yellowFrom: data.Benchmark - 10,
            yellowTo: data.Benchmark,
            greenFrom: data.Benchmark,
            greenTo: data.MaxValue,
            minorTicks: 5
        };

        gauge02.draw(gauge02DataTable, gaugeOptions02);
    }

    function updateGauge03(data) {

        gauge03DataTable.addColumn('number', data.Name);
        gauge03DataTable.addRows(1);
        gauge03DataTable.setValue(0, 0, data.CurrentValue);

        gaugeOptions03 = {
            min: 0,
            max: data.MaxValue,
            redFrom: 0,
            redTo: data.Benchmark - 10,
            yellowFrom: data.Benchmark - 10,
            yellowTo: data.Benchmark,
            greenFrom: data.Benchmark,
            greenTo: data.MaxValue,
            minorTicks: 5
        };

        gauge03.draw(gauge03DataTable, gaugeOptions03);
    }

    function updateGauge04(data) {

        gauge04DataTable.addColumn('number', data.Name);
        gauge04DataTable.addRows(1);
        gauge04DataTable.setValue(0, 0, data.CurrentValue);

        gaugeOptions04 = {
            min: 0,
            max: data.MaxValue,
            redFrom: 0,
            redTo: data.Benchmark - 10,
            yellowFrom: data.Benchmark - 10,
            yellowTo: data.Benchmark,
            greenFrom: data.Benchmark,
            greenTo: data.MaxValue,
            minorTicks: 5
        };

        gauge04.draw(gauge04DataTable, gaugeOptions04);
    }


    function updateSiteRanking(data) {

        $.each(data, function(i, itemData) {

            //if (i == 6) return false;
           // alert(itemData.name);

        });
    
    }

    function updateDashboard(data) {
        updateGauge00(data.Gauges[0]);
        updateGauge01(data.Gauges[1]);
        updateGauge02(data.Gauges[2]);
        updateGauge03(data.Gauges[3]);
        updateGauge04(data.Gauges[4]);
        updateSiteRanking(data.TopSite)
        return false;
    }

    function updateScollingData(scrollingData) {
    
    $("#01").html("<div class='columnScroller'>")
    $.each(scrollingData, function(i, itemData) {

    //if (i == 6) return false;

    $(".columnScroller").append("<div class='sites'><dl><dt id='" + i + "'>" + itemData.LocationName + "</dt><dd>InStore " + itemData.InStore + "</dd><dd>OPD " + itemData.OPD + "</dd><dd>Avg " + itemData.Avg + "</dd><dd>Tick " + itemData.Tick + "</dd></dl></div>");

        });

        $(".columnScroller").append("</div>");
    
        return false;
    } 
    
    </script>
    
    <script type="text/javascript">
        //<![CDATA[
        $(function() {

            /*Page Load - Grab the data right away*/
            $(this).oneTime(1000, function() {

                $.getJSON('/Dashboard/UpdateDisplayData/', { siteId: "<%= Model.Site.Id %>" }, function(data) {
                    updateDashboard(data);
                });

                $.getJSON('/Dashboard/UpdateScrollingData/', { siteId: "<%= Model.Site.Id %>" }, function(srollingData) {
                    updateScollingData(srollingData);
                });

            });


            /*Grab the data every x seconds */
            $(this).everyTime(25000, function() {

                $.getJSON('/Dashboard/UpdateDisplayData/', { siteId: "<%= Model.Site.Id %>" }, function(data) {
                    updateDashboard(data);
                });

            }, true); //true == 'belaying' is on!

            

        });
        //]]>
  </script>
    <script type="text/javascript">
        $(function() {

	//ID, class and tag element that font size is adjustable in this array
	//Put in html or body if you want the font of the entire page adjustable
var section = new Array('#siteMessage','.columnScroller');
	section = section.join(',');

	// Reset Font Size
	var originalFontSize = $(section).css('font-size');  
		$(".resetFont").click(function(){
		$(section).css('font-size', originalFontSize);
	});
	// Increase Font Size
	$(".increaseFont").click(function(){
		var currentFontSize = $(section).css('font-size');
		var currentFontSizeNum = parseFloat(currentFontSize, 10);
		var newFontSize = currentFontSizeNum*1.2;
		$(section).css('font-size', newFontSize);
		return false;
	});
  
	// Decrease Font Size
	$(".decreaseFont").click(function(){
		var currentFontSize = $(section).css('font-size');
		var currentFontSizeNum = parseFloat(currentFontSize, 10);
		var newFontSize = currentFontSizeNum*0.8;
		$(section).css('font-size', newFontSize);
		return false;
	});
});
  </script>
  
  
<div id="siteMessage">
    <%= Model.HeadOfficeMessage %>
    <br/>
</div>
   
    <div class="gaugeColumns" >
        <div class="gauge" id="gauge00" style="width: 100%; height: 100%;"></div>      
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge01" style="width: 100%; height: 100%;"></div>
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge02" style="width: 100%; height: 100%;"></div>
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge03" style="width: 100%; height: 100%;"></div>
    </div>
    <div class="gaugeColumns">
        <div class="gauge" id="gauge04" style="width: 100%; height: 100%;"></div>
    </div>

<div class="siteScroller">
    <marquee behavior="scroll" direction="left" scrollamount="3" width="1000"><div id="01"></div></marquee>
</div>

</asp:Content>
