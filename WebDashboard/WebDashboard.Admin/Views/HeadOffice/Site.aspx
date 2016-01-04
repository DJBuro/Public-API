<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<WebDashboardViewData.PageViewData>" %>
<%@ Import Namespace="WebDashboard.Mvc.Extensions"%>
<%@ Import Namespace="WebDashboard.Web.Models"%>
<%@ Import Namespace="WebDashboard.Dao.Domain"%>
<%@ Import Namespace="WebDashboard.Admin"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    <%=Html.Resource("Master, SiteDetails")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<h2><%= Html.ActionLink(Html.Resource("Master, Home"), "/", "HeadOffice")%> > <%= Html.ActionLink(Model.Site.HeadOffice.Name, "/Details/" + Model.Site.HeadOffice.Id, "HeadOffice")%> > <%= Html.ActionLink(Html.Resource("Master, Sites"), "/Sites/" + Model.Site.HeadOffice.Id, "HeadOffice")%>  > <%= Model.Site.Name%></h2>

<%
    using (Html.BeginForm("UpdateSite", "HeadOffice", FormMethod.Post))
    {
%> 
<%=Html.Hidden("Site.Id", Model.Site.Id)%>
<%=Html.Hidden("Site.HeadOffice.Id", Model.Site.HeadOffice.Id)%>
<table>
    <tr class="separator">
       <td colspan="4"></td>
    </tr> 
    <tr>
        <td colspan="3"><strong><%=Html.Resource("Master, SiteDetails")%></strong></td><td></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, RamesesStoreId")%></td><td><%=Html.Resource("Master, Name")%></td><td colspan="2"></td>
    </tr>
    <tr>
        <td><%=Html.TextBox("Site.SiteId", Model.Site.SiteId, new { @size = "10" })%></td><td><%=Html.TextBox("Site.Name", Model.Site.Name)%></td><td colspan="2"><%=Html.CheckBox("Site.Enabled", Model.Site.Enabled)%> <%=Html.Resource("Master, Enabled")%></td>
    </tr>
    <tr>
        <td><%=Html.Resource("Master, SiteType")%></td><td><%=Html.Resource("Master, IPAddress")%></td><td><%=Html.Resource("Master, SiteKey")%></td><td></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("Site.SiteType.Id", Model.SiteTypeListItems)%></td><td><%=Html.TextBox("Site.IPAddress", Model.Site.IPAddress)%></td><td><%=Html.TextBox("Site.Key", Model.Site.Key, new { @size = "10" })%></td><td></td>
    </tr>
    <tr>
        <td colspan="4"><%=Html.Resource("Master, Region")%></td>
    </tr>
    <tr>
        <td><%=Html.DropDownList("Site.Region.Id", Model.RegionListItems)%></td><td></td><td></td><td><input type="submit" value="<%=Html.Resource("Master, Update")%>" /></td>
    </tr>
    <tr class="separator">
       <td colspan="4"></td>
    </tr>
    <%
    }//end update siteRegion

    //you cannot remove an active site
    if (!Model.Site.Enabled)
    {
        using (Html.BeginForm("RemoveSite", "HeadOffice", FormMethod.Post))
        {
%> 
        <%=Html.Hidden("Site.Id", Model.Site.Id)%>
        <%=Html.Hidden("Site.HeadOffice.Id", Model.Site.HeadOffice.Id)%>
    <tr>
        <td colspan="3"><strong><%=Html.Resource("Master, RemoveSite")%></strong></td><td><input type="submit" value="<%=Html.Resource("Master, Remove")%>" /></td>
    </tr>
        <tr class="separator">
        <td colspan="4"></td>
    </tr>
    <%
    }//end removeSite
    }//end if%>
    <tr>
        <td colspan="4">
            <table>
                <tr>
                    <td colspan="3">
                        <%=Html.Resource("Master, LastUpdate")%>: <%= String.Format("{0:G}", Model.Site.LastUpdated)%>
                    </td>
                    <td>
                        <%
                            if (Model.Site.Enabled)
                            {
                                %>  
                                <%--<a href="http://dashboard.androtechnology.co.uk/flex/index.html#<%=Model.Site.Key%>" target="_blank">View <%= Model.Site.Name %>'s Dashboard</a>--%>
                                <a href="http://dashboard.androtechnology.co.uk/flex2/index.html#<%=Obfuscator.encryptString(Model.Site.Key.ToString())%>" target="_blank">View <%= Model.Site.Name%>'s Dashboard</a>
                                <%
                            } 
                        %>
                    </td>
                </tr>
                <tr>
                    <td align="right"><strong>Column</strong></td><td><strong>Data</strong></td><td><%= Html.ActionLink(Html.Resource("Master, RefreshData"), "/Site/" + Model.Site.Id, "HeadOffice")%></td>
                </tr>
                 <tr>
                    <td align="right"><strong>1 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column1", Model.Site.Column1)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>2 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column2", Model.Site.Column2)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>3 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column3", Model.Site.Column3)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>4 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column4", Model.Site.Column4)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>5 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column5", Model.Site.Column5)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>6 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column6", Model.Site.Column6)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>7 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column7", Model.Site.Column7)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>8 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column8", Model.Site.Column8)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>9 :</strong> </td><td colspan="2"><%=Html.TextBox("Site.Column9", Model.Site.Column9)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>10 :</strong></td><td colspan="2"><%=Html.TextBox("Site.Column10", Model.Site.Column10)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>11 :</strong></td><td colspan="2"><%=Html.TextBox("Site.Column11", Model.Site.Column11)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>12 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column12", Model.Site.Column12)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>13 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column13", Model.Site.Column13)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>14 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column14", Model.Site.Column14)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>15 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column15", Model.Site.Column15)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>16 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column16", Model.Site.Column16)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>17 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column17", Model.Site.Column17)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>18 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column18", Model.Site.Column18)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>19 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column19", Model.Site.Column19)%></td>
                </tr>
                <tr>
                    <td align="right"><strong>20 : </strong></td><td colspan="2"><%=Html.TextBox("Site.Column20", Model.Site.Column20)%></td>
                </tr>
            </table>
        </td>    
    </tr>
</table>

</asp:Content>