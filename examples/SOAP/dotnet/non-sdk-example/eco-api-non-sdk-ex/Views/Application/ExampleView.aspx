<%@ Page Title="" Language="C#" MasterPageFile="~/Views/site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
<h2>e-conomic api non-SDK example</h2>

    <% Html.BeginForm("Connect", "Application",FormMethod.Post); %>
        
    <fieldset>
        <legend>Connect to your agreement</legend><br/>

        <%--<input type="text" name="agreementNo" value="" placeholder="Agreement no."/><br/>
        <input type="text" name="username" value="" placeholder="Username"/><br/>
        <input type="password" name="password" value="" placeholder="Password"/><br/><br/>--%>
        
        <input type="text" name="token" value="" placeholder="Enter your access token here"/><br/>
        <input type="text" name="appToken" value="" placeholder="Enter your appToken here"/><br/><br/>
                
        <input type="submit" name="Connect" value="Connect" />  <%: ViewData["Auth_message"] %><br/>
        <%: Html.ActionLink("Disconnect", "Disconnect") %>
    </fieldset>

    <% Html.EndForm(); %>
    
    <fieldset>
        <legend>Use the API</legend>
        
        <p><%: ViewData["message"] %></p>
        <p style="color: red"><%: ViewData["Error_message"] %></p>

        <%: Html.ActionLink("Create products and productgroups", "CreateProductsAndGroup") %><br />
        <%: Html.ActionLink("Create debtors and debtorgroups", "CreateDeptorsAndGroup") %><br />
        <%: Html.ActionLink("Create order and orderlines", "CreateOrderAndLines")%><br />
        <%: Html.ActionLink("Upgrade current order to invoice", "UpgradeOrder")%><br />

    </fieldset>

    


</asp:Content>
