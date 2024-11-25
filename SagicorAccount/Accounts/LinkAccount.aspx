<%@ Page Title="Link Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkAccount.aspx.cs"  Inherits="SagicorAccount.Account.LinkAccount" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Link Your Account</h3>

    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>

    <div class="link-account-section">
        <p>Link Your FLOW/SAGICOR LIFE Account to Your Online Banking Profile</p>

        <!-- Platform Selection -->
        <div>
            <asp:Label ID="lblPlatform" runat="server" Text="Select Platform: " AssociatedControlID="rblPlatform"></asp:Label>
            <asp:RadioButtonList ID="rblPlatform" runat="server" RepeatDirection="Vertical">
                <asp:ListItem Text="FLOW" Value="FLOW" />
                <asp:ListItem Text="SAGICOR LIFE" Value="SAGICOR" />
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvPlatform" runat="server" ControlToValidate="rblPlatform"
                ErrorMessage="Please select a platform." ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
        <br />

        <!-- Account Number Input -->
        <div>
            <asp:Label ID="lblAccountNumber" runat="server" Text="Enter your Account Number: " AssociatedControlID="txtAccountNumber"></asp:Label>
            <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="account-input" />
            <asp:RequiredFieldValidator ID="rfvAccountNumber" runat="server" ControlToValidate="txtAccountNumber"
                ErrorMessage="Account number is required." ForeColor="Red"></asp:RequiredFieldValidator>
        </div>
        <br />

        <!-- Link Account Button -->
        <div>
            <asp:Button ID="btnLinkAccount" runat="server" Text="Link Account" OnClick="btnLinkAccount_Click" CssClass="btn-primary" />
        </div>
    </div>
    <br /><br />
</asp:Content>
