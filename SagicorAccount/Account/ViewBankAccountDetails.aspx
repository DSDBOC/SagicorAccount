<%@ Page Title="Bank Account Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewBankAccountDetails.aspx.cs"  Inherits="SagicorAccount.Account.ViewBankAccountDetails" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Bank Account Details</h2>
    
    <!-- Success/Failure Message -->
    <asp:Label ID="lblMessage" runat="server" Visible="true"></asp:Label>

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
