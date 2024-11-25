<%@ Page Title="User Dashboard" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs"  Inherits="SagicorAccount.Account.UserDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main aria-labelledby="title">
        <h2 id="title">User Dashboard</h2>

        <div>
            <h4>Your Account Information</h4>
                        <!-- Display User Info -->
            <div>
                <p><strong>User ID:</strong> <asp:Label ID="lblUserID" runat="server" Text=""></asp:Label></p>
                <p><strong>Email:</strong> <asp:Label ID="lblEmail" runat="server" Text=""></asp:Label></p>
                <p><strong>Username:</strong> <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label></p>
            </div>
            
            <h4>Your Bank Account Details</h4>
            <asp:GridView ID="gvBankAccountss" runat="server" AutoGenerateColumns="True" CssClass="table"></asp:GridView>

        </div>
        <div>
</div>

    </main>
</asp:Content>
