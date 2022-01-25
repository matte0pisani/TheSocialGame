using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Collections.Generic;

namespace TheSocialGame
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgottenPasswordPage : ContentPage
    {
        IAuth auth;

        public ForgottenPasswordPage()
        {
            InitializeComponent();
            auth = DependencyService.Get<IAuth>();
        }



        private async void Button_Clicked(object sender, EventArgs e)
        {
            if (auth.PasswordDimenticata(EmailEntry.Text))
            {
                await DisplayAlert("RESET PASSWORD", "Ti abbiamo inviato una mail per reimpostare la tua password", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("ERRORE", "Qualcosa è andato storto, ritenta!", "OK");

            }

            /*if (EmailEntry.Text != null && EmailEntry.Text.Length > 0)
            {
                var client = new AmazonSimpleEmailServiceClient("", "", RegionEndpoint.EUSouth1);
                SendEmailRequest sendRequest = GenerateDefaultEmailRequest("matteo.pisani.roma@gmail.com", EmailEntry.Text);
                try
                {
                    System.Diagnostics.Debug.Print("Sending email using Amazon SES...");
                    await client.SendEmailAsync(sendRequest);
                    System.Diagnostics.Debug.Print("The email was sent successfully.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Print("The email was not sent.");
                    System.Diagnostics.Debug.Print("Error message: " + ex.Message);

                }
            }*/


        }



        async void BackClicked(Object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        /*
        private SendEmailRequest GenerateDefaultEmailRequest(string senderAddress, string receiverAddress)
        {
            string subject = "Recupero password account TheSocialGame";
            string textBody = "Amazon SES Test (.NET)\r\n"
                                        + "This email was sent through Amazon SES "
                                        + "using the AWS SDK for .NET.";
            string htmlBody = @"<html>
                    <head></head>
                        <body>
                            <h1>Amazon SES Test (AWS SDK for .NET)</h1>
                            <p>This email was sent with
                                <a href='https://aws.amazon.com/ses/'>Amazon SES</a> using the
                                <a href='https://aws.amazon.com/sdk-for-net/'>
                                AWS SDK for .NET</a>.</p>
                        </body>
             </html>";
            var sendRequest = new SendEmailRequest
            {
                Source = senderAddress,
                Destination = new Destination
                {
                    ToAddresses =
                        new List<string> { receiverAddress }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = htmlBody
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = textBody
                        }
                    }
                }
            };

            return sendRequest;
        }*/
    }
}
