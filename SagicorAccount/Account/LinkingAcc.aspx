<%@ Page Title="Link Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LinkingAcc.aspx.cs" Inherits="SagicorAccount.Account.LinkingAcc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Link Your FLOW Account</h2>
    
    <asp:Label ID="lblSelectBankAccount" runat="server" Text="Select Your Bank Account:" AssociatedControlID="ddlBankAccounts" />
    <asp:DropDownList ID="ddlBankAccounts" runat="server" AutoPostBack="true">
        <asp:ListItem Text="Select Bank Account" Value="" />
    </asp:DropDownList>
    
    <br /> <br />
    
    <asp:Label ID="lblFlowAccountNumber" runat="server" Text="Enter Your FLOW Account Number:" AssociatedControlID="txtFlowAccountNumber" />
    <asp:TextBox ID="txtFlowAccountNumber" runat="server" />
    
    <br /> <br />
    
    <asp:Button ID="btnLinkAccount" runat="server" Text="Link Account" OnClick="btnLinkAccount_Click" />
    
    <br /> <br />
    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" />

    <br /><br />

    <asp:Literal ID="litLinkedAccountInfo" runat="server"></asp:Literal>

</asp:Content>
