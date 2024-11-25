<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserDashboard.aspx.cs" Inherits="SagicorAccount.Account.UserDashboard" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>User Dashboard - Sagicor Account</title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" />
    <link href="~/Content/site.css" rel="stylesheet" />
</head>
<body>
    <div class="container mt-5">
        <h2>Welcome to Your Dashboard</h2>

        <asp:Label ID="FailureText" runat="server" ForeColor="Red" Visible="false" CssClass="alert alert-danger"></asp:Label>

        <h4>Your Bank Accounts</h4>

        <!-- Bank Accounts GridView -->
        <asp:GridView ID="gvBankAccounts" runat="server" AutoGenerateColumns="True" CssClass="table table-striped table-bordered" EmptyDataText="You have no bank accounts." OnRowCommand="gvBankAccounts_RowCommand">
            <Columns>
                <asp:BoundField DataField="AccountID" HeaderText="Account ID" SortExpression="AccountID" />
                <asp:BoundField DataField="AccountType" HeaderText="Account Type" SortExpression="AccountType" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance" DataFormatString="{0:C}" />
                <asp:BoundField DataField="AccountNumber" HeaderText="Account Number" SortExpression="AccountNumber" />
                <asp:ButtonField ButtonType="Button" CommandName="ViewDetails" Text="View Details" />
            </Columns>
        </asp:GridView>

        <br />
        <!-- Log out button -->
        <asp:Button ID="btnLogOut" runat="server" Text="Log Out" CssClass="btn btn-danger" OnClick="LogOut_Click" />

    </div>

    <script src="~/Scripts/jquery-3.6.0.min.js"></script>
    <script src="~/Scripts/bootstrap.bundle.min.js"></script>
</body>
</html>
