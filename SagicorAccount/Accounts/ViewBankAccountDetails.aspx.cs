using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class ViewBankAccountDetails : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {

        }



        protected void btnAddAccount_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] != null)
            {
                string userID = Session["UserID"].ToString();
                string accountType = ddlAccountType.SelectedValue;
                decimal balance;

                if (decimal.TryParse(txtBalance.Text, out balance) && balance >= 0)
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

                    // Refresh the GridView and show a success message                  
                    lblMessage.Text = "Bank account added successfully!";
                    lblMessage.CssClass = "text-success";
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