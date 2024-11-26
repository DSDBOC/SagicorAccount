<%@ Page Title="Make Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakePayments.aspx.cs" Inherits="SagicorAccount.Account.MakePayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Make a Payment</h2>

    <div class="form-group">
        <label for="lblBankAccount">Bank Account:</label>
        <asp:Label ID="lblBankAccount" runat="server" CssClass="form-control"></asp:Label>
    </div>

    <div class="form-group">
        <label for="txtFlowAccountNumber">FLOW Account Number:</label>
        <asp:TextBox ID="txtFlowAccountNumber" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <label for="txtPaymentAmount">Payment Amount:</label>
        <asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="form-control"></asp:TextBox>
    </div>

    <div class="form-group">
        <asp:Button ID="btnSubmitPayment" runat="server" Text="Submit Payment" CssClass="btn btn-primary" OnClick="btnSubmitPayment_Click" />
    </div>

    <div class="alert" role="alert">
        <asp:Label ID="lblPaymentStatus" runat="server" CssClass="text-info"></asp:Label>
    </div>

    <asp:HiddenField ID="hfBankAccountNumber" runat="server" />
</asp:Content>
