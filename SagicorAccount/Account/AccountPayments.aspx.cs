using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;

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

        

        protected void gvTransactions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Check if the row is a data row
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Example: Change the background color of rows based on the transaction type
                string transactionType = DataBinder.Eval(e.Row.DataItem, "TransactionType").ToString();
                if (transactionType == "Payment")
                {
                    e.Row.BackColor = System.Drawing.Color.LightGreen;
                }
                else if (transactionType == "Withdrawal")
                {
                    e.Row.BackColor = System.Drawing.Color.LightCoral;
                }
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

            // Query to fetch the linked bank accounts for the logged-in user, showing AccountID and AccountNumber
            string query = "SELECT ba.AccountID, ba.AccountNumber FROM BankAccounts ba " +
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
                        // Clear existing items in DropDownList
                        ddlLinkedAccounts.Items.Clear();

                        // Add default item
                        ddlLinkedAccounts.Items.Add(new ListItem("Select a bank account", ""));

                        // Loop through the linked accounts and add them to the DropDownList
                        while (reader.Read())
                        {
                            string accountID = reader["AccountID"].ToString();
                            string accountNumber = reader["AccountNumber"].ToString();

                            // Create a ListItem with AccountNumber as Text and AccountID as Value
                            ddlLinkedAccounts.Items.Add(new ListItem(accountNumber, accountID));
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
            // Get the logged-in user ID
            string userID = Session["UserID"] as string;
            if (string.IsNullOrEmpty(userID))
            {
                lblPaymentStatus.Text = "Error: User not logged in.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Parse user inputs
            int bankAccountID = int.Parse(ddlLinkedAccounts.SelectedValue);  // Selected Bank Account ID from dropdown
            decimal paymentAmount = decimal.Parse(txtPaymentAmount.Text);    // Payment amount entered by user

            // Ensure that the selected LinkedAccount is valid
            int linkedAccountID = GetLinkedAccountID(userID, bankAccountID);
            if (linkedAccountID == 0)
            {
                lblPaymentStatus.Text = "Error: No linked account found for the selected bank account.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Define connection string
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

            // Use transaction for atomic updates
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction(); // Start transaction

                try
                {
                    // Update BankAccount (Deduct from user account)
                    string updateBankAccountQuery = "UPDATE BankAccounts SET Balance = Balance - @Amount WHERE AccountID = @BankAccountID AND UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(updateBankAccountQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                        cmd.Parameters.AddWithValue("@BankAccountID", bankAccountID);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            throw new Exception("Failed to update BankAccount balance.");
                    }

                    // Update LinkedAccount (Add to linked account balance)
                    string updateLinkedAccountQuery = "UPDATE LinkedAccounts SET Balance = Balance + @Amount WHERE LinkID = @LinkedAccountID AND BankAccountID = @BankAccountID AND UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(updateLinkedAccountQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                        cmd.Parameters.AddWithValue("@LinkedAccountID", linkedAccountID);
                        cmd.Parameters.AddWithValue("@BankAccountID", bankAccountID);
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                            throw new Exception("Failed to update LinkedAccount balance.");
                    }

                    // Insert the transaction into the BankTransactions table
                    string insertTransactionQuery = "INSERT INTO BankTransactions (UserID, BankAccountID, Amount, TransactionType, Date, Narrative) " +
                                                    "VALUES (@UserID, @BankAccountID, @Amount, @TransactionType, @Date, @Narrative)";
                    using (SqlCommand cmd = new SqlCommand(insertTransactionQuery, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@BankAccountID", bankAccountID);
                        cmd.Parameters.AddWithValue("@Amount", paymentAmount);
                        cmd.Parameters.AddWithValue("@TransactionType", "Payment");  // Assuming this is a payment
                        cmd.Parameters.AddWithValue("@Date", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Narrative", "Payment to FLOW account");  // You can customize the narrative
                        cmd.ExecuteNonQuery();
                    }

                    // Commit transaction
                    transaction.Commit();

                    // Display success message
                    lblPaymentStatus.Text = "Payment successful!";
                    lblPaymentStatus.CssClass = "text-success";

                    // Redirect to prevent resubmission
                    Response.Redirect(Request.Url.ToString(), false);
                }
                catch (Exception ex)
                {
                    // Rollback transaction on error
                    transaction.Rollback();

                    // Display error message
                    lblPaymentStatus.Text = "Error: " + ex.Message;
                    lblPaymentStatus.CssClass = "text-danger";
                }
            }
        }







        // Method to retrieve the LinkedAccountID based on the UserID and BankAccountID
        private int GetLinkedAccountID(string userID, int bankAccountID)
        {
            int linkedAccountID = 0;

            string query = "SELECT LinkID FROM LinkedAccounts WHERE UserID = @UserID AND BankAccountID = @BankAccountID";

            using (SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@BankAccountID", bankAccountID);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        linkedAccountID = Convert.ToInt32(result);
                    }
                }
            }

            return linkedAccountID;
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
