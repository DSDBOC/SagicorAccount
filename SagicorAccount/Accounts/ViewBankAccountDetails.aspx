<%@ Page Title="Bank Account Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewBankAccountDetails.aspx.cs"  Inherits="SagicorAccount.Account.ViewBankAccountDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div>
<h4>Add New Bank Account</h4>
<asp:DropDownList ID="ddlAccountType" runat="server">
<asp:ListItem Text="Savings" Value="Savings"></asp:ListItem>
<asp:ListItem Text="Checking" Value="Checking"></asp:ListItem>
</asp:DropDownList><br />
<asp:TextBox ID="txtBalance" runat="server" CssClass="form-control" Placeholder="Enter Initial Balance"></asp:TextBox>
<asp:Button ID="btnAddAccount" runat="server" Text="Add Account" CssClass="btn btn-primary" OnClick="btnAddAccount_Click" />
<asp:Label ID="lblMessage" runat="server" CssClass="text-success"></asp:Label>
</div>

</asp:Content>
