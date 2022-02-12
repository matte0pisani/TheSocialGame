﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;
using Xamarin.Essentials;
using Xamarin.Forms;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;



namespace TheSocialGame
{
    public partial class AddExperiencePage : ContentPage
    {
        private Utente user;
        private int nuoviPuntiSocial;
        private Page prossima;
        private Esperienza nuova;
        public AddExperiencePage(Utente us)
        {
            InitializeComponent();
            user = us;
            nuoviPuntiSocial = 0;
            nuova = new Esperienza();
            App.Current.Resources["BackgroundColor"] = user.Sfondo;
            App.Current.Resources["FirstColor"] = user.Primario;
            App.Current.Resources["SecondColor"] = user.Secondario;
            nuova.ListaPartecipanti.Add(user);
            EsciSenzaSalvareFrame.IsVisible = false;
            CopertinaFrame.IsVisible = false;
            Rimuovi.IsVisible = false;
            LabelFromTime.IsVisible = false;
            FromTime.IsVisible = false;
            LabelToTime.IsVisible = false;
            ToTime.IsVisible = false;
            WarningPartecipanti.IsVisible = false;
            WarningTipologia.IsVisible = false;
            WarningTitolo.IsVisible = false;
            TipiEsp.ItemsSource = new List<string>(user.ListaDistintivi.Keys);
        }

        /* metodi di apertura pagine connesse */
        void NotificationClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new NotificationPage();
        }

        void HomeClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new HomePage();
        }

        void ProfileClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new ProfilePage(user);
        }

        void SearchClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new SearchPage();
        }

        void RankingClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new RankingPage(user);
        }

        void BackClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = true;
            prossima = new ProfilePage(user);
        }

        void AnnullaClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = false;
        }

        async void ConfermaClicked(Object sender, EventArgs e)
        {
            EsciSenzaSalvareFrame.IsVisible = false;
            await Navigation.PushAsync(prossima);
            Navigation.RemovePage(this);
        }

        void TitoloCopertina(Object sender, EventArgs e)
        {
            nuova.Titolo = Titolo.Text;
        }


        /* gestione foto profilo da fotocamera*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void CameraClicked(Object sender, EventArgs e)
        {
            var foto = await MediaPicker.CapturePhotoAsync();

            if (foto != null)
            {
                using (var stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        nuova.Copertina = ms.ToArray();
                    }
                }

                CopertinaFrame.IsVisible = true;
                AvvisoCopertina.IsVisible = false;
                Rimuovi.IsVisible = true;
                Fotocamera.IsVisible = false;
                Galleria.IsVisible = false;
                Copertina.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(nuova.Copertina);
                });

                switch (Device.RuntimePlatform)
                {
                    case Device.iOS:
                        nuova.CopertinaLiveIOS = true;
                        Copertina.Rotation = 90;
                        break;
                }
            }

        }

        /* gestione foto profilo da galleria*/
        //Quando avremo il database bisogna gestire l'eliminazione della vecchia foto dal daltabase
        async void FromGalleryClicked(Object sender, EventArgs e)
        {

            var foto = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
            {
                Title = "Scegli la tua immagine del profilo!"
            });

            if (foto != null)
            {
                using (var stream = await foto.OpenReadAsync())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        nuova.Copertina = ms.ToArray();
                    }
                }
                CopertinaFrame.IsVisible = true;
                AvvisoCopertina.IsVisible = false;
                Rimuovi.IsVisible = true;
                Fotocamera.IsVisible = false;
                Galleria.IsVisible = false;
                Copertina.Source = ImageSource.FromStream(() =>
                {
                    return new MemoryStream(nuova.Copertina);
                });

            }
        }

        void RimuoviClicked(Object sender, EventArgs e)
        {
            nuova.Copertina = null; // simile problema che con immagine profilo, forse spreco spazio e peggioramento prestazioni
            CopertinaFrame.IsVisible = false;
            AvvisoCopertina.IsVisible = true;
            Rimuovi.IsVisible = false;
            Fotocamera.IsVisible = true;
            Galleria.IsVisible = true;
        }

        void DataInizioSelected(Object sender, EventArgs e)
        {
            nuova.DataInizio = DataInizio.Date;
            DataFine.MinimumDate = DataInizio.Date;
        }

        void DataFineSelected(Object sender, EventArgs e)
        {
            nuova.DataFine = DataFine.Date;
            DataInizio.MaximumDate = DataFine.Date;
        }

        //Capire come gestire esperienze in diretta
        void LiveChanged(Object sender, EventArgs e)
        {
            if (Live.IsToggled)
            {
                LabelFromTime.IsVisible = true;
                FromTime.IsVisible = true;
                LabelToTime.IsVisible = true;
                ToTime.IsVisible = true;
                nuova.Live = true;
            }
            else
            {
                LabelFromTime.IsVisible = false;
                FromTime.IsVisible = false;
                LabelToTime.IsVisible = false;
                ToTime.IsVisible = false;
                nuova.Live = false;
            }
        }

        private void TipoEsperienza(Object sender, EventArgs e)
        {
            nuova.Tipologia = (string)TipiEsp.SelectedItem;
        }

        private async void AggiungiPartecipanti(Object sender, EventArgs e)
        {
            if (!Partecipanti.Text.Equals(""))
            {
                foreach (Utente friend in user.Amici.Keys)
                {
                    if (friend.Username.Equals(Partecipanti.Text))
                    {
                        nuova.ListaPartecipanti.Add(friend);
                        ListaPartecipanti.Text = ListaPartecipanti.Text + "@" + friend.Username + "  ";
                        return;
                    }
                }

                Utente DBfriend = await DBmanager.GetUtenteBasePerNome(Partecipanti.Text);
                if (DBfriend != null) 
                {
                    nuoviPuntiSocial++;     // aumento i punti social ad ogni nuovo amico; possibile uso dei punti social, DA RIVEDERE
                    nuova.ListaPartecipanti.Add(DBfriend);
                    ListaPartecipanti.Text = ListaPartecipanti.Text + "@" + DBfriend.Username + "  "; 
                }
                else { await DisplayAlert("Utente inesistente", "Non esiste alcun utente con il nome dato", "OK") ; }

                Partecipanti.Text = null;
            }

        }

        private void PrivataChanged(Object sender, EventArgs e)
        {
            if (Privata.IsToggled)
                nuova.Privata = true;
            else nuova.Privata = false;
        }

        private async void Salva(object sender, EventArgs e)
        {
            if (nuova.DataInizio.Equals(new DateTime())) nuova.DataInizio = DataInizio.Date;
            if (nuova.DataFine.Equals(new DateTime())) nuova.DataFine = DataFine.Date;

            if (nuova.Titolo == null || nuova.Tipologia == null || nuova.ListaPartecipanti.Count == 0)
            {
                SavingLabel.IsVisible = false;
                Warning.IsVisible = true;
                if (nuova.Titolo == null) WarningTitolo.IsVisible = true;
                if (nuova.Tipologia == null) WarningTipologia.IsVisible = true;
                if (nuova.ListaPartecipanti.Count == 1) WarningPartecipanti.IsVisible = true;
            }
            else
            {
                foreach (Utente u in nuova.ListaPartecipanti)
                {
                    u.Esperienze.Add(nuova);
                    int numExp = u.ListaDistintivi[nuova.Tipologia].Item1;
                    Dictionary<int, bool> dictExp = u.ListaDistintivi[nuova.Tipologia].Item2;
                    numExp++;
                    if (numExp >= Constants.sogliaPrimoLivello)
                    {
                        dictExp[1] = true;      // non parametrico, in caso da re-implementare con ciclo
                        if (numExp >= Constants.sogliaSecondoLivello)
                        {
                            dictExp[2] = true;
                        }
                    }
                    u.ListaDistintivi[nuova.Tipologia] = (numExp, dictExp);
                    u.PuntiEsperienza++;
                    u.PuntiSocial += nuoviPuntiSocial;  // non credo sia corretto, DA RIVEDERE
                    u.Livello = (u.PuntiSocial + u.PuntiEsperienza) / 10 + 1;       // formula presa da PuntiFake; DA RIVEDERE
                    u.AggiungiAmici(nuova.ListaPartecipanti);
                }

                await Navigation.PushAsync(new ProfilePage(user));
                Navigation.RemovePage(this);
            }
        }

    }
}
