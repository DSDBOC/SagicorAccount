<%@ Page Title="Make Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakePayments.aspx.cs" Inherits="SagicorAccount.Account.MakePayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Make a Payment</h2>

    <!-- Display Linked Bank Account -->
    <div class="form-group">
        <label for="lblBankAccount">Bank Account:</label>
        <asp:Label ID="lblBankAccount" runat="server" CssClass="form-control" Text="No linked account found" />
    </div>

    <!-- Input for Bank Account Number -->
    <div class="form-group">
        <label for="txtBankAccountNumber">Bank Account Number:</label>
        <asp:TextBox ID="txtBankAccountNumber" runat="server" CssClass="form-control" 
                     TextMode="SingleLine" Placeholder="Enter Bank Account Number" />
        <asp:RequiredFieldValidator 
            ID="rfvBankAccountNumber" 
            runat="server" 
            ControlToValidate="txtBankAccountNumber" 
            ErrorMessage="Bank Account Number is required." 
            ForeColor="Red" />
    </div>

    <!-- Input for FLOW Account Number -->
    <div class="form-group">
        <label for="txtFlowAccountNumber">FLOW Account Number:</label>
        <asp:TextBox ID="txtFlowAccountNumber" runat="server" CssClass="form-control" 
                     TextMode="SingleLine" Placeholder="Enter FLOW Account Number" />
        <asp:RequiredFieldValidator 
            ID="rfvFlowAccountNumber" 
            runat="server" 
            ControlToValidate="txtFlowAccountNumber" 
            ErrorMessage="FLOW Account Number is required." 
            ForeColor="Red" />
    </div>

    <!-- Input for Payment Amount -->
    <div class="form-group">
        <label for="txtPaymentAmount">Payment Amount:</label>
        <asp:TextBox ID="txtPaymentAmount" runat="server" CssClass="form-control" />
        <asp:RequiredFieldValidator 
            ID="rfvPaymentAmount" 
            runat="server" 
            ControlToValidate="txtPaymentAmount" 
            ErrorMessage="Payment Amount is required." 
            ForeColor="Red" />
        <asp:RangeValidator 
            ID="rvPaymentAmount" 
            runat="server" 
            ControlToValidate="txtPaymentAmount" 
            MinimumValue="1" 
            MaximumValue="10000" 
            Type="Integer" 
            ErrorMessage="Payment Amount must be between 1 and 10,000." 
            ForeColor="Red" />
    </div>

    <!-- Submit Button for Payment -->
    <div class="form-group">
        <asp:Button ID="btnSubmitPayment" runat="server" Text="Submit Payment" CssClass="btn btn-primary" OnClick="btnSubmitPayment_Click" />
    </div>

    <!-- Payment Status Message -->
    <div class="alert" role="alert">
        <asp:Label ID="lblPaymentStatus" runat="server" CssClass="text-info" />
    </div>

    <!-- Hidden Field to Store Bank Account Number -->
    <asp:HiddenField ID="hfBankAccountNumber" runat="server" />
</asp:Content>
