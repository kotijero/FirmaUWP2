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
    public class MjestoViewModel : DetailsViewModelBase<Mjesto>
    {
        public MjestoViewModel()
            :base((t, s) => t.NazMjesta.ToLower().Contains(s))
        {
            itemsList = new List<Mjesto>();
        }

        public override Task<string> Load()
        {
            throw new NotImplementedException();
        }

        public void SetMjestoList(List<Mjesto> mjesta)
        {
            itemsList = mjesta;
            FilteredListViewModel.SetItemsList(mjesta);
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
