<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SagicorAccount.Account.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h2>Welcome to Your Dashboard</h2>
        <hr />
        
        <!-- Display User's Full Name and User ID -->
        <div>
            <asp:Label ID="lblUserID" runat="server" Text="User ID:"></asp:Label><br />
            <asp:Label ID="lblFullName" runat="server" Text="Name:"></asp:Label>
        </div>

        <!-- Additional user dashboard content goes here -->
    </div>
</asp:Content>
