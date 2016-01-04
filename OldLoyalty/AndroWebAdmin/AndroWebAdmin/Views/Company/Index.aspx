<%@ Page Title="Company Administration" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.CompanyViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="System.Linq.Expressions"%>
<%@ Import Namespace="AndroWebAdmin.Controllers"%>
<%@ Import Namespace="AndroWebAdmin.Mvc.Utilities"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Company Administration
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    Company Administration
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
                        <tr>
                            <td class="article_indent text-page text-page-indent"></td><td></td><td></td><td><%= Html.ActionLink("Create New Company", "AddCompany/", "Company")%></td>
                        </tr>
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                    <%
                        foreach (var company in Model.Companies)
                       {
                       %>
                            <tr>
                                <td class="article_indent text-page text-page-indent" colspan="3"><h2><%= company.Name %></h2></td><td><%= Html.ActionLink("Edit Company", "EditCompany/" + company.Id.Value, "Company")%></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= company.Country.Name %></td><td>Loyalty Company Id <%= company.Id%> </td><td>Rameses Company Id <%= company.RamesesCompanyId%></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent">Max Redeemable Points: <%= company.MaxRedeemablePoints%></td><td><%= Html.ActionLink(company.LoyaltyAccounts.Count + " Loyalty Accounts", "Company/" + company.Id.Value, "LoyaltyAccount")%></td><td><%= Html.ActionLink(company.Sites.Count + " Sites", "Sites/" + company.Id.Value, "Company")%></td><td><%= Html.ActionLink("Add New Site", "AddSite/" + company.Id.Value, "Company")%></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent">RedemptionRatio: <%= company.RedemptionRatio%> = 1</td><td>Gain Ratio: 1 = <%= company.GainRatio%></td><td>Only Whole Moneies: <%= company.OnlyWholeNumber ? "YES":"NO" %></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><strong>User Titles</strong></td><td></td><td></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent">
                                <select>                     
                                <%
                               foreach (CompanyUserTitle c in company.CompanyUserTitles)
                               {%>
                               <option> <%= c.UserTitle.Title %></option> 
                               <%    //todo: usertitles page
                               } %></select>  
                                </td><td></td><td></td><td><%= Html.ActionLink("Edit User Titles", "UserTitles/" + company.Id.Value, "Company")%></td>
                            </tr>
                          <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
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
