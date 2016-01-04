<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.LoyaltyCardViewData>" %>
<%@ Import Namespace="Loyalty.Dao.Domain"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Loyalty Card <%= Model.LoyaltyCard.CardNumber %> Details
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                   Loyalty Card '<%= Model.LoyaltyCard.CardNumber %>' Details
                </div>
            </div>
        </div>
    </div>
<%
    var cardStatus = "";
    
         foreach (LoyaltyCardStatus loyaltyCardStatus in Model.LoyaltyCard.LoyaltyCardStatus)
         {
             cardStatus = loyaltyCardStatus.Status.Name;
         }
%>
    <table cellpadding="0" cellspacing="0">
        <tr>
	        <td valign="top">
		        <div>
                    <div class="content-text">
                    <table class="contentpaneopen">
                        <tr>
                            <td class="article_indent text-page text-page-indent"><h2><%= Model.LoyaltyCard.CardNumber %></h2></td><td><%= cardStatus%></td><td>Pin: <%= (Model.LoyaltyCard.Pin ?? "none")  %></td><td><%= Html.ActionLink("View Loyalty Account", "Details/" + Model.LoyaltyCard.LoyaltyAccount.Id.Value, "LoyaltyAccount")%></td>
                        </tr>
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>
                        <tr>
                            <td class="article_indent text-page text-page-indent" colspan="4"><h2><%= Model.LoyaltyCard.TransactionHistory.Count %> Transactions</h2></td>
                        </tr>    
                        <tr>
                            <td><%= Model.LoyaltyCard.LoyaltyAccount.Site.Company.Name%></td><td>Order Id</td><td>Order Total</td><td>Loyalty Account Points: <%=Model.LoyaltyCard.LoyaltyAccount.Points %></td>
                        </tr>     
                        <tr >
                            <td colspan="4"><span class="article_separator"></span></td>
                        </tr>                
                    <%
                        foreach (TransactionHistory transactionHistory in Model.LoyaltyCard.TransactionHistory)
                        {
                       %>
                        <tr>
                            <td class="article_indent text-page text-page-indent" ><%= transactionHistory.Site.Name%></td><td><%= transactionHistory.OrderId%></td><td><%= transactionHistory.OrderTotal%></td><td class="togClick" id="<%= transactionHistory.Id%>"><a style="cursor:pointer;">View Details</a></td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <table class="allHistory" id="trans<%= transactionHistory.Id%>" style="margin-bottom:20px;margin-left:20px;">
                                     <tr>
                                        <td><strong>Name</strong></td><td align=center><strong>Item Points Gained</strong></td><td><strong>Price</strong></td>
                                     </tr>
                                    <%foreach (OrderItemHistory itemHistory in transactionHistory.OrderItemHistory)
                                      {%>
                                    <tr>
                                        <td><%= itemHistory.Name %></td><td align=center><%= itemHistory.ItemLoyaltyPoints != 0 ? itemHistory.ItemLoyaltyPoints.Value.ToString() : "n/a"%></td><td><%= itemHistory.ItemPrice %></td>
                                    </tr>
                                      
                                      <%}//OrderItemHistory
                                        %>
                                    <tr>
                                        <td colspan="4"><span class="article_separator"></span></td>
                                    </tr>  
                                     <tr>
                                        <td>Loyalty Points Added</td><td align=center>+ <%= transactionHistory.LoyaltyPointsAdded%></td><td></td>
                                     </tr>
                                     <tr>
                                        <td>Loyalty Points Redeemed</td><td align=center>- <%= transactionHistory.LoyaltyPointsRedeemed%></td><td></td>
                                     </tr>
                                     <tr>
                                        <td>Order Total</td><td></td><td><%= transactionHistory.OrderTotal%></td>
                                     </tr>
                                </table>                                    
                            </td>
                        </tr>
                            <%
                        }//TransactionHistory
                               %>
                          
                          
                          </table>

                        
                    </div>	

                </div>
	        </td>
        </tr>
    </table>
</div>
<script>
    $(document).ready(function() {

    $(".allHistory").toggle();

        $("td.togClick").click(function() {
            
            var thisId = ($(this).attr("id"));

            $("#trans" + thisId).toggle();

        });

    });

        /*   $(document).ready(function(){
        $("li.threadReport").click(function () { 
        $(this).slideUp(),
        $.post('/forum.ma/report-thread/5407', { id:$(this).attr("id") } )  
        });
            
        $("li.postReport").click(function () { 
        $(this).slideUp(),
        $.post('/forum.ma/report-post/0'.replace('0', $(this).attr("id") ),{ id:$(this).attr("id") } );
        });
            
        $("li.postRelease").click(function () { 
        $(this).slideUp(),
        $.post('/forum.ma/release-post/0'.replace('0', $(this).attr("id") ),{ id:$(this).attr("id") } );
        });
            
        });

    });
        
        <li class="postReport" id="56515"><a>Report Post</a></li>
        
        */
    

  </script>

</asp:Content>
