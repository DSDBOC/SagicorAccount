using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount
{
    public partial class Logout : System.Web.UI.Page
    {
        // Logout.aspx.cs

        protected void Page_Load(object sender, EventArgs e)
        {
            // Clear the entire session
            Session.Clear();

            // Abandon the session to ensure no session data persists
            Session.Abandon();

            // Clear any session-related cookies (optional)
            Response.Cookies.Clear();

            // Redirect to the default or home page (adjust the path as needed)
            Response.Redirect("~/Default.aspx");  // Or redirect to the desired home page
        }

    }
}