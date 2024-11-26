<%@ Page Title="User Profile" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SagicorAccount.Account.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><strong>User Profile</strong></h2><br />

    <!-- User Information Section -->
    <div style="padding: 15px;  border-radius: 8px; border: 1px solid #ddd; max-width: 500px; ">
        <div style="margin-bottom: 10px;">
            <strong> User ID: </strong>
            <asp:Label ID="lblUserID" runat="server" Text="User ID: "></asp:Label>
        </div>

        <div style="margin-bottom: 10px;">
            <strong>Name: </strong>
            <asp:Label ID="lblFullName" runat="server" Text="Name: "></asp:Label>
        </div>

        <div style="margin-bottom: 10px;">
            <strong>Username: </strong>
            <asp:Label ID="lblUserName" runat="server" Text="Username: "></asp:Label>
        </div>

        <div style="margin-bottom: 10px;">
            <strong>Email: </strong>
            <asp:Label ID="lblEmail" runat="server" Text="Email: "></asp:Label>
        </div>

        
    </div><br /><br />
</asp:Content>
