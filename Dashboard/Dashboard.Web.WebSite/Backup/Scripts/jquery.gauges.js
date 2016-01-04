/*
compress using:
    http://compressorrater.thruhere.net/
    YUI Compressor 2.4.2	preserveAllSemi, disableOpt
*/

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

    if (data == null) return false;

    gauge00DataTable.addColumn('number', data.Name);
    gauge00DataTable.addRows(1);
    gauge00DataTable.setValue(0, 0, data.CurrentValue);

    gaugeOptions00 = {
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

    gauge00.draw(gauge00DataTable, gaugeOptions00)
    updateSiteRanking(data.TopSite, "#top00");

    if (data.BottomSite != null) {
        updateSiteRanking(data.BottomSite, "#bot00");
    }

    if (data.CurrentValue != null) {
        updateGaugeRanking(data.CurrentValue, '#gauge00');
    }
    
    if (data.SiteRank == null) return false;
        $('#siteRank00').html('Site: ' + data.SiteRank);

    if (data.RegionRank == null) return false;
    $('#regionRank00').html('Region: ' + data.RegionRank);
//alert('bug hunting');
    return false;
}

function updateGauge01(data) {
    
    if (data == null) return false;

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
    updateSiteRanking(data.TopSite, "#top10");
    if (data.BottomSite != null) {
        updateSiteRanking(data.BottomSite, "#bot10");
    }

    if (data.CurrentValue != null) {
        updateGaugeRanking(data.CurrentValue, '#gauge01');
    }
    if (data.SiteRank == null) return false;
        $('#siteRank01').html('Site: ' + data.SiteRank);

    if (data.RegionRank == null) return false;
        $('#regionRank01').html('Region: ' + data.RegionRank);
    return false;
}

function updateGauge02(data) {

    if (data == null) return false;

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
    updateSiteRanking(data.TopSite, "#top20");
    
    if (data.BottomSite != null) {
        updateSiteRanking(data.BottomSite, "#bot20");
    }
    if (data.CurrentValue != null) {
        updateGaugeRanking(data.CurrentValue, '#gauge02');
    }
    if (data.SiteRank == null) return false;
        $('#siteRank02').html('Site: ' + data.SiteRank);
        
    if (data.RegionRank == null) return false;
        $('#regionRank02').html('Region: ' + data.RegionRank);
    return false;
}

function updateGauge03(data) {

    if (data == null) return false;

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
    updateSiteRanking(data.TopSite, "#top30");

    if (data.BottomSite != null) {
        updateSiteRanking(data.BottomSite, "#bot30");
    }
    if (data.CurrentValue != null) {
        updateGaugeRanking(data.CurrentValue, '#gauge03');
    }
    if (data.SiteRank == null) return false;
        $('#siteRank03').html('Site: ' + data.SiteRank);
    if (data.RegionRank == null) return false;
        $('#regionRank03').html('Region: ' + data.RegionRank);
    return false;
}

function updateGauge04(data) {

    if (data == null) return false;

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
    updateSiteRanking(data.TopSite, "#top40");

    if (data.BottomSite != null) {
        updateSiteRanking(data.BottomSite, "#bot40");
    }
    if (data.CurrentValue != null) {
        updateGaugeRanking(data.CurrentValue, '#gauge04');
    }
    if (data.SiteRank == null) return false;
        $('#siteRank04').html('Site: ' + data.SiteRank);
        
    if (data.RegionRank == null) return false;
    $('#regionRank04').html('Region: ' + data.RegionRank);

        
    return false;
}

function updateSiteRanking(data, cssClass) {
    $.each(data, function(i, itemData) {
        //if (i == 6) return false;
        if (itemData.indicatorType == "euro") {
            $(cssClass + i).html(itemData.rank + ". " + itemData.name + " <strong>€" + itemData.indicator.toFixed(2) + "</strong>");
        }
        else if (itemData.indicatorType == "pound") {
        $(cssClass + i).html(itemData.rank + ". " + itemData.name + " <strong>£" + itemData.indicator.toFixed(2) + "</strong>");
        }
        else {
            $(cssClass + i).html(itemData.rank + ". " + itemData.name + " <strong>" + itemData.indicator + "" + itemData.indicatorType + "</strong>");
        }
    });
    return false;
}

/*
To see if the Site's stats are going up or down:
Using jQuerys internal 'data()' method to store state
Not very well documented, but uses a 'key/value' pair
*/
function updateGaugeRanking(currentValue, cssClass) {

    if ($(cssClass).data('value')) {
        if (currentValue == $(cssClass).data('value')) {
           // alert('same');
            $(cssClass + 'Rank').html('<img src="../../Content/imgs/same.gif" />');
         }
         if (currentValue > $(cssClass).data('value')) {
            $(cssClass + 'Rank').html('<img src="../../Content/imgs/up.gif" />');
        }
        if (currentValue < $(cssClass).data('value')) {
            $(cssClass + 'Rank').html('<img src="../../Content/imgs/down.gif" />');
        }
        
        $(cssClass).data('value', currentValue); //set the new value
    }
    else { //first time in - set the value
        $(cssClass).data('value', currentValue);
        $(cssClass + 'Rank').html('<img src="../../Content/imgs/up.gif" />');
    }
    return false;
}

function updateDashboard(data) {

        var gaugeCount = 0;
        for (i in data.Gauges) // in returns key, not object
        {
            if (data.Gauges[i] != undefined)
                gaugeCount++;
        }
        
        /*we always assume that the first 5 elements are gauges*/
    updateGauge00(data.Gauges[0]);
    updateGauge01(data.Gauges[1]);
    updateGauge02(data.Gauges[2]);
    updateGauge03(data.Gauges[3]);
    updateGauge04(data.Gauges[4]);
    
    //alert(data.Gauges[5].MaxValue);
    if (gaugeCount > 5) {
       // alert(gaugeCount);
        updateColumn(data.Gauges);
    }

    return false;
}
/*update the columns that don't have gauges*/
function updateColumn(data) {
    $.each(data, function(i, itemData) {

        if (i > 4) {
            var indicatorType = "";

            if (itemData.TopSite[0] == null)
            { return false; }

            if (itemData.TopSite[0].indicatorType == "euro") {
                indicatorType = "€";
            }
            else if (itemData.TopSite[0].indicatorType == "pound") {
                indicatorType = "£";
            }
            //alert(itemData.indicatorType);
            $('#cntTitle' + i + '0').html(itemData.Name + ': ' + indicatorType + itemData.CurrentValue);

            $('#cnRank' + i).html('Site: ' + itemData.SiteRank);
            $('#cnRegion' + i).html('Region: ' + itemData.RegionRank);

            updateSiteRanking(itemData.TopSite, '#cnTop' + i + '0');

            if (itemData.CurrentValue != null) {
                updateGaugeRanking(itemData.CurrentValue, '#containerId' + i);
            }

            if (itemData.BottomSite != null) {
                updateSiteRanking(itemData.BottomSite, '#cnBot' + i + '0');
            }

        }
    });
}

function updateScollingOTDData(scrollingData) {
    $("#scrollOTD").html(scrollingData);
    return false;
}

function UpdateScrollingTicketsData(scrollingData) {
    $("#scrollTickets").html(scrollingData);
    return false;
}

function updateScollingOPDData(scrollingData) {
    $("#scrollOPD").html(scrollingData);
    return false;
}
//use ratios to get define the screen layout
//bottom
//1024 x 768 =     1019 x 602 ff -0.5907 (57 - 60)
//                  988 x 588 ie -0.5951 (59)
//bottom
//1280 x 1024 =    1272 x 858 ff -0.674 (66 - 68)
//                 1251 x 844 ie -0.674 (67)
//right
//1440 x 1024 =    1432 x 734 ff -0.5125 (49 - 51)
//                 1410 x 721 ie -0.5113 (51)
function getRes(width, height) {
    var scale = ((height / width) * 100)

    var returnval = 'bottom';
    //alert(scale + ": " +width + " x " + height);
    if (scale > 45 & scale < 53) {
        returnval = 'right';
    }
    return returnval;
}

//Make all the gauge columns the same height
function setHeight(col) {

    var maxHeight = 0;
    //Get all the element with class = col
    col = $(col);

    //Loop all the col
    col.each(function() {

        //Store the highest value
        if ($(this).width() > maxHeight) {
            maxHeight = $(this).width();
        }
    });
    
    //Set the height (to the same as the width - eg a square)
    col.height(maxHeight);

    return false;
}

//set the width of the bottom scrollers 
function setPointerWidth(pointer,documentWidth) {
    var maxWidth = 0;
    pointer = $(pointer);
    pointer.width(documentWidth - 10);
    return false;
}
