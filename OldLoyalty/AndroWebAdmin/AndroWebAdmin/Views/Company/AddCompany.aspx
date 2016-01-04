<%@ Page Title="Add New Company" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.CompanyViewData>"  %>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Add New Company
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    New Company Administration
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
                          <% using (Html.BeginForm("SaveCompany", "Company", FormMethod.Post))
                             {
                            %>
                            <tr>
                                <td>Company Name</td><td>Rameses Company Id</td><td>Rameses HeadOffice Id</td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("Name")%></td><td><%= Html.TextBox("RamesesCompanyId")%></td><td><%= Html.TextBox("RamesesHeadOfficeId")%></td>
                            </tr>
                            <tr>
                                <td>Key</td><td>Password</td><td>Only Whole Moneies</td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("CompanyKey")%></td><td><%= Html.TextBox("CompanyPassword")%></td><td><%= Html.CheckBox("OnlyWholeNumber")%></td>
                            </tr>
                            <tr>
                                <td>Maximum Redeemable Points</td><td>Redemption Ratio</td><td>Gain Ratio</td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.TextBox("MaxRedeemablePoints")%> = 1</td><td><%= Html.TextBox("RedemptionRatio")%> = 1</td><td>1 = <%= Html.TextBox("GainRatio")%></td>
                            </tr>
                            <tr>
                                <td>Country</td><td></td><td></td>
                            </tr>
                            <tr>
                                <td class="article_indent text-page text-page-indent"><%= Html.DropDownList("Country.Id",Model.CountryListItems)%></td><td></td><td><input type="submit" value="save" /></td>
                            </tr>
                            <%} %>
                          <tr >
                            <td colspan="3"><span class="article_separator"></span></td>
                          </tr>
                      </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>
</asp:Content>
