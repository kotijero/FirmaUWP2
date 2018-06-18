using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.DomainViewModels
{
    public class DrzavaViewModel : DetailsViewModelBase<Drzava>
    {
        private BlDrzava BlDrzava = new BlDrzava();
        public DrzavaViewModel()
            :base((t, s) => t.NazDrzave.ToLower().Contains(s.ToLower()) || t.OznDrzave.ToLower().Contains(s.ToLower()) ) // uvjet filtriranja
        {
            MjestoViewModel = new MjestoViewModel();
        }

        public MjestoViewModel MjestoViewModel { get; private set; }

        public async override Task<string> Load()
        {
            //FilteredListViewModel.SetItemsList(BlDrzava.FetchAll());
            Loading = false;
            throw new NotImplementedException();
        }

        public new void PositionChanged()
        {
            if (FilteredListViewModel.CurrentPosition < 0)
            {
                ShowDetails = false;
            } else
            {
                MjestoViewModel.SetMjestoList(CurrentItem.Mjesta);
                OnPropertyChanged(nameof(CurrentItem));
            }
        }

        #region Actions

        public override string Edit()
        {
            throw new NotImplementedException();
        }

        public override string New()
        {
            throw new NotImplementedException();
        }

        public override string Save()
        {
            throw new NotImplementedException();
        }

        public override void Cancel()
        {

        }

        public override string Delete()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Validation
        public override void ClearErrors(bool notify)
        {
            throw new NotImplementedException();
        }

        public override void ValidateProperty(string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
