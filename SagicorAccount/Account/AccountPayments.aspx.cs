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
                lblPaymentStatus.Visible = true;
                return;
            }

            // Load the user's linked bank accounts on first load
            if (!IsPostBack)
            {
                LoadLinkedAccounts();
                PopulateFlowAccountDropdown();
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
                lblPaymentStatus.Visible = true;
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
                        lblPaymentStatus.Visible = true;
                    }
                }
            }
        }



        // Method to handle payment submission
        protected void btnSubmitPayment_Click(object sender, EventArgs e)
        {
            // Get the selected bank account ID and the entered payment details
            string selectedAccountID = ddlLinkedAccounts.SelectedValue;
            string flowAccountNumber = ddlFlowAccountNumber.SelectedItem.Text;
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
                    // Database operation: insert transaction into BankTransactions
                    string query = "INSERT INTO BankTransactions (UserID, BankAccountID, Amount, TransactionType, Date, Narrative) " +
                                   "VALUES (@UserID, @BankAccountID, @Amount, @TransactionType, @Date, @Narrative)";

                    // Prepare connection
                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@UserID", Session["UserID"].ToString());
                            cmd.Parameters.AddWithValue("@BankAccountID", selectedAccountID);
                            cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                            cmd.Parameters.AddWithValue("@TransactionType", "Payment");
                            cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                            cmd.Parameters.AddWithValue("@Narrative", "Payment to Flow Account: " + flowAccountNumber);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Deduct the amount from the bank account balance
                    string updateBalanceQuery = "UPDATE BankAccounts SET Balance = Balance - @Amount WHERE AccountID = @BankAccountID";

                    using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(updateBalanceQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                            cmd.Parameters.AddWithValue("@BankAccountID", selectedAccountID);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Simulate successful payment
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

        private void PopulateFlowAccountDropdown()
        {
            string userId = Session["UserID"]?.ToString(); // Assuming the user ID is stored in session

            if (!string.IsNullOrEmpty(userId))
            {
                // Query to fetch the linked FlowAccountNumbers for the logged-in user
                string query = "SELECT LinkID, FlowAccountNumber FROM LinkedAccounts WHERE UserID = @UserID";

                // Use a connection string from your web.config
                using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);

                        try
                        {
                            conn.Open();
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                // Clear existing items in the DropDownList
                                ddlFlowAccountNumber.Items.Clear();
                                ddlFlowAccountNumber.Items.Add(new ListItem("Select Flow Account", ""));

                                // Populate the dropdown with linked Flow Account Numbers
                                while (reader.Read())
                                {
                                    string flowAccountNumber = reader["FlowAccountNumber"].ToString();
                                    string linkId = reader["LinkID"].ToString(); // You can store LinkID in the Value field

                                    ddlFlowAccountNumber.Items.Add(new ListItem(flowAccountNumber, linkId));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle any errors here
                            lblPaymentStatus.Text = "Error retrieving linked accounts: " + ex.Message;
                            lblPaymentStatus.CssClass = "text-danger";
                            lblPaymentStatus.Visible = true;
                        }
                    }
                }
            }
        }
    }

    }
