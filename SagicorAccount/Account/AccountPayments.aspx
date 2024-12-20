﻿<%@ Page Title="Make Payment" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AccountPayments.aspx.cs" Inherits="SagicorAccount.Account.AccountPayments" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="../StyleSheets/MakePayment.css" />

    <div class="container mt-4">
        <h2>Make a Payment</h2>
        
        <!-- Status Message -->
        <asp:Label ID="lblPaymentStatus" runat="server" CssClass="text-center" Visible="false" />

        <!-- Payment Form Panel -->
        <asp:Panel ID="pnlPaymentForm" runat="server">

            <div class="form-group">
                <label for="ddlLinkedAccounts">Select Linked Bank Account</label>
                <asp:DropDownList ID="ddlLinkedAccounts" runat="server" class="form-control">
                    <asp:ListItem Text="Select a bank account" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="ddlFlowAccountNumber">Select Flow Account Number</label>
                <asp:DropDownList ID="ddlFlowAccountNumber" runat="server" class="form-control">
                    <asp:ListItem Text="Select Flow Account" Value="" />
                </asp:DropDownList>
            </div>

            <div class="form-group">
                <label for="txtPaymentAmount">Payment Amount</label>
                <asp:TextBox ID="txtPaymentAmount" runat="server" class="form-control" placeholder="Enter Payment Amount" />
            </div>

            <div class="form-group mt-3">
                <asp:Button ID="btnSubmitPayment" runat="server" Text="Submit Payment" CssClass="btn btn-primary" OnClick="btnSubmitPayment_Click" />
            </div>
        </asp:Panel>

    </div>
</asp:Content>
