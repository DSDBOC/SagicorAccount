using System;
using System.Web.UI;

namespace SagicorAccount.Account
{
    public partial class WebForm1 : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if the user is logged in (i.e., session variables are set)
            if (Session["UserID"] != null)
            {
                // Retrieve user details from session
                string userID = Session["UserID"].ToString();
                string firstName = Session["FirstName"].ToString();
                string lastName = Session["LastName"].ToString();

                // Display user details on the page
                lblUserID.Text = "User ID: " + userID;
                lblFullName.Text = "Name: " + firstName + " " + lastName;
            }
            else
            {
                // Redirect to login page if session is not available (i.e., user is not logged in)
                Response.Redirect("~/Account/Login.aspx");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear the session to log out the user
            Session.Clear();
            Session.Abandon();

            // Redirect to the homepage after logout
            Response.Redirect("~/");
        }

    }
}
