using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class ArtiklDetails : Page
    {
        private ArtiklViewModel ViewModel;
        public ArtiklDetails()
        {
            ViewModel = new ArtiklViewModel();
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await Task.Run(() => ViewModel.Load());
        }

        protected async override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
            if (ViewModel.InEditMode)
            {
                ContentDialog contentDialog = new ContentDialog
                {
                    Title = "Spremanje izmjena",
                    Content = "Želite li spremiti izmjene?",
                    PrimaryButtonText = "Da",
                    SecondaryButtonText = "Ne",
                    CloseButtonText = "Natrag"
                };
                var response = await contentDialog.ShowAsync();
                if (response == ContentDialogResult.Primary)
                {
                    var result = ViewModel.Save();
                    if (!string.IsNullOrEmpty(result))
                    {
                        e.Cancel = true;
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Pogreška",
                            Content = result,
                            CloseButtonText = "U redu"
                        };
                        await errorDialog.ShowAsync();
                    }
                }
                else if (response == ContentDialogResult.Secondary)
                {
                    ViewModel.Cancel();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                ViewModel.ShowDetails = false;
            }
        }

        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }
    }
}
