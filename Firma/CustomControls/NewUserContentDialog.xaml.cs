using Firma.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModel;
using ViewModel.CustomControlViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Firma.CustomControls
{
    public sealed partial class NewUserContentDialog : ContentDialog
    {
        NewKorisnikViewModel ViewModel;
        public Korisnik Korisnik;
        public NewUserContentDialog()
        {
            Korisnik = new Korisnik();
            ViewModel = new NewKorisnikViewModel();
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            if (!ViewModel.AddUser())
            {
                args.Cancel = true;
            }
            else
            {
                Korisnik = ViewModel.Korisnik;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }
    }
}
