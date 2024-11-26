using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class AddBankAccount : Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SuccessMessage.Visible = false;
                ErrorMessage.Visible = false;
            }
        }

        protected void AddAccountButton_Click(object sender, EventArgs e)
        {
            string accountType = AccountType.SelectedValue;
            decimal initialBalance;
            string displayName = DisplayName.Text; // Get the display name from the textbox

            // Validate the inputs
            if (string.IsNullOrEmpty(accountType) || !decimal.TryParse(InitialBalance.Text, out initialBalance) || initialBalance < 0 || string.IsNullOrEmpty(displayName))
            {
                ErrorMessage.Text = "Please select a valid account type, enter a non-negative balance, and provide a display name.";
                ErrorMessage.Visible = true;
                return;
            }

            try
            {
                // Fetch logged-in user ID
                string userID = Session["UserID"] as string;
                if (string.IsNullOrEmpty(userID))
                {
                    ErrorMessage.Text = "Session expired. Please log in again.";
                    ErrorMessage.Visible = true;
                    return;
                }

                // Insert the bank account into the database
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO BankAccounts (UserID, AccountType, Balance, DisplayName)
                        VALUES (@UserID, @AccountType, @Balance, @DisplayName)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@AccountType", accountType);
                        command.Parameters.AddWithValue("@Balance", initialBalance);
                        command.Parameters.AddWithValue("@DisplayName", displayName);  // Add DisplayName to the query

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            SuccessMessage.Text = "Bank account added successfully.";
                            SuccessMessage.Visible = true;
                            DisplayName.Text = string.Empty; // Clear the DisplayName textbox
                            InitialBalance.Text = string.Empty; // Clear the InitialBalance textbox
                            AccountType.SelectedIndex = 0; // Reset AccountType dropdown to default
                        }
                        else
                        {
                            ErrorMessage.Text = "Failed to add the bank account. Please try again.";
                            ErrorMessage.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage.Text = "An error occurred: " + ex.Message;
                ErrorMessage.Visible = true;
            }
        }
    }
}
