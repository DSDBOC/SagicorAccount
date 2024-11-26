<%@ Page Title="Make Payments" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MakingPayments.aspx.cs" Inherits="SagicorAccount.Account.MakingPayments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Make a Payment</h2>
    <div>
        <!-- Message Label -->
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <div>
        <fieldset>
            <legend>Payment Details</legend>
            
            <!-- Dropdown for Bank Accounts -->
            <div>
                <label for="ddlBankAccounts">Select Bank Account:</label>
                <asp:DropDownList ID="ddlBankAccounts" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <br />

            <!-- Dropdown for Linked Accounts -->
            <div>
                <label for="ddlLinkedAccounts">Select Linked Account (FLOW/SAGICOR):</label>
                <asp:DropDownList ID="ddlLinkedAccounts" runat="server" CssClass="form-control"></asp:DropDownList>
            </div>
            <br />

            <!-- Payment Amount Input -->
            <div>
                <label for="txtAmount">Payment Amount:</label>
                <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvAmount" runat="server" ControlToValidate="txtAmount"
                    ErrorMessage="Amount is required." ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="txtAmount"
                    ErrorMessage="Please enter a valid amount (e.g., 1000.00)." ForeColor="Red"
                    ValidationExpression="^\d+(\.\d{1,2})?$"></asp:RegularExpressionValidator>
            </div>
            <br />

            <!-- Payment Button -->
            <div>
                <asp:Button ID="btnMakePayment" runat="server" Text="Make Payment" CssClass="btn btn-primary"
                    OnClick="btnMakePayment_Click" />
            </div>
        </fieldset>
    </div>
</asp:Content>
