<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountPayments.aspx.cs" Inherits="SagicorAccount.Account.AccountPayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h2>Make a Payment</h2>
        
        <!-- Status Message -->
        <asp:Label ID="lblPaymentStatus" runat="server" CssClass="text-center" Visible="false" />

        <!-- Payment Form Panel -->
        <asp:Panel ID="pnlPaymentForm" runat="server">

            <div class="form-group">
                <label for="ddlLinkedAccounts">Select Linked Bank Account</label>
                <asp:DropDownList ID="ddlLinkedAccounts" runat="server" class="form-control">
                    <asp:ListItem Text="Select a bank account" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="ddlFlowAccountNumber">Select Flow Account Number</label>
                <asp:DropDownList ID="ddlFlowAccountNumber" runat="server" class="form-control">
                    <asp:ListItem Text="Select Flow Account" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtPaymentAmount">Payment Amount</label>
                <asp:TextBox ID="txtPaymentAmount" runat="server" class="form-control" placeholder="Enter Payment Amount" />
            </div>

            <div class="form-group mt-3">
                <asp:Button ID="btnSubmitPayment" runat="server" Text="Submit Payment" CssClass="btn btn-primary" OnClick="btnSubmitPayment_Click" />
            </div>
        </asp:Panel>

        <!-- GridView for Bank Transactions -->
        <div class="mt-5">
            <h3>Your Recent Transactions</h3>
            <asp:GridView ID="gvTransactions" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False" OnRowDataBound="gvTransactions_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="TransactionID" HeaderText="Transaction ID" SortExpression="TransactionID" />
                    <asp:BoundField DataField="Amount" HeaderText="Amount" SortExpression="Amount" />
                    <asp:BoundField DataField="TransactionType" HeaderText="Transaction Type" SortExpression="TransactionType" />
                    <asp:BoundField DataField="Date" HeaderText="Date" SortExpression="Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Narrative" HeaderText="Narrative" SortExpression="Narrative" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
