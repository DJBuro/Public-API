<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.GlobalViewData>"%>
<%@ Import Namespace="System.Linq.Expressions"%>
<%@ Import Namespace="AndroWebAdmin.Mvc.Utilities"%>
<%@ Import Namespace="AndroWebAdmin.Controllers"%>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Loyalty Card Global Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<%
    
    const int varId = -1;
    int varId2;
    Expression<Action<CompanyController>> sitesIndex = c => c.Sites(varId);
    string siteUrl = SiteLinkBuilder.BuildUrlFromExpression(ViewContext, sitesIndex, "sitestest");
    
     %>
         <script type="text/javascript">

             $(document).ready(function() {

             //$("a[href]").click(function() {
                $("td.SitesMess a[href]").click(function() {
                $.replace('<%=siteUrl %>', $(this).attr("id"));
                    });
            });
    </script>
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    Loyalty Global Administration
                </div>
            </div>
        </div>
    </div>
    <table cellpadding="0" cellspacing="0">
        <tr>
	        <td valign="top">
		        <div>
                    <div class="content-text">
                        <table class="contentpaneopen">
                            <tr >
                                <td colspan="4"><span class="article_separator"></span></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><h2><%= Model.Countries.Count %> Countries</h2></td><td></td><td></td><td><%= Html.ActionLink("Add New Country", "AddCountry/", "Global")%></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"></td><td><strong>ISO-Code</strong></td><td><strong>Currency</strong></td><td></td>
                            </tr>
                        <%
                            varId2 = 0;
                            //varId2 = Model.Countries[0].Id.Value;
                            foreach (var country in Model.Countries)
                            {
                               //;
                                %>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= country.Name %></td><td><%= country.ISOCode %>  </td><td> <%foreach (CountryCurrency countryCurrency in country.CountryCurrencies) { %> <%= countryCurrency.Currency.Symbol %> <%  } %> </td><td><%= Html.ActionLink("Edit Country", "EditCountry/" + country.Id.Value, "Global")%></td>
                            </tr>
                            <%} %>
                        <tr>
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= Model.Currencies.Count %> Currencies</h2></td><td></td><td></td><td><%= Html.ActionLink("Add New Currency", "AddCurrency/", "Global")%></td>
                        </tr>
                        <tr>
                            <td class="article_indent text-page text-page-indent"></td><td><strong>Currency Name</strong></td><td><strong>Symbol</strong></td><td></td>
                        </tr>
                        <%
                            foreach (var currency in Model.Currencies)
                            {%>
                            <tr>
                                <td class="article_indent text-page text-page-indent"></td><td><%= currency.Name %></td><td><%= currency.Symbol %></td><td><%= Html.ActionLink("Edit Currency", "EditCurrency/" + currency.Id.Value, "Global")%></td>
                            </tr>
                            <%} %>
                        <tr>
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                        <% if (Model.LoyaltyLogs.Count > 0)
                           { %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%=Model.LoyaltyLogs.Count%> Loyalty Log Files</h2></td><td></td><td></td><td><%= Html.ActionLink("Clear Log", "ClearLog", "Global")%></td>
                        </tr>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><strong>Site Id</strong></td><td><strong>Message</strong></td><td><strong>Time Logged</strong></td><td></td>
                        </tr>
                        
                        
                        <%
                            foreach (var log in Model.LoyaltyLogs)
                            {%>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= log.SiteId != 0 ? log.SiteId.ToString() : "none"%></td><td><%= log.Message%></td><td><%=String.Format("{0:F}", log.DateTimeCreated)%></td><td><%= Html.ActionLink("View Details", "ViewLog/" + log.Id.Value, "Global")%></td>
                            </tr>
                            <%} %>
                        <tr>
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                        <%}//Loyalty Logs count %>
                     </table>
                 </div>	

                </div>
	        </td>
        </tr>
    </table>
</div>
</asp:Content>
