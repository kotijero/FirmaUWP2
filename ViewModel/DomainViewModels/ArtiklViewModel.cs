using Firma.Bl;
using Firma.Bl.ValidationModels;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DomainViewModels
{
    public class ArtiklViewModel : DetailsViewModelBase<Artikl>
    {
        private BlArtikl BlArtikl = new BlArtikl();
        public ArtiklViewModel()
            :base((t, s) => t.NazArtikla.ToLower().Contains(s.ToLower()))
        {
            List<Firma.Helpers.ListableModel> listableModels = new List<Firma.Helpers.ListableModel>();
            Dokument dokument = new Dokument();
            listableModels.Add(dokument);
        }

        public async override Task<string> Load()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () =>
                {
                    FilteredListViewModel.ItemsList.Clear();
                    Loading = true;
                    ShowDetails = false;
                });

            var result = await Task.Run(() => BlArtikl.FetchAll());
            if (string.IsNullOrEmpty(result.ErrorMessage))
            {
                itemsList = result.Value;
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                    () =>
                    {
                        FilteredListViewModel.SetItemsList(itemsList);
                        Loading = false;
                    });
                return string.Empty;
            }
            else
            {
                itemsList = new List<Artikl>();
                return result.ErrorMessage;
            }
        }

        #region CRUD

        public override void Cancel()
        {
            InEditMode = false;
            if (isNewItem)
            {
                isNewItem = false;
                if (FilteredListViewModel.CurrentPosition < 0)
                    ShowDetails = false;
            }
            else
            {
                CurrentItem.NazArtikla = oldItem.NazArtikla;
                CurrentItem.JedMjere = oldItem.JedMjere;
                CurrentItem.CijArtikla = oldItem.CijArtikla;
                CurrentItem.ZastUsluga = oldItem.ZastUsluga;
                CurrentItem.SlikaArtikla = oldItem.SlikaArtikla;
                CurrentItem.TekstArtikla = oldItem.TekstArtikla;
                CurrentItem.ResetImageSource();
            }
            ClearErrors(true);
            OnPropertyChanged(nameof(CurrentItem));
        }

        public override string Delete()
        {
            Artikl artiklToDelete = CurrentItem;
            var result = BlArtikl.DeleteItem(artiklToDelete);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return result.ErrorMessage;
            }
            itemsList.Remove(artiklToDelete);
            FilteredListViewModel.Filter = string.Empty;
            return string.Empty;
        }

        public override string Edit()
        {
            if (!inEditMode)
            {
                InEditMode = true;
                oldItem = new Artikl
                {
                    NazArtikla = CurrentItem.NazArtikla,
                    JedMjere = CurrentItem.JedMjere,
                    CijArtikla = CurrentItem.CijArtikla,
                    ZastUsluga = CurrentItem.ZastUsluga,
                    SlikaArtikla = CurrentItem.SlikaArtikla,
                    SlikaArtiklaImage = CurrentItem.SlikaArtiklaImage,
                    TekstArtikla = CurrentItem.TekstArtikla
                };
            }
            return string.Empty;
        }

        

        public override string New()
        {
            newItem = new Artikl();
            IsNewItem = true;
            InEditMode = true;
            OnPropertyChanged(nameof(CurrentItem));
            return string.Empty;
        }

        public override string Save()
        {
            if (isNewItem)
            {
                var result = BlArtikl.AddItem(newItem);
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return result.ErrorMessage;
                }
                itemsList.Add(newItem);
                FilteredListViewModel.Filter = string.Empty;
                FilteredListViewModel.CurrentPosition = FilteredListViewModel.ItemsList.IndexOf(newItem);
                IsNewItem = false;
            }
            else
            {
                var result = BlArtikl.UpdateItem(CurrentItem);
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    return result.ErrorMessage;
                }
            }
            InEditMode = false;
            return string.Empty;
        }

        #endregion

        #region Image Actions

        public async void ChangeImage()
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            var file = await picker.PickSingleFileAsync();
            if (file != null) CurrentItem.SetSlikaArtiklaFromFile(file);
        }

        public void RemoveImage()
        {
            CurrentItem.SetSlikaArtiklaFromFile(null);
        }

        #endregion

        #region

        public override void ClearErrors(bool notify)
        {
            Errors.ClearErrors(notify);
        }
        public ArtiklValidationModel Errors { get; } = new ArtiklValidationModel();

        public override void ValidateProperty(string propertyName)
        {
            if (inEditMode)
                BlArtikl.ValidateProperty(CurrentItem, Errors, propertyName);
        }

        #endregion
    }
}
