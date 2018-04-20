using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ViewModel
{
    public class DokumentViewModel : ObservableModel
    {
        public DokumentViewModel()
        {
            CreateNewDokuent();
            currentPosition = -1;
            inEditMode = false;
            Loading = true;
        }

        #region Loading Delay

        private bool loading;

        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public async Task Load()
        {
            FilteredDokumentList.Clear();
            BlArtikl blArtikl = new BlArtikl();
            ArtiklList = await Task.Run(() => blArtikl.FetchAll());

            BlPartner blPartner = new BlPartner();
            var partnerLookupList = await Task.Run(() => blPartner.FetchLookup());
            foreach(var partner in partnerLookupList)
            {
                PartnerLookupList.Add(partner);
            }
            PartnerLookupList.Add(Defaults.PartnerLookup);

            dokumentList = await Task.Run(() => blDokument.FetchAll(ArtiklList));

            DokumentLookupList = new List<LookupModel>
            {
                Defaults.DokumentLookup
            };
            foreach (var dokument in dokumentList)
            {
                dokument.PartnerLookup = PartnerLookupList.FirstOrDefault(t => t.Key.Equals(dokument.IdPartnera));
                DokumentLookupList.Add(new LookupModel(dokument.IdDokumenta, dokument.LookupData));
            }
            DokumentLookupList.Add(Defaults.DokumentLookup);
            foreach(var dokument2 in dokumentList)
            {
                dokument2.PrethodniDokumentLookup = DokumentLookupList.FirstOrDefault(t => t.Key.Equals(dokument2.IdPrethDokumenta));
                FilteredDokumentList.Add(dokument2);
                foreach(var stavka in dokument2.Stavke)
                {
                    stavka.ArtiklList = ArtiklList;
                }
            }
            OnPropertyChanged(nameof(ArtiklList));
            OnPropertyChanged(nameof(PartnerLookupList));
            Loading = false;
        }

        #endregion

        private BlDokument blDokument = new BlDokument();

        private List<Dokument> dokumentList;

        public ObservableCollection<Dokument> FilteredDokumentList { get; set; } = new ObservableCollection<Dokument>();

        private int currentPosition;

        public int CurrentPosition
        {
            get { return currentPosition; }
            set
            {
                currentPosition = value;
                OnPropertyChanged();
                if (value.Equals(-1))
                    ShowDetails = false;
                else
                {
                    ShowDetails = true;
                    OnPropertyChanged(nameof(CurrentDokument));
                }
            }
        }

        public Dokument CurrentDokument
        {
            get
            {
                if (isNewItem || loading) return newDokument;
                else return FilteredDokumentList[currentPosition];
            }
        }

        public List<Artikl> ArtiklList;

        private bool isNewItem;

        public bool IsNewItem
        {
            get { return isNewItem; }
        }

        private bool showDetails;
        public bool ShowDetails
        {
            get { return showDetails; }
            set
            {
                showDetails = value;
                OnPropertyChanged();
            }
        }

        #region Edit Mode

        private bool inEditMode;

        public bool InEditMode
        {
            get { return inEditMode; }
            set
            {
                inEditMode = value;
                SetStavkasEditMode();
                OnPropertyChanged();
                OnPropertyChanged(nameof(NotInEditMode));
            }
        }

        private void SetStavkasEditMode()
        {
            foreach (var stavka in CurrentDokument.Stavke)
            {
                stavka.InEditMode = inEditMode;
            }
        }
        
        public bool NotInEditMode
        {
            get { return !inEditMode; }
        }

        #endregion

        #region CRUD

        private Dokument originalDokument;
        public void Edit()
        {
            if (!inEditMode)
            {
                InEditMode = true;
                List<Stavka> stavkeList = new List<Stavka>();
                foreach(var stavka in CurrentDokument.Stavke)
                {
                    stavkeList.Add(new Stavka
                    {
                        IdStavke = stavka.IdStavke,
                        Artikl = stavka.Artikl,
                        KolArtikla = stavka.KolArtikla,
                        PostoRabat = stavka.PostoRabat,
                        IdDokumenta = stavka.IdDokumenta,
                        SifArtikla = stavka.SifArtikla,
                        ArtiklList = stavka.ArtiklList
                    });
                }
                originalDokument = new Dokument
                {
                    IdDokumenta = CurrentDokument.IdDokumenta,
                    IdPrethDokumenta = CurrentDokument.IdPrethDokumenta,
                    PartnerLookup = CurrentDokument.PartnerLookup,
                    IdPartnera = CurrentDokument.IdPartnera,
                    BrDokumenta = CurrentDokument.BrDokumenta,
                    VrDokumenta = CurrentDokument.VrDokumenta,
                    DatDokumenta = CurrentDokument.DatDokumenta,
                    PostoPorez = CurrentDokument.PostoPorez,
                    PrethodniDokumentLookup = CurrentDokument.PrethodniDokumentLookup
                };
                originalDokument.AddStavkeFromList(stavkeList, true);
            }
        }

        public async void Save()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Potvrda spremanja",
                Content = $"Spremiti dokument br. {CurrentDokument.BrDokumenta}?",
                PrimaryButtonText = "Da",
                CloseButtonText = "Natrag na uređivanje"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                InEditMode = false;
                if (isNewItem)
                {
                    newDokument = blDokument.AddItem(newDokument);
                    dokumentList.Add(newDokument);
                    isNewItem = false;
                    OnPropertyChanged(nameof(IsNewItem));
                    Filter = string.Empty;
                    ApplyFilter();
                    CurrentPosition = dokumentList.IndexOf(newDokument);
                }
                else
                {
                    blDokument.UpdateItem(CurrentDokument);
                }
            }
        }

        public void Cancel()
        {
            InEditMode = false;
            if (isNewItem)
            {
                isNewItem = false;
                OnPropertyChanged(nameof(IsNewItem));
            }
            else
            {
                CurrentDokument.IdDokumenta = originalDokument.IdDokumenta;
                CurrentDokument.IdPrethDokumenta = originalDokument.IdPrethDokumenta;
                CurrentDokument.PartnerLookup = originalDokument.PartnerLookup;
                CurrentDokument.BrDokumenta = originalDokument.BrDokumenta;
                CurrentDokument.VrDokumenta = originalDokument.VrDokumenta;
                CurrentDokument.DatDokumenta = originalDokument.DatDokumenta;
                CurrentDokument.PostoPorez = originalDokument.PostoPorez;
                CurrentDokument.PrethodniDokumentLookup = originalDokument.PrethodniDokumentLookup;
                CurrentDokument.AddStavkeFromList(originalDokument.Stavke.ToList(), true);
            }
        }

        private Dokument newDokument;

        private void CreateNewDokuent()
        {
            newDokument = new Dokument();
            if (loading)
            {
                
            }
        }

        public void New()
        {
            CreateNewDokuent();

            isNewItem = true;
            InEditMode = true;
            ShowDetails = true;
            OnPropertyChanged(nameof(IsNewItem));
            OnPropertyChanged(nameof(CurrentDokument));
        }

        public void NewStavka()
        {
            if (inEditMode)
            {
                CurrentDokument.Stavke.Add(new Stavka
                {
                    ArtiklList = ArtiklList,
                    InEditMode = inEditMode,
                    IdDokumenta = CurrentDokument.IdDokumenta
                });
            }
        }

        public void RemoveStavka(Stavka stavka)
        {
            CurrentDokument.Stavke.Remove(stavka);
        }

        public async void Delete()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Brisanje",
                Content = $"Jeste li sigurni da želite obrisati dokument br. {CurrentDokument.BrDokumenta}?",
                PrimaryButtonText = "Da",
                SecondaryButtonText = "Odustani"
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                Dokument toDelete = CurrentDokument;
                string response = blDokument.DeleteItem(toDelete);
                if (string.IsNullOrEmpty(response))
                {
                    dokumentList.Remove(toDelete);
                    ApplyFilter();
                }
                else
                {
                    ContentDialog warningDialog = new ContentDialog
                    {
                        Title = "Upozorenje",
                        Content = response,
                        PrimaryButtonText = "U redu"
                    };
                    await warningDialog.ShowAsync();
                }
            }
        }

        #endregion

        #region Filter

        private string filter;

        public string Filter
        {
            get { return filter; }
            set
            {
                filter = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }

        public void ApplyFilter()
        {
            FilteredDokumentList.Clear();
            foreach(var dokument in dokumentList.Where(t => t.BrDokumenta.ToString().Contains(filter) || t.VrDokumenta.Contains(filter)))
            {
                FilteredDokumentList.Add(dokument);
            }
        }

        #endregion

        #region Lookups

        public ObservableCollection<LookupModel> PartnerLookupList = new ObservableCollection<LookupModel>();
        public List<LookupModel> DokumentLookupList;

        #endregion

    }
}
