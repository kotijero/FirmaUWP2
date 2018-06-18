using Firma.Bl.ValidationModels;
using Firma.DAL;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class BlMjesto : BlBase
    {
        MjestoDalProvider mjestoDalProvider = new MjestoDalProvider();

        public ResultWrapper<List<LookupModel>> FetchLookup()
        {
            List<LookupModel> lookupList = null;
            string errorMessage = string.Empty;
            try
            {
                lookupList = mjestoDalProvider.FetchLookup();
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<LookupModel>>(lookupList, errorMessage);
        }

        public void ValidateProperty(Mjesto mjesto, MjestoValidationModel validationModel, string propertyName)
        {
            validationModel.ClearErrors(true);
            switch (propertyName)
            {
                case nameof(Mjesto.IdMjesta):
                    {
                        break;
                    }
                case nameof(Mjesto.OznDrzave):
                    {
                        if (string.IsNullOrEmpty(mjesto.OznDrzave))
                            validationModel.OznDrzave = "Oznaka države je obavezno polje!";
                        break;
                    }
                case nameof(Mjesto.NazMjesta):
                    {
                        if (string.IsNullOrEmpty(mjesto.NazMjesta))
                            validationModel.NazMjesta = "Naziv mjesta je obavezno polje!";
                        else if (mjesto.NazMjesta.Length > 40)                   // TODO - premjesti u pravila
                            validationModel.NazMjesta = "Maksimalna duljina naziva mjesta je 40.";
                        break;
                    }
                case nameof(Mjesto.PostNazMjesta):
                    {
                        if (mjesto.PostNazMjesta != null && mjesto.PostNazMjesta.Length > 50)
                            validationModel.PostNazMjesta = "Maksimalna duljina je 50.";
                        break;
                    }
                case nameof(Mjesto.PostBrMjesta):
                    {
                        if (!mjesto.PostBrMjesta.HasValue || mjesto.PostBrMjesta.Value <= 0)
                            validationModel.PostBrMjesta = "Poštanski broj mora biti veći od 0.";
                        break;
                    }
            }
        }
    }
}
