<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<label id="<%:ViewData.TemplateInfo.HtmlFieldPrefix %>"><%:ViewData.TemplateInfo.FormattedModelValue %></label>
<input type="hidden" name="<%:ViewData.TemplateInfo.HtmlFieldPrefix %>" value="<%:ViewData.TemplateInfo.FormattedModelValue %>" />