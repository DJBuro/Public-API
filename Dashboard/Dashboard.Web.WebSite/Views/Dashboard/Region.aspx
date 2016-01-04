<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<DashboardViewData.HeadOfficeViewData>" %>
<%@ Import Namespace="Dashboard.Web.WebSite.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Region
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


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
