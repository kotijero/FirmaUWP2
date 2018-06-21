using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using ViewModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Firma
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainPageViewModel ViewModel;

        public MainPage()
        {
            ViewModel = new MainPageViewModel();
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        private void ArtiklButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.ArtiklDetails));
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Navigation logic
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Visible;
            }
            else
            {
                Windows.UI.Core.SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = Windows.UI.Core.AppViewBackButtonVisibility.Collapsed;
            }
            if (ViewModel.SignedIn == false)
            {
                Login();
            }
        }

        private void PartnerButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.PartnerDetails));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DokumentButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.DokumentDetails));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.SettingsPage), ViewModel.Korisnik);
        }

        private async void Login()
        {
            CustomControls.SignInContentDialog signInContentDialog = new CustomControls.SignInContentDialog();
            var result = await signInContentDialog.ShowAsync();
            if (signInContentDialog.ViewModel.Korisnik == null)
            {
                CloseApp();
            } else
            {
                ViewModel.Korisnik = signInContentDialog.ViewModel.Korisnik;
                ViewModel.SignedIn = true;
            }
        }

        public void CloseApp()
        {
            Application.Current.Exit();

        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }

        private void KorisniciButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.KorisniciList), ViewModel.Korisnik);
        }

        private void DokumentVirtualizationButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.DokumentVirtualizationDetails));
        }
    }
}
