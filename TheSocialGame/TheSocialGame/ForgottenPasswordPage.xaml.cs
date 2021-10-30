using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgottenPasswordPage : ContentPage
    {
        public ForgottenPasswordPage()
        {
            InitializeComponent();
        }

        public async Task SendEmail(string subject, string body, List<string> recipients)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = subject,
                    Body = body,
                    To = recipients,
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
                System.Diagnostics.Debug.Print(fbsEx.ToString());
            }
            catch (Exception ex)
            {
                // Some other exception occurred
                System.Diagnostics.Debug.Print(ex.ToString());
            }
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (EmailEntry.Text != null && EmailEntry.Text.Length > 0)
            {
                List<string> recipients = new List<string>();
                recipients.Add(EmailEntry.Text);
                await SendEmail("prova email TSG", "Lorem ipsum", recipients);
            }
        }
    }

}