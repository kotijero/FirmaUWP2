using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Firma.CustomControls
{
    public sealed partial class DokumentPickerDialog : ContentDialog
    {
        DokumentPickerViewModel ViewModel;
        public DokumentPickerDialog(DokumentPickerViewModel viewModel)
        {
            ViewModel = viewModel;
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            var response = ViewModel.CanSelect();
            if (!string.IsNullOrEmpty(response))
            {
                ErrorTextBlock.Text = response;
                args.Cancel = true;
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.SelectionMade = false;
        }
    }
}
