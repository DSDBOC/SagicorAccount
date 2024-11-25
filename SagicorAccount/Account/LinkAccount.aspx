<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkAccount.aspx.cs" Inherits="SagicorAccount.Account.LinkAccount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Link Your FLOW Account</h2>

    <div class="form-group">
        <label for="txtFlowAccount">FLOW Account Number:</label>
        <asp:TextBox ID="txtFlowAccount" runat="server" CssClass="form-control" placeholder="Enter your FLOW account number"></asp:TextBox>
    </div>

    <div class="form-group">
        <label for="ddlBankAccount">Select Bank Account:</label>
        <asp:DropDownList ID="ddlBankAccount" runat="server" CssClass="form-control">
            <asp:ListItem Text="Select a Bank Account" Value="0" />
            
        </asp:DropDownList>
    </div>

    <div class="form-group">
        <asp:Button ID="btnLinkAccount" runat="server" Text="Link Account" CssClass="btn btn-primary" OnClick="btnLinkAccount_Click" />
    </div>

    <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>

</asp:Content>
