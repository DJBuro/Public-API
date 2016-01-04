<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyCardViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Loyalty Cards
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.LoyaltyCards.Count %> Loyalty Cards
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
                    <%
                        foreach (Company company in Model.Companies)
                        {%>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= company.Name %></h2></td><td><%= Html.ActionLink(company.LoyaltyAccounts.Count + " Loyalty Accounts", "Company/" + company.Id.Value, "LoyaltyAccount")%></td><td><%= Html.ActionLink("View Cards", "Company/" + company.Id.Value, "LoyaltyCard")%></td>
                        </tr>
                        <%} %>
                    
                    <tr >
                        <td colspan="3"><span class="article_separator"></span></td>
                        </tr>
                    <%
                        foreach (Status status in Model.Statuses)
                       {
                       %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= status.LoyaltyCardByStatus.Count%> <%= status.Name %></h2></td><td><%= status.Description %></td><td><%= Html.ActionLink("View Cards", "Status/" + status.Id.Value, "LoyaltyCard")%></td>
                        </tr>
                        
                          <%} %>
                          
                          </table>

                        
                    </div>	

                </div>
	        </td>
        </tr>
    </table>
</div>

</asp:Content>
