<%@ Page Title="Bank Account Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewBankAccountDetails.aspx.cs"  Inherits="SagicorAccount.Account.ViewBankAccountDetails" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Bank Account Details</h2>
    
    <!-- Success/Failure Message -->
    <asp:Label ID="lblMessage" runat="server" Visible="true"></asp:Label>

    <!-- Dropdown for account type -->
    <div class="form-group">
        <label for="ddlAccountType">Account Type:</label>
        <asp:DropDownList ID="ddlAccountType" runat="server" CssClass="form-control">
            <asp:ListItem Text="Select Account Type" Value="" />
            <asp:ListItem Text="Savings" Value="Savings" />
            <asp:ListItem Text="Checking" Value="Checking" />
        </asp:DropDownList>
    </div>

    <!-- Textbox for balance -->
    <div class="form-group">
        <label for="txtBalance">Initial Balance:</label>
        <asp:TextBox ID="txtBalance" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Enter initial balance"></asp:TextBox>
    </div>

    <!-- Add Account Button -->
    <asp:Button ID="btnAddAccount" runat="server" Text="Add Account" CssClass="btn btn-primary" OnClick="btnAddAccount_Click" />

    <!-- Display existing accounts -->
    <h3>Existing Accounts</h3>
    <asp:GridView ID="gvBankAccounts" runat="server" AutoGenerateColumns="False" CssClass="table">
        <Columns>
            <asp:BoundField DataField="AccountID" HeaderText="Account ID" />
            <asp:BoundField DataField="AccountType" HeaderText="Account Type" />
            <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" />
            <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:C}" />
        </Columns>
    </asp:GridView>
</asp:Content>
