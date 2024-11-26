<%@ Page Title="Contact Us" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SagicorAccount.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="contact-page" aria-labelledby="title">
          <link rel="stylesheet" type="text/css" href="StyleSheets/Contact.css" />
        <header>
            <h1 id="title">Contact Us</h1>
            <h2>We are here to assist you</h2>
        </header>

        <!-- Support Information Section -->
        <section class="support-info">
            <h3>Support</h3>
            <p>If you need assistance with your account, please reach out to us through one of the following methods:</p>
            <address>
                <strong>Customer Support:</strong>
                <a href="mailto:support@example.com">support@example.com</a><br />
                <strong>Account Inquiries:</strong>
                <a href="mailto:accounts@example.com">accounts@example.com</a><br />
                <strong>Technical Support:</strong>
                <a href="mailto:techsupport@example.com">techsupport@example.com</a><br />
            </address>
        </section>

        <!-- Contact Form Section -->
        <section class="contact-form">
            <h3>Contact Form</h3>
            <p>Alternatively, you can send us a message using the form below, and one of our representatives will get back to you promptly.</p>

            <form method="post" action="SubmitContactForm.aspx">
                <div class="form-group">
                    <label for="name">Full Name:</label>
                    <input type="text" id="name" name="name" required class="form-control" />
                </div>

                <div class="form-group">
                    <label for="email">Email Address:</label>
                    <input type="email" id="email" name="email" required class="form-control" />
                </div>

                <div class="form-group">
                    <label for="message">Message:</label><br />
                    <textarea id="message" name="message" rows="5" required class="form-control"></textarea>
                </div>

                <button type="submit" class="btn btn-primary">Send Message</button>
            </form>
        </section>

        </main>

    </asp:Content>