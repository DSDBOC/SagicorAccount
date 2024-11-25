using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if the user is logged in via session
                if (Session["UserID"] != null)
                {
                    // User is logged in, show Account dropdown and hide Login/Register
                    navAccount.Visible = true;
                    navRegister.Visible = false;
                    navLogin.Visible = false;
                }
                else
                {
                    // User is not logged in, show Login/Register and hide Account dropdown
                    navAccount.Visible = false;
                    navRegister.Visible = true;
                    navLogin.Visible = true;
                }
            }
        }

    }
}