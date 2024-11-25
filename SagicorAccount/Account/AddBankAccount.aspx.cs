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
            if (string.IsNullOrEmpty(accountType) || !decimal.TryParse(InitialBalance.Text, out initialBalance) || initialBalance < 0)
            {
                ErrorMessage.Text = "Please select a valid account type and enter a non-negative balance.";
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
                        INSERT INTO BankAccounts (UserID, AccountType, Balance)
                        VALUES (@UserID, @AccountType, @Balance)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@AccountType", accountType);
                        command.Parameters.AddWithValue("@Balance", initialBalance);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            SuccessMessage.Text = "Bank account added successfully.";
                            SuccessMessage.Visible = true;
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
