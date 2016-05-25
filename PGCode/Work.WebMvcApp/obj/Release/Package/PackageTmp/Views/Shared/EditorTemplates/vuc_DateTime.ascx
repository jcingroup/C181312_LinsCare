<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%: Html.TextBox("", String.Format("{0:yyyy-MM-dd}", ViewData.TemplateInfo.FormattedModelValue), 
new { @style = "width:120px" ,@id = ViewData.TemplateInfo.HtmlFieldPrefix,@type="date"})%>