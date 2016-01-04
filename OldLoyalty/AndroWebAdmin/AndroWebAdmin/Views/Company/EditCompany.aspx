<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.CompanyViewData>" %>
<%@ Import Namespace="AndroWebAdmin.Controllers"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Edit <%= Model.Company.Name %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.Company.Name %> Administration
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
                            <td class="article_indent text-page text-page-indent"></td><td></td><td><a href="#">Remove Company</a></td>
                        </tr>
                        <tr >
                            <td colspan="3"><span class="article_separator"></span>
                        </tr>
                        <% using (Html.BeginForm("SaveCompany","Company",FormMethod.Post))
                           {
                            %>
                            <%= Html.Hidden("Id",Model.Company.Id) %>
                            <tr>
                                <td>Company Name</td><td>Rameses Company Id</td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("Name",Model.Company.Name) %></td><td><%= Html.TextBox("RamesesCompanyId", Model.Company.RamesesCompanyId)%></td><td></td>
                            </tr>
                            <tr>
                                <td>Key</td><td>Password</td><td>Only Whole Moneies</td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("CompanyKey", Model.Company.CompanyKey)%></td><td><%= Html.TextBox("CompanyPassword", Model.Company.CompanyPassword)%></td><td><%= Html.CheckBox("OnlyWholeNumber", Model.Company.OnlyWholeNumber)%></td>
                            </tr>
                            <tr>
                                <td>Maximum Redeemable Points</td><td>Redemption Ratio</td><td>Gain Ratio</td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("MaxRedeemablePoints", Model.Company.MaxRedeemablePoints)%> = 1</td><td><%= Html.TextBox("RedemptionRatio", Model.Company.RedemptionRatio)%> = 1</td><td>1 = <%= Html.TextBox("GainRatio", Model.Company.GainRatio)%></td>
                            </tr>
                            <tr>
                                <td>Country</td><td></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.DropDownList("Country.Id", Model.CountryListItems) %></td><td></td><td><input type="submit" value="save" /></td>
                            </tr>
                            <%} %>
                          <tr>
                            <td colspan="3"><span class="article_separator"></span></td>
                          </tr>
                        <tr>
                            <td><%= Html.ActionLink(Model.Company.LoyaltyAccounts.Count + " Loyalty Accounts", "Company/" + Model.Company.Id.Value, "LoyaltyAccount")%></td><td><%= Html.ActionLink(Model.Company.Sites.Count + " Sites", "Sites/" + Model.Company.Id.Value, "Company")%></td><td></td>
                        </tr>
                      </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>

</asp:Content>
