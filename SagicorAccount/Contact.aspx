<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SagicorAccount.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="contact-page" aria-labelledby="title">
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

        <!-- Custom Styles -->
        <style>
            /* Base body styles */
            body {
                background-color: #f4f6f9; /* Light neutral background */
                font-family: 'Roboto', Arial, sans-serif; /* Modern, professional font */
                color: #333; /* Dark text for good readability */
            }

            main {
                max-width: 800px;
                margin: 0 auto;
                padding: 40px;
                background-color: #ffffff;
                border-radius: 8px;
                box-shadow: 0px 6px 12px rgba(0, 0, 0, 0.1);
            }

            h1 {
                text-align: center;
                color: #005b96; /* Bank-friendly color */
                font-size: 32px;
                margin-bottom: 10px;
            }

            h2 {
                text-align: center;
                color: #005b96;
                font-size: 20px;
                margin-bottom: 20px;
            }

            h3 {
                color: #333; /* Dark gray text for headings */
                font-size: 22px;
                margin-bottom: 15px;
            }

            /* Form and button styles */
            .form-group {
                margin-bottom: 20px;
            }

            .form-group label {
                font-weight: bold;
                color: #555;
            }

            .form-control {
                width: 100%;
                padding: 10px;
                border: 1px solid #ccc;
                border-radius: 4px;
                font-size: 14px;
                color: #333;
            }

            .form-control:focus {
                border-color: #005b96;
                box-shadow: 0 0 5px rgba(0, 91, 150, 0.3);
            }

            .btn-primary {
                background-color: #005b96; /* Bank-friendly blue */
                color: white;
                padding: 10px 20px;
                border: none;
                border-radius: 4px;
                font-size: 16px;
                cursor: pointer;
                text-transform: uppercase;
                font-weight: bold;
            }

            .btn-primary:hover {
                background-color: #00457c;
            }

            .btn-primary:focus {
                outline: none;
                box-shadow: 0 0 5px rgba(0, 91, 150, 0.5);
            }

            /* Section and address styles */
            address {
                font-style: normal;
                line-height: 1.6;
                margin-bottom: 20px;
            }

            /* General container styles */
            section {
                margin-bottom: 30px;
            }

            section p {
                font-size: 16px;
                color: #555;
                line-height: 1.5;
            }

            /* Footer styles (if applicable) */
            footer {
                text-align: center;
                margin-top: 50px;
                font-size: 14px;
                color: #999;
            }

        </style>
    </main>
</asp:Content>
