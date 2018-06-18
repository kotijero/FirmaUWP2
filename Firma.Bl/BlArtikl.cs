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
    public class BlArtikl : BlBase
    {
        ArtiklDalProvider artiklDalProvider = new ArtiklDalProvider();
        public ResultWrapper<Artikl> Fetch(int Id)
        {
            Artikl artikl = null;
            string errorMessage = string.Empty;
            try
            {
                artikl = new Artikl(artiklDalProvider.Fetch(Id));
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Artikl>(artikl, errorMessage);
        }

        public ResultWrapper<List<Artikl>> FetchAll()
        {
            List<Artikl> artiklList = null;
            string errorMessage = string.Empty;

            try
            {
                var artiklDtoList = artiklDalProvider.FetchAll().OrderBy(t => t.NazArtikla);
                artiklList = new List<Artikl>();
                foreach (var artikl in artiklDtoList)
                {
                    artiklList.Add(new Artikl(artikl));
                }
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<Artikl>>(artiklList, errorMessage);
        }

        public ResultWrapper<Artikl> AddItem(Artikl item)
        {
            Artikl artikl = null;
            string errorMessage = string.Empty;
            try
            {
                artikl = new Artikl(artiklDalProvider.AddItem(item.ToDTO()));
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Artikl>(artikl, errorMessage);
        }

        public ResultWrapper<Artikl> UpdateItem(Artikl item)
        {
            Artikl artikl = null;
            string errorMessage = string.Empty;
            try
            {
                artikl = new Artikl(artiklDalProvider.UpdateItem(item.ToDTO()));
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Artikl>(artikl, errorMessage);
        }

        public ResultWrapper<Artikl> DeleteItem(Artikl item)
        {
            Artikl artikl = null;
            string errorMessage = string.Empty;
            try
            {
                StavkaDalProvider stavkaDal = new StavkaDalProvider();
                if (stavkaDal.CheckStavkeForSifArtikla(item.SifArtikla))
                {
                    errorMessage = "Nije moguće obrisati artikl jer postoje stavke dokumenta sa njime.";
                }
                else
                {
                    artiklDalProvider.DeleteItem(item.ToDTO());
                }
                artikl = item;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Artikl>(artikl, errorMessage);
        }

        public ResultWrapper<List<LookupModel>> FetchLookup()
        {
            List<LookupModel> lookupList = null;
            string errorMessage = string.Empty;
            try
            {
                lookupList = artiklDalProvider.FetchLookup();
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<LookupModel>>(lookupList, errorMessage);
        }

        #region Validation

        public void ValidateProperty(Artikl partner, ArtiklValidationModel validationModel, string propertyName)
        {
            validationModel.ClearProperty(propertyName);
            switch (propertyName)
            {
                case nameof(Artikl.SifArtikla):
                    {
                        break;
                    }
                case nameof(Artikl.NazArtikla):
                    {
                        if (string.IsNullOrEmpty(partner.NazArtikla))
                            validationModel.NazArtikla = "Naziv artikla je obavezno polje!";
                        if (partner.NazArtikla.Length > NazArtiklaMaxLength)
                            validationModel.NazArtikla = $"Maksimalna duljina naziva je {NazArtiklaMaxLength}.";
                        break;
                    }
                case nameof(Artikl.JedMjere):
                    {
                        if (string.IsNullOrEmpty(partner.JedMjere))
                            validationModel.JedMjere = "Jedinica mjere je obavezno polje!";
                        if (partner.JedMjere.Length > JedMjereMaxLength)
                            validationModel.JedMjere = $"Maksimalna duljina jedinice mjere je {JedMjereMaxLength}.";
                        break;
                    }
                case nameof(Artikl.CijArtikla):
                    {
                        if (partner.CijArtikla <= 0)
                            validationModel.CijArtikla = "Cijena mora biti pozitivna vrijednost!";
                        break;
                    }
            }
        }

        public void ValidateModel(Artikl artikl, ArtiklValidationModel validationModel)
        {
            ValidateProperty(artikl, validationModel, nameof(Artikl.SifArtikla));
            ValidateProperty(artikl, validationModel, nameof(Artikl.NazArtikla));
            ValidateProperty(artikl, validationModel, nameof(Artikl.JedMjere));
            ValidateProperty(artikl, validationModel, nameof(Artikl.CijArtikla));
        }

        #region Business Rules Constants

        private const int NazArtiklaMaxLength = 255;
        private const int JedMjereMaxLength = 5;

        #endregion

        #endregion
    }
}
