<%@ Page Language="C#"  Inherits="System.Web.Mvc.ViewPage<DashboardViewData.ScrollerViewData>"  %>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<link href="../../Content/Scroller.css" rel="stylesheet" type="text/css" />
<script src="../../Scripts/jquery-1.3.2.min.js" type="text/javascript"></script>
<script src="../../Scripts/jquery.marqueeSingle.js" type="text/javascript"></script>

   <script type="text/javascript">
        $(document).ready(function() {
        $('div.scrollerOPD marquee').marqueeSingle('pointer');
        });
    </script>
<head runat="server">
    <title>OPD</title>
</head>
<body>
    <div>
    <div class="scrollerOPD"><marquee behavior="scroll" direction="left" scrollamount="4" width="100%"><%= Model.ScrollOPD %></marquee></div>
    </div>
</body>
</html>
