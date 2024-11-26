using System;
using System.Data.SqlClient;
using System.Web.UI;
using SagicorAccount.Account;

namespace SagicorAccount.Account
{
    public partial class MakePayments : System.Web.UI.Page
    {
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

                        // Here, you can set the selected account directly if only one account is linked
                        lblBankAccount.Text = "Bank Account: " + displayName; // or display it in a label

                        // You could also set the account number in a hidden field for use later if needed
                        hfBankAccountNumber.Value = accountNumber; // hidden field to store the account number
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
            string flowAccountNumber = txtFlowAccountNumber.Text;
            decimal paymentAmount;

            // Validate payment details
            if (string.IsNullOrEmpty(flowAccountNumber) || !decimal.TryParse(txtPaymentAmount.Text, out paymentAmount))
            {
                lblPaymentStatus.Text = "Please provide valid details for the payment.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            string userID = Session["UserID"] as string;
            string bankAccountNumber = hfBankAccountNumber.Value;

            if (string.IsNullOrEmpty(bankAccountNumber))
            {
                lblPaymentStatus.Text = "No bank account linked to the user.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Call the PaymentService to process the payment
            PaymentService paymentService = new PaymentService();
            string paymentResult = paymentService.ProcessPayment(userID, flowAccountNumber, paymentAmount);

            lblPaymentStatus.Text = paymentResult;

            // Display success or error class based on the result
            lblPaymentStatus.CssClass = paymentResult.Contains("successful") ? "text-success" : "text-danger";
        }
    }
}
