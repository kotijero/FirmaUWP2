using Firma.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModel.DomainViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Firma.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class KorisniciList : Page
    {
        KorisnikViewModel ViewModel;
        public KorisniciList()
        {
            ViewModel = new KorisnikViewModel();
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ViewModel.LoggedInUser = (Korisnik) e.Parameter;
            await ViewModel.Load();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            ViewModel.ShowDetails = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private async void NewButton_Click(object sender, RoutedEventArgs e)
        {
            CustomControls.NewUserContentDialog newUserContentDialog = new CustomControls.NewUserContentDialog();
            var result = await newUserContentDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                ViewModel.New(newUserContentDialog.Korisnik);
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Potvrda brisanja",
                Content = $"Jeste li sigurni da želite obrisati korisnika {ViewModel.CurrentKorisnik.Username}?",
                PrimaryButtonText = "Da",
                CloseButtonText = "Odustani"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var response = ViewModel.Delete();

                if (!string.IsNullOrEmpty(response))
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Pogreška",
                        Content = response,
                        CloseButtonText = "U redu"
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }
    }
}
