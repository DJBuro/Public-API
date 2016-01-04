<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyUserViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Loyalty Users
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                    <%= Model.LoyaltyUsers.Count %> Registered Users
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
                        foreach (LoyaltyUser loyaltyUser in Model.LoyaltyUsers)
                        {
                            
                         %>
                        <tr>
                            <td class="article_indent text-page text-page-indent"><%= loyaltyUser.LoyaltyAccount.Site.Company.Name%></td><td><%= loyaltyUser.LoyaltyAccount.Site.Name%></td><td><%= loyaltyUser.UserTitle.Title %> <%= loyaltyUser.FirstName %> <%= loyaltyUser.SurName %></td><td><%= Html.ActionLink("View Account", "Details/" + loyaltyUser.LoyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
                        </tr>
                        <%}//loyatlyUser %>
                        <tr >
                            <td colspan="4"><span class="article_separator"></span>
                            </td>
                        </tr>
                        
                          </table>
                    </div>	
                </div>
	        </td>
        </tr>
    </table>
</div>
</asp:Content>
