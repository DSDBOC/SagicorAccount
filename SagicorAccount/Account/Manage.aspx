<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="SagicorAccount.Account.Manage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Manage Your Account</h2>

        <!-- Linked Accounts Section -->
        <h3>Your Linked Accounts</h3>
        <asp:GridView ID="gvLinkedAccounts" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="LinkID" HeaderText="Link ID" />
                <asp:BoundField DataField="BankAccountID" HeaderText="Bank Account ID" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>

        <!-- Bank Accounts Section -->
        <h3>Your Bank Accounts</h3>
        <asp:GridView ID="gvBankAccounts" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="AccountID" HeaderText="Account ID" />
                <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:C}" />
            </Columns>
        </asp:GridView>

        <!-- Transactions Section -->
        <h3>Your Recent Transactions</h3>
        <asp:GridView ID="gvTransactions" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False">
            <Columns>
                <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" />
                <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                <asp:BoundField DataField="TransactionType" HeaderText="Type" />
                <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                <asp:BoundField DataField="Narrative" HeaderText="Narrative" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
