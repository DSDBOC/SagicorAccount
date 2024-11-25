<%@ Page Title="Make Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakePayments.aspx.cs" Inherits="SagicorAccount.Account.MakePayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Make a Payment</h2>

    <div class="form-group">
        <label for="ddlLinkedAccounts">Select Linked Account:</label>
        <asp:DropDownList ID="ddlLinkedAccounts" runat="server" CssClass="form-control"></asp:DropDownList>
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
</asp:Content>
