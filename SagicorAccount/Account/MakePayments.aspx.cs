using System;
using System.Data.SqlClient;
using System.Web.UI;
using SagicorAccount.Account;

namespace SagicorAccount.Account
{
    public partial class MakePayments : System.Web.UI.Page
    {
        PaymentService paymentService = new PaymentService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Ensure the user is logged in before processing payment
                if (Session["UserID"] == null)
                {
                    lblPaymentStatus.Text = "You must be logged in to make a payment.";
                    lblPaymentStatus.CssClass = "text-danger";
                    return;
                }

                // Load the user's bank account linked to their profile
                LoadUserBankAccount();
            }
        }

        private void LoadUserBankAccount()
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                lblPaymentStatus.Text = "User not logged in.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Query to fetch the linked bank account for the logged-in user
            string query = "SELECT AccountNumber, DisplayName FROM BankAccounts WHERE UserID = @UserID";
            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        // Assuming the user has one bank account
                        string accountNumber = reader["AccountNumber"].ToString();
                        string displayName = reader["DisplayName"].ToString();

                        // Display the bank account info
                        lblBankAccount.Text = "Bank Account: " + displayName;
                        hfBankAccountNumber.Value = accountNumber; // Store the account number in the hidden field
                    }
                    else
                    {
                        lblPaymentStatus.Text = "No linked bank account found for this user.";
                        lblPaymentStatus.CssClass = "text-danger";
                    }
                }
            }
        }

        protected void btnSubmitPayment_Click(object sender, EventArgs e)
        {
            string bankAccountNumber = txtBankAccountNumber.Text.Trim();  // Get the bank account number from user input
            string flowAccountNumber = txtFlowAccountNumber.Text.Trim();  // Get the flow account number
            decimal paymentAmount;

            // Validate the payment amount
            if (decimal.TryParse(txtPaymentAmount.Text.Trim(), out paymentAmount) && paymentAmount > 0)
            {
                // Validate if entered bank account matches the linked bank account
                if (bankAccountNumber != hfBankAccountNumber.Value)
                {
                    lblPaymentStatus.Text = "Bank Account number does not match the linked account.";
                    lblPaymentStatus.CssClass = "text-danger";
                    return;
                }

                try
                {
                    // Query the LinkedAccounts table to validate the FlowAccountNumber
                    string query = "SELECT * FROM LinkedAccounts WHERE FlowAccountNumber = @FlowAccountNumber";
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@FlowAccountNumber", flowAccountNumber);
                            conn.Open();

                            SqlDataReader reader = cmd.ExecuteReader();
                            if (!reader.HasRows)
                            {
                                lblPaymentStatus.Text = "Invalid FLOW account number.";
                                lblPaymentStatus.CssClass = "text-danger";
                                return;
                            }
                        }
                    }

                    // Call the payment processing method after validation
                    string result = paymentService.ProcessPayment(bankAccountNumber, flowAccountNumber, paymentAmount);

                    // Display the result from the payment service
                    lblPaymentStatus.Text = result;
                    lblPaymentStatus.CssClass = result.StartsWith("Success") ? "text-success" : "text-danger";
                    lblPaymentStatus.Visible = true;
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur while calling the payment service
                    lblPaymentStatus.Text = "Payment failed: " + ex.Message;
                    lblPaymentStatus.CssClass = "text-danger";
                    lblPaymentStatus.Visible = true;
                }
            }
            else
            {
                lblPaymentStatus.Text = "Please enter a valid payment amount.";
                lblPaymentStatus.CssClass = "text-danger";
                lblPaymentStatus.Visible = true;
            }
        }

    }

}