<%@ Page Title="Link Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkAccount.aspx.cs" Inherits="SagicorAccount.Account.LinkAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="text-primary">Link Your FLOW Account</h2>
    <p>Enter your FLOW account number below to link it to your profile. Only accounts verified on the FLOW/SAGICOR LIFE platform can be linked.</p>

    <!-- Input Section -->
    <div class="form-group">
        <label for="txtFlowAccountNumber">FLOW Account Number:</label>
        <asp:TextBox 
            ID="txtFlowAccountNumber" 
            runat="server" 
            CssClass="form-control" 
            Placeholder="Enter FLOW Account Number" 
            MaxLength="10"
            aria-describedby="flowAccountHelpText"></asp:TextBox>
        <small id="flowAccountHelpText" class="form-text text-muted">Enter the 10-digit account number for verification.</small>
    </div>

    <div class="form-group">
        <asp:Button 
            ID="btnLinkAccount" 
            runat="server" 
            Text="Link Account" 
            CssClass="btn btn-success" 
            OnClick="btnLinkAccount_Click" 
            aria-label="Link your FLOW account" />
    </div>

    <!-- Message Section -->
    <div id="messageContainer" class="mt-3">
        <asp:Label ID="lblMessage" runat="server" CssClass="alert" role="alert"></asp:Label>
    </div>

    <hr />

    <!-- Account Details Section -->
    <h3 class="text-secondary">Account Details</h3>
    <div id="accountDetails" class="mt-3">
        <div class="form-group">
            <label for="lblName">Name:</label>
            <asp:Label 
                ID="lblName" 
                runat="server" 
                CssClass="form-control" 
                Text="--" 
                aria-live="polite"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblAccountNumber">Account Number:</label>
            <asp:Label 
                ID="lblAccountNumber" 
                runat="server" 
                CssClass="form-control" 
                Text="--"
                aria-live="polite"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblAccountType">Account Type:</label>
            <asp:Label 
                ID="lblAccountType" 
                runat="server" 
                CssClass="form-control" 
                Text="--"
                aria-live="polite"></asp:Label>
        </div>
        <div class="form-group">
            <label for="lblBalance">Balance:</label>
            <asp:Label 
                ID="lblBalance" 
                runat="server" 
                CssClass="form-control" 
                Text="--"
                aria-live="polite"></asp:Label>
        </div>
    </div>
</asp:Content>
