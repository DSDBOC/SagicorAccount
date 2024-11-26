<%@ Page Title="Add Bank Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddBankAccount.aspx.cs" Inherits="SagicorAccount.Account.AddBankAccount" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <link rel="stylesheet" type="text/css" href="../StyleSheets/AddBankAccount.css" />
    <h2>Add Bank Account</h2><hr />
    <asp:Label ID="SuccessMessage" runat="server" ForeColor="Green" Visible="false" Text="Bank account added successfully."></asp:Label>
    <asp:Label ID="ErrorMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

    <asp:Panel ID="AddBankAccountPanel" runat="server">
        <div class="form-group">
            <label for="AccountType">Account Type:</label>
            <asp:DropDownList ID="AccountType" runat="server" CssClass="form-control">
                <asp:ListItem Text="Select Account Type" Value="" />
                <asp:ListItem Text="Savings" Value="Savings" />
                <asp:ListItem Text="Checking" Value="Checking" />
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <label for="InitialBalance">Initial Balance:</label>
            <asp:TextBox ID="InitialBalance" runat="server" CssClass="form-control" TextMode="Number" Placeholder="Enter initial balance"></asp:TextBox>
        </div>
        <div class="form-group">
            <label for="DisplayName">Display Name:</label>
            <asp:TextBox ID="DisplayName" runat="server" CssClass="form-control" Placeholder="Enter display name"></asp:TextBox><br />
        </div>
        <asp:Button ID="AddAccountButton" runat="server" Text="Add Account" CssClass="btn btn-primary" OnClick="AddAccountButton_Click" />
    </asp:Panel>
</asp:Content>

