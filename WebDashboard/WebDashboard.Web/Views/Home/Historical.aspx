<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Dao.Domain.Helpers"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	historical data
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <table>
        <tr>
            <%
                foreach (Indicator indicator in Model.Indicators)
                {
                    %>
                    <td><strong><%= Html.ActionLink(indicator.LongName, "Historical/" + indicator.Id, "Home")%></strong></td>
                    <%
                } %>
        </tr>
    </table>
    <h2><%=Model.Indicator.Definition.Name %></h2>
    <table>
        <tr>
            <td><strong>Rank</strong></td><td><strong>Name</strong></td><td><strong><%=Model.Indicator.ValueType.Value%></strong></td>
        </tr>
        <%
            foreach (SiteRank siteRank in Model.SiteRanks)
            {
                %>
                
                        <tr>
            <td><%= siteRank.rank %></td><td><%= siteRank.name %></td><td><%= siteRank.value %></td>
        </tr>
                
                <%
            }//end foreach siteRank %>
    </table>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="HelpContent" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="ValidatorContent" runat="server">
</asp:Content>
