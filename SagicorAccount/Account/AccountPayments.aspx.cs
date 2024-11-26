using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class AccountPayments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Ensure the user is logged in before processing payment
            if (Session["UserID"] == null)
            {
                lblPaymentStatus.Text = "You must be logged in to make a payment.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Load the user's linked bank accounts on first load
            if (!IsPostBack)
            {
                LoadLinkedAccounts();
            }
        }

        // Method to load the linked accounts for the logged-in user into the DropDownList
        private void LoadLinkedAccounts()
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                lblPaymentStatus.Text = "User not logged in.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Query to fetch the linked bank accounts for the logged-in user
            string query = "SELECT ba.AccountID, ba.DisplayName FROM BankAccounts ba " +
                           "INNER JOIN LinkedAccounts la ON ba.AccountID = la.BankAccountID " +
                           "WHERE la.UserID = @UserID";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        // Clear existing items
                        ddlLinkedAccounts.Items.Clear();

                        // Add default item
                        ddlLinkedAccounts.Items.Add(new ListItem("Select a bank account", ""));

                        // Loop through the linked accounts and add them to the dropdown
                        while (reader.Read())
                        {
                            string accountID = reader["AccountID"].ToString();
                            string displayName = reader["DisplayName"].ToString();

                            // Create a ListItem and add to DropDownList
                            ddlLinkedAccounts.Items.Add(new ListItem(displayName, accountID));
                        }
                    }
                    else
                    {
                        lblPaymentStatus.Text = "No linked bank accounts found.";
                        lblPaymentStatus.CssClass = "text-danger";
                    }
                }
            }
        }

        // Method to handle payment submission
        protected void btnSubmitPayment_Click(object sender, EventArgs e)
        {
            // Get the selected bank account ID and the entered payment details
            string selectedAccountID = ddlLinkedAccounts.SelectedValue;
            string flowAccountNumber = txtFlowAccountNumber.Text.Trim();
            decimal paymentAmount;

            // Validate the payment amount
            if (decimal.TryParse(txtPaymentAmount.Text.Trim(), out paymentAmount) && paymentAmount > 0)
            {
                // Ensure an account is selected
                if (string.IsNullOrEmpty(selectedAccountID))
                {
                    lblPaymentStatus.Text = "Please select a linked bank account.";
                    lblPaymentStatus.CssClass = "text-danger";
                    return;
                }

                // Validate the Flow account number
                if (string.IsNullOrEmpty(flowAccountNumber))
                {
                    lblPaymentStatus.Text = "Please enter a Flow account number.";
                    lblPaymentStatus.CssClass = "text-danger";
                    return;
                }

                try
                {
                    // Process the payment here (e.g., update the BankTransactions table, deduct from the account balance, etc.)
                    // For now, we assume the payment processing is done successfully

                    // Simulating successful payment (you would call your payment service here)
                    lblPaymentStatus.Text = "Payment of " + paymentAmount.ToString("C") + " was successful!";
                    lblPaymentStatus.CssClass = "text-success";
                    lblPaymentStatus.Visible = true;
                }
                catch (Exception ex)
                {
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
