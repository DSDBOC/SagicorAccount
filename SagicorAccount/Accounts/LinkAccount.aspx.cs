using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.UI;
using SagicorLife.Class;

namespace SagicorAccount.Account
{
    public partial class LinkAccount : Page
    {
        // Singleton instance for HttpClient
        private static readonly HttpClient client = new HttpClient();

        protected async void btnLinkAccount_Click(object sender, EventArgs e)
        {
            string accountNumber = txtAccountNumber.Text.Trim();
            string selectedPlatform = rblPlatform.SelectedValue; // FLOW or SAGICOR

            // Validate input
            if (string.IsNullOrEmpty(accountNumber))
            {
                DisplayMessage("Please enter a valid account number.", false);
                return;
            }

            if (string.IsNullOrEmpty(selectedPlatform))
            {
                DisplayMessage("Please select a platform (FLOW or SAGICOR LIFE).", false);
                return;
            }

            // Call mock service to verify account
            bool isAccountValid = await VerifyAccountExistsAsync(selectedPlatform, accountNumber);

            if (isAccountValid)
            {
                // Logic to link the account (e.g., database entry)
                DisplayMessage("Account successfully linked!", true);
            }
            else
            {
                DisplayMessage("Account not found. Please check your account number.", false);
            }
        }

        private async Task<bool> VerifyAccountExistsAsync(string platform, string accountNumber)
        {
            string apiUrl = string.Empty;

            // Determine the correct mock service URL
            if (platform == "FLOW")
            {
                apiUrl = $"http://localhost:5000/MockServices/MockAccountService.asmx/VerifyAccount?accountNumber={accountNumber}&accountType=FLOW";
            }
            else if (platform == "SAGICOR")
            {
                apiUrl = $"http://localhost:5000/MockServices/MockAccountService.asmx/VerifyAccount?accountNumber={accountNumber}&accountType=SAGICOR";
            }
            else
            {
                DisplayMessage("Invalid platform selected.", false);
                return false;
            }

            try
            {
                // Call the mock API
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string responseContent = await response.Content.ReadAsStringAsync();
                return bool.TryParse(responseContent, out bool result) && result;
            }
            catch (Exception ex)
            {
                // Log and display error message
                Console.WriteLine($"Error: {ex.Message}");
                DisplayMessage("An error occurred while verifying the account. Please try again later.", false);
                return false;
            }
        }

        private void DisplayMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = isSuccess ? System.Drawing.Color.Green : System.Drawing.Color.Red;
            lblMessage.Visible = true;
        }
    }
}
