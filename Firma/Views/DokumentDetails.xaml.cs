using Firma.CustomControls;
using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ViewModel;
using ViewModel.CustomControlViewModels;
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
    public sealed partial class DokumentDetails : Page
    {
        private DokumentViewModel ViewModel;
        public DokumentDetails()
        {
            ViewModel = new DokumentViewModel();
            this.InitializeComponent();
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
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

        private void DeleteStavkaButton_Click(object sender, RoutedEventArgs e)
        {
            var current = ((sender as Button).Tag as StavkaViewModel);
            ViewModel.RemoveStavka(current);
        }

        private void PartnerAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = ViewModel.QueryPartner(sender.Text);
            } else if (args.Reason == AutoSuggestionBoxTextChangeReason.ProgrammaticChange)
            {
                
            }
        }

        private void PartnerAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                ViewModel.SubmitPartner(((LookupModel)args.ChosenSuggestion).Key);
            }
        }

        private async void EditPrethodniDokument_Click(object sender, RoutedEventArgs e)
        {
            DokumentPickerViewModel dokumentPickerViewModel = ViewModel.GenerateDokumentPickerViewModel();
            DokumentPickerDialog dokumentPickerDialog = new DokumentPickerDialog(dokumentPickerViewModel);
            var response = await dokumentPickerDialog.ShowAsync();
            if (response == ContentDialogResult.Primary || response == ContentDialogResult.Secondary)
            {
                ViewModel.SubmitPrethodniDokument(dokumentPickerViewModel);
            }
        }

        private async void ChangeArtiklButton_Click(object sender, RoutedEventArgs e)
        {
            StavkaViewModel stavkaViewModel = (StavkaViewModel)((Button)sender).Tag;
            ArtiklPickerViewModel artiklPickerViewModel = stavkaViewModel.GenerateArtiklPickerViewModel();
            ArtiklPicker artiklPicker = new ArtiklPicker(artiklPickerViewModel);
            var response = await artiklPicker.ShowAsync();
            if (response == ContentDialogResult.Primary)
            {
                stavkaViewModel.SubmitArtikl(artiklPickerViewModel);
            }
        }

        private async void AddNewStavkaButton_Click(object sender, RoutedEventArgs e)
        {
            var stavkaViewModel = ViewModel.NewStavka();
            ArtiklPickerViewModel artiklPickerViewModel = stavkaViewModel.GenerateArtiklPickerViewModel();
            ArtiklPicker artiklPicker = new ArtiklPicker(artiklPickerViewModel);
            var response = await artiklPicker.ShowAsync();
            if (response == ContentDialogResult.Primary)
            {
                stavkaViewModel.SubmitArtikl(artiklPickerViewModel);
            }
            else
            {
                ViewModel.RemoveStavka(stavkaViewModel);
            }
        }
    }
}
