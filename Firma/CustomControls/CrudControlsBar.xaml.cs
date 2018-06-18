using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Firma.CustomControls
{
    public sealed partial class CrudControlsBar : UserControl
    {
        public CrudControlsBar()
        {
            this.InitializeComponent();
        }

        public CrudControlsViewModel ViewModel
        {
            get { return (CrudControlsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(CrudControlsViewModel), typeof(CrudControlsBar), new PropertyMetadata(null));

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = CreateConfirmationDialog(SaveItemMessage ?? "Jeste li sigurni da želite spremiti zapis?", "Potvrda spremanja");
            var reponse = await contentDialog.ShowAsync();
            if (reponse == ContentDialogResult.Primary)
            {
                var result = ViewModel.Save();
                if (!string.IsNullOrEmpty(result))
                {
                    ContentDialog errorDialog = CreateErrorDialog(result);
                    await errorDialog.ShowAsync();
                }
            }
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = CreateConfirmationDialog(SaveItemMessage ?? "Jeste li sigurni da želite odbaciti promjene?", "Potvrda");
            var reponse = await contentDialog.ShowAsync();
            if (reponse == ContentDialogResult.Primary)
            {
                ViewModel.Cancel();
            }
        }

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.New();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Edit();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog contentDialog = CreateConfirmationDialog(SaveItemMessage ?? "Jeste li sigurni da želite izbrisati zapis?", "Potvrda brisanja");
            var reponse = await contentDialog.ShowAsync();
            if (reponse == ContentDialogResult.Primary)
            {
                var result = ViewModel.Delete();
                if (!string.IsNullOrEmpty(result))
                {
                    ContentDialog errorDialog = CreateErrorDialog(result);
                    await errorDialog.ShowAsync();
                }
            }
        }

        private ContentDialog CreateConfirmationDialog(string content, string title)
        {
            return new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = "Da",
                CloseButtonText = "Ne"
            };
        }

        private ContentDialog CreateErrorDialog(string content)
        {
            return new ContentDialog
            {
                Title = "Pogreška",
                Content = content,
                CloseButtonText = "U redu"
            };
        }

        public string SaveItemMessage
        {
            get { return (string)GetValue(SaveItemMessageProperty); }
            set { SetValue(SaveItemMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SaveItemMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SaveItemMessageProperty =
            DependencyProperty.Register(nameof(SaveItemMessage), typeof(string), typeof(CrudControlsBar), new PropertyMetadata(null));


        public string CancelMessage
        {
            get { return (string)GetValue(CancelMessageProperty); }
            set { SetValue(CancelMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CancelMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CancelMessageProperty =
            DependencyProperty.Register(nameof(CancelMessage), typeof(string), typeof(CrudControlsBar), new PropertyMetadata(null));


        public string DeleteMessage
        {
            get { return (string)GetValue(DeleteMessageProperty); }
            set { SetValue(DeleteMessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeleteMessage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeleteMessageProperty =
            DependencyProperty.Register("DeleteMessage", typeof(string), typeof(CrudControlsBar), new PropertyMetadata(null));
        
    }
}
