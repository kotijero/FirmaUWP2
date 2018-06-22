using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using ViewModel;
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
    public sealed partial class PartnerDetails : Page
    {
        private PartnerViewModel ViewModel;
        public PartnerDetails()
        {
            ViewModel = new PartnerViewModel();
            this.InitializeComponent();
            this.NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            
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

        private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            On_BackRequested();
            args.Handled = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            On_BackRequested();
        }

        private void ValidateOnFocusLost(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidateProperty((sender as TextBox).Tag as string);
        }

        private void ValidateOnTextChanged(object sender, TextChangedEventArgs e)
        {
            ViewModel.ValidateProperty((sender as TextBox).Tag as string);
        }

        private void MjSjedistaAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = ViewModel.QueryMjesto(sender.Text);
            }
        }

        private void MjSjedistaAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                ViewModel.SubmitMjPartnera((LookupModel)args.ChosenSuggestion);
            }
        }

        private void MjIsporukeAutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                sender.ItemsSource = ViewModel.QueryMjesto(sender.Text);
            }
        }

        private void MjIsporukeAutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                ViewModel.SubmitMjPartnera((LookupModel)args.ChosenSuggestion);
            }
        }

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            (e.OriginalSource as TextBox).SelectAll();
        }
    }
}
