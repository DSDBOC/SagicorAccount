using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class ViewBankAccountDetails : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["SagicorLifeConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Load existing bank accounts for the logged-in user
                LoadBankAccounts();
            }
        }

        // Method to load bank accounts
        private void LoadBankAccounts()
        {
            string userID = Session["UserID"] as string;

            if (string.IsNullOrEmpty(userID))
            {
                // Redirect to login if no user is logged in
                Response.Redirect("~/Account/Login");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT AccountID, AccountType, AccountNumber, Balance FROM BankAccounts WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", userID);

                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Check if there are any rows
                    if (reader.HasRows)
                    {
                        gvBankAccounts.DataSource = reader;
                        gvBankAccounts.DataBind();
                    }
                    else
                    {
                        lblMessage.Text = "No bank accounts found.";
                        lblMessage.CssClass = "text-info";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred while fetching your accounts: " + ex.Message;
                lblMessage.CssClass = "text-danger";
            }
        }

        // Event handler for adding new bank account
        protected void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                string accountType = ddlAccountType.SelectedValue;
                decimal balance;

                if (decimal.TryParse(txtBalance.Text, out balance) && balance >= 0)
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            string query = @"
                                INSERT INTO BankAccounts (UserID, AccountType, Balance)
                                VALUES (@UserID, @AccountType, @Balance)";

                            SqlCommand cmd = new SqlCommand(query, conn);
                            cmd.Parameters.AddWithValue("@UserID", userID);
                            cmd.Parameters.AddWithValue("@AccountType", accountType);
                            cmd.Parameters.AddWithValue("@Balance", balance);

                            conn.Open();
                            cmd.ExecuteNonQuery();
                        }

                        // Refresh the GridView after adding a new account
                        LoadBankAccounts();

                        // Display success message
                        lblMessage.Text = "Bank account added successfully!";
                        lblMessage.CssClass = "text-success";
                    }
                    catch (Exception ex)
                    {
                        lblMessage.Text = "An error occurred while adding the bank account: " + ex.Message;
                        lblMessage.CssClass = "text-danger";
                    }
                }
                else
                {
                    lblMessage.Text = "Please enter a valid balance.";
                    lblMessage.CssClass = "text-danger";
                }
            }
            else
            {
                // User is not logged in
                Response.Redirect("~/Account/Login");
            }
        }
    }
}
