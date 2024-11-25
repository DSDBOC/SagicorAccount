using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SagicorAccount.Account
{
    public partial class MakePayments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadLinkedAccounts();
            }
        }

        private void LoadLinkedAccounts()
        {
            // Simulate fetching linked accounts for the logged-in user
            var linkedAccounts = new[]
            {
                new { AccountNumber = "12345", DisplayName = "12345 - John Doe" },
                new { AccountNumber = "67890", DisplayName = "67890 - Jane Smith" }
            };

            ddlLinkedAccounts.DataSource = linkedAccounts;
            ddlLinkedAccounts.DataTextField = "DisplayName";
            ddlLinkedAccounts.DataValueField = "AccountNumber";
            ddlLinkedAccounts.DataBind();
            ddlLinkedAccounts.Items.Insert(0, new ListItem("-- Select an Account --", ""));
        }

        protected void btnSubmitPayment_Click(object sender, EventArgs e)
        {
            string selectedBankAccount = ddlLinkedAccounts.SelectedValue;
            string flowAccountNumber = txtFlowAccountNumber.Text;
            decimal paymentAmount;

            if (string.IsNullOrEmpty(selectedBankAccount) || string.IsNullOrEmpty(flowAccountNumber) || !decimal.TryParse(txtPaymentAmount.Text, out paymentAmount))
            {
                lblPaymentStatus.Text = "Please provide valid details for the payment.";
                lblPaymentStatus.CssClass = "text-danger";
                return;
            }

            // Call the PaymentService to process the payment
            PaymentService paymentService = new PaymentService();
            string paymentResult = paymentService.ProcessPayment(selectedBankAccount, flowAccountNumber, paymentAmount);

            lblPaymentStatus.Text = paymentResult;

            // Display success or error class based on the result
            lblPaymentStatus.CssClass = paymentResult.Contains("successful") ? "text-success" : "text-danger";
        }

    }
}
