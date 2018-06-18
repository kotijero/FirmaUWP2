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
    public class BlPartner : BlBase
    {
        private PartnerDalProvider partnerDalProvider = new PartnerDalProvider();
        public ResultWrapper<Partner> Fetch(int Id)
        {
            Partner partner = null;
            string errorMessage = string.Empty;
            try
            {
                var partnerDto = partnerDalProvider.Fetch(Id);

                MjestoDalProvider mjestoDalProvider = new MjestoDalProvider();
                LookupModel mjestoPartneraLookup = null;
                LookupModel mjestoIsporukeLookup = null;

                if (!partnerDto.IdMjestaPartnera.Equals(-1)) mjestoPartneraLookup = mjestoDalProvider.FetchSingleLookup(partnerDto.IdMjestaPartnera.Value);
                else mjestoPartneraLookup = Defaults.MjestoLookup;

                if (!partnerDto.IdMjestaIsporuke.Equals(-1)) mjestoIsporukeLookup = mjestoDalProvider.FetchSingleLookup(partnerDto.IdMjestaIsporuke.Value);
                else mjestoPartneraLookup = Defaults.MjestoLookup;

                partner = new Partner(partnerDto, mjestoPartneraLookup, mjestoIsporukeLookup);
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Partner>(partner, errorMessage);
        }

        public ResultWrapper<List<Partner>> FetchAll()
        {
            List<Partner> partnerList = null;
            string errorMessage = string.Empty;
            try
            {
                var partnerDtoList = partnerDalProvider.FetchAll();

                MjestoDalProvider mjestoDalProvider = new MjestoDalProvider();
                var mjestoLookupList = mjestoDalProvider.FetchLookup();

                partnerList = new List<Partner>();
                foreach (var dto in partnerDtoList)
                {
                    partnerList.Add(new Partner(dto, mjestoLookupList));
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<Partner>>(partnerList, errorMessage);
         }
        public ResultWrapper<Partner> AddItem(Partner item)
        {
            Partner partner = null;
            string errorMessage = string.Empty;
            try
            {
                DTO.Partner partnerDto = partnerDalProvider.AddItem(item.ToDTO());
                item.IdPartnera = partnerDto.IdPartnera;
                partner = item;
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Partner>(partner, errorMessage);
        }

        public ResultWrapper<Partner> UpdateItem(Partner item)
        {
            Partner partner = null;
            string errorMessage = string.Empty;
            try
            {
                partnerDalProvider.UpdateItem(item.ToDTO());
                partner = item;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Partner>(partner, errorMessage);
        }

        public ResultWrapper<Partner> DeleteItem(Partner item)
        {
            Partner partner = null;
            string errorMessage = string.Empty;
            try
            {
                DokumentDalProvider dokumentDal = new DokumentDalProvider();
                if (dokumentDal.CheckDokumentForPartner(item.IdPartnera))
                {
                    errorMessage = "Nije moguće obrisati partnera jer postoje njegovi dokumenti.";
                }
                partnerDalProvider.DeleteItem(item.ToDTO());
                partner = item;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Partner>(partner, errorMessage);
        }

        public ResultWrapper<List<LookupModel>> FetchLookup()
        {
            List<LookupModel> lookupList = null;
            string errorMessage = string.Empty;
            try
            {
                lookupList = partnerDalProvider.FetchLookup();
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<LookupModel>>(lookupList, errorMessage);
        }

        #region Validation

        public void ValidateProperty(Partner partner, PartnerValidationModel validationModel, string propertyName)
        {
            validationModel.ClearProperty(propertyName);
            switch (propertyName)
            {
                case nameof(Partner.IdPartnera):
                    break;
                case nameof(Partner.TipPartnera):
                    {
                        if (!partner.TipPartnera.Equals(Constants.OsobaTip) && !partner.TipPartnera.Equals(Constants.TvrtkaTip))
                            validationModel.TipPartnera = "Tip partnera je obavezno polje!";
                        break;
                    }
                case nameof(Partner.AdrPartnera):
                    {
                        if (!string.IsNullOrEmpty(partner.AdrPartnera) && partner.AdrPartnera.Length > AdrPartneraMaxLength)
                            validationModel.AdrPartnera = $"Maksimalna duljina polja je {AdrPartneraMaxLength}!";
                        break;
                    }
                case nameof(Partner.AdrIsporuke):
                    {
                        if (!string.IsNullOrEmpty(partner.AdrIsporuke) && partner.AdrIsporuke.Length > AdrIsporukeMaxLength)
                            validationModel.AdrIsporuke = $"Maksimalna duljina polja je {AdrIsporukeMaxLength}";
                        break;
                    }
                case nameof(Partner.OIB):
                    {
                        if (string.IsNullOrEmpty(partner.OIB))
                            validationModel.OIB = "OIB je obavezno polje!";
                        else
                        {
                            if (!System.Text.RegularExpressions.Regex.IsMatch(partner.OIB, "^[0-9]*$"))
                                validationModel.OIB = "Neispravan unos.";
                            if (partner.OIB.Length != OIBObligatoryLength)
                                validationModel.OIB = $"Obavezna duljina OIB-a je {OIBObligatoryLength}";
                        }
                        break;
                    }
                case nameof(Partner.ImeOsobe):
                    {
                        if (partner.TipPartnera.Equals(Constants.OsobaTip))
                        {
                            if (string.IsNullOrEmpty(partner.ImeOsobe))
                                validationModel.ImeOsobe = "Ime osobe je obazeno polje!";
                            else if (partner.ImeOsobe.Length > ImeOsobeMaxLength)
                                validationModel.ImeOsobe = $"Maksimalna duljina polja je {ImeOsobeMaxLength}";
                        }
                        break;
                    }
                case nameof(Partner.PrezimeOsobe):
                    {
                        if (partner.TipPartnera.Equals(Constants.OsobaTip))
                        {
                            if (string.IsNullOrEmpty(partner.PrezimeOsobe))
                                validationModel.PrezimeOsobe = "Prezime osobe je obazeno polje!";
                            else if (partner.PrezimeOsobe.Length > PrezimeOsobeMaxLength)
                                validationModel.PrezimeOsobe = $"Maksimalna duljina polja je {PrezimeOsobeMaxLength}";
                        }
                        break;
                    }
                case nameof(Partner.NazivTvrtke):
                    {
                        if (partner.TipPartnera.Equals(Constants.TvrtkaTip))
                        {
                            if (string.IsNullOrEmpty(partner.NazivTvrtke))
                                validationModel.NazivTvrtke = "Naziv tvrtke je obazeno polje!";
                            else if (partner.Naziv.Length > NazivTvrtkeMaxLength)
                                validationModel.NazivTvrtke = $"Maksimalna duljina polja je {NazivTvrtkeMaxLength}";
                        }
                        break;
                    }
                case nameof(Partner.MatBrTvrtke):
                    {
                        if (partner.TipPartnera.Equals(Constants.TvrtkaTip))
                        {
                            if (string.IsNullOrEmpty(partner.MatBrTvrtke))
                                validationModel.MatBrTvrtke = "Matični broj je obavezno polje!";
                            else
                            {
                                if (!System.Text.RegularExpressions.Regex.IsMatch(partner.MatBrTvrtke, "^[0-9]*$"))
                                    validationModel.MatBrTvrtke = "Neispravan unos!";
                                if (partner.MatBrTvrtke.Length > MatBrTvrtkeMaxLength)
                                    validationModel.MatBrTvrtke = $"Maksimalna duljina polja je {MatBrTvrtkeMaxLength}";
                            }
                        }
                        break;
                    }
            }
        }

        public void ValidateModel(Partner partner, PartnerValidationModel validationModel)
        {
            ValidateProperty(partner, validationModel, nameof(Partner.IdPartnera));
            ValidateProperty(partner, validationModel, nameof(Partner.TipPartnera));
            ValidateProperty(partner, validationModel, nameof(Partner.AdrPartnera));
            ValidateProperty(partner, validationModel, nameof(Partner.AdrIsporuke));
            ValidateProperty(partner, validationModel, nameof(Partner.OIB));
            ValidateProperty(partner, validationModel, nameof(Partner.ImeOsobe));
            ValidateProperty(partner, validationModel, nameof(Partner.PrezimeOsobe));
            ValidateProperty(partner, validationModel, nameof(Partner.MatBrTvrtke));
            ValidateProperty(partner, validationModel, nameof(Partner.NazivTvrtke));
        }
        

        #region Business Rules Constants

        private const int AdrPartneraMaxLength = 50;
        private const int AdrIsporukeMaxLength = 50;

        private const int OIBObligatoryLength = 11;

        private const int ImeOsobeMaxLength = 20;
        private const int PrezimeOsobeMaxLength = 20;

        private const int NazivTvrtkeMaxLength = 50;
        private const int MatBrTvrtkeMaxLength = 30;

        #endregion

        #endregion
    }
}
