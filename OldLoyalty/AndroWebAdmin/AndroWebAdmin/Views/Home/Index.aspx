<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<AndroWebAdminViewData.IndexViewData>" %>
<%@ Import Namespace="AndroWebAdmin.Controllers"%>
<%@ Import Namespace="System.Linq.Expressions"%>
<%@ Import Namespace="AndroWebAdmin.Mvc.Utilities"%>
<%@ Import Namespace="AndroWebAdmin.Models"%>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Main Menu</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

<div id="container">
    <div class="container-indent">
        <div class="clear">
           <div class="indent-text">
                <div class="componentheading">
                Loyalty Administration
                </div>
            </div>
        </div>
    </div>

    <table class="blog" cellpadding="0" cellspacing="0">
        <tr>
	        <td valign="top">
		        <div>
		        
                    <div class="wrapper-title ">
                <div class="clear contentpaneopen">
                    <div class="fright">
                        <div class="icon-indent">
                                <a href="#" title="PDF" rel="nofollow"><img src="Content/images/pdf_button.png" alt="PDF"  /></a>                            
                                <a href="#" title="Print" rel="nofollow"><img src="Content/images/printButton.png" alt="Print"  /></a>                            
                                <a href="#" title="E-mail"><img src="Content/images/emailButton.png" alt="E-mail"  /></a>
                         </div>
                    </div>
                    
                    <div class="fleft contentheading">
                        <div class="title">Not sure what to put here</div>
					    <div class="small"><span>Written by Administrator</span>&nbsp;&nbsp;</div>
                        <div class="createdate">Wednesday, June 10th 2009</div>
                    </div>
                    
                </div>
            </div>

                    <div class="content-text">
                        <table class="contentpaneopen">
                            <tr>
                                <td valign="top" colspan="2" class="article_indent text-page text-page-indent">
                                    Some more text here, as we need a landing page, this is what you will see first... hopefully something more important than what I am writting right now.                    
                                     <div  class="modifydate">
                                            Last Updated ( Wednesday June 10th 2009 15:14 )
                                    </div>
                                </td>
                            </tr>
                        </table>

                        <div class="indent-article-separator"><span class="article_separator"></span>
                        </div>
                    </div>	

                </div>
	        </td>
        </tr>

    <%--<tr>
	    <td valign="top" align="center" class="text-links">
		    &lt;&lt; <span class="pagenav">Start</span> &lt; <span class="pagenav">Prev</span> <span class="pagenav">1</span> <a title="2" href="/joomla_23849/index.php?option=com_content&amp;view=category&amp;layout=blog&amp;id=31&amp;Itemid=63&amp;limitstart=3" class="pagenav">2</a> <a title="3" href="/joomla_23849/index.php?option=com_content&amp;view=category&amp;layout=blog&amp;id=31&amp;Itemid=63&amp;limitstart=6" class="pagenav">3</a> <a title="Next" href="/joomla_23849/index.php?option=com_content&amp;view=category&amp;layout=blog&amp;id=31&amp;Itemid=63&amp;limitstart=3" class="pagenav">Next</a> &gt; <a title="End" href="/joomla_23849/index.php?option=com_content&amp;view=category&amp;layout=blog&amp;id=31&amp;Itemid=63&amp;limitstart=6" class="pagenav">End</a> &gt;&gt;		<br /><br />
	    </td>
    </tr>
    <tr>
	    <td valign="top" align="center" class="text-pages">
		    Page 1 of 3	</td>
    </tr>--%>
    </table>
</div>

</asp:Content>
