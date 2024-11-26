<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SagicorAccount._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

      <link rel="stylesheet" type="text/css" href="StyleSheets/Default.css" />

    <main>
        <!-- Title Section -->
        <section class="intro-section text-center" aria-labelledby="aspnetTitle">
            <h1 id="aspnetTitle" class="page-title">FlexiPay</h1>
            <p class="lead-text">"Pay Smarter, Live Easier – Your Bills, Your Bank, One Click."</p>
        </section>
        <br>
        <!-- Main Content Section -->
        <div class="row content-sections">
            <section class="col-md-4" aria-labelledby="gettingStartedTitle">
                <h2 id="gettingStartedTitle" class="section-title">Online Bill Payment</h2>
                <p class="section-text">
                   A secure and convenient way to pay your bills online, directly from your bank account to your FLOW accounts.
                </p>
            </section>

            <section class="col-md-4" aria-labelledby="librariesTitle">
                <h2 id="librariesTitle" class="section-title">Log In to Your Account</h2>
                <p class="section-text">
                   Access your online banking profile to securely manage your accounts and payments. Login with your username and password to get started.
                </p>
            </section>

            <section class="col-md-4" aria-labelledby="hostingTitle">
                <h2 id="hostingTitle" class="section-title">Link Account</h2>
                <p class="section-text">
                    Once your account is linked, you can make payments to your FLOW account directly through the online banking platform.
                    Your transaction will be reflected in both your bank account and the linked account.
                </p>
            </section>
        </div>

    </main>

</asp:Content>
