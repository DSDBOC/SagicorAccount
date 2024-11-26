<%@ Page Title="Manage Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Manage.aspx.cs" Inherits="SagicorAccount.Account.Manage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2 class="mb-4">Manage Your Account</h2>

        <!-- Linked Accounts Section -->
        <section class="mb-5">
            <h3>Your Linked Accounts</h3>
            <asp:GridView ID="gvLinkedAccounts" runat="server" CssClass="table table-bordered text-center" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="LinkID" HeaderText="Link ID" />
                    <asp:BoundField DataField="BankAccountID" HeaderText="Bank Account ID" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblLinkedAccountsStatus" runat="server" CssClass="text-info d-block mt-3"></asp:Label>
        </section>

        <!-- Bank Accounts Section -->
        <section class="mb-5">
            <h3>Your Bank Accounts</h3>
            <asp:GridView ID="gvBankAccounts" runat="server" CssClass="table table-bordered text-center" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="AccountID" HeaderText="Account ID" />
                    <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" />
                    <asp:BoundField DataField="Balance" HeaderText="Balance" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblBankAccountsStatus" runat="server" CssClass="text-info d-block mt-3"></asp:Label>
        </section>

        <!-- Transactions Section -->
        <section class="mb-5">
            <h3>Your Recent Transactions</h3>
            <asp:GridView ID="gvTransactions" runat="server" CssClass="table table-bordered text-center" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" DataFormatString="{0:C}" />
                    <asp:BoundField DataField="TransactionType" HeaderText="Type" />
                    <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
                    <asp:BoundField DataField="Narrative" HeaderText="Narrative" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="lblTransactionsStatus" runat="server" CssClass="text-info d-block mt-3"></asp:Label>
        </section>

        <!-- Error Section -->
        <asp:Label ID="lblError" runat="server" CssClass="text-danger d-block mt-4"></asp:Label>
    </div>
</asp:Content>
