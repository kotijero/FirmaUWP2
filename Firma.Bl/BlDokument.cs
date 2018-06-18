﻿using Firma.Bl.ValidationModels;
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
    public class BlDokument : BlBase
    {
        DokumentDalProvider dokumentDalProvider = new DokumentDalProvider();

        public ResultWrapper<Dokument> Fetch(int Id)
        {
            throw new NotImplementedException();
        }

        public ResultWrapper<List<Dokument>> FetchAll()
        {
            List<Artikl> artiklList = new List<Artikl>();
            try
            {
                ArtiklDalProvider artiklDal = new ArtiklDalProvider();
                foreach (var artikl in artiklDal.FetchAll())
                {
                    artiklList.Add(new Artikl(artikl));
                }
            } catch (Exception exc)
            {
                return new ResultWrapper<List<Dokument>>(null, HandleException(exc));
            }

            return FetchAll(artiklList);
        }

        public ResultWrapper<List<Dokument>> FetchAll(List<Artikl> artiklList)
        {
            List<Dokument> dokumentList = null;
            string errorMessage = string.Empty;
            try
            {
                List<Stavka> stavkeList = new List<Stavka>();
                StavkaDalProvider stavkaDal = new StavkaDalProvider();
                foreach (var stavka in stavkaDal.FetchAll())
                {
                    stavkeList.Add(new Stavka(stavka, artiklList.FirstOrDefault(t => t.SifArtikla.Equals(stavka.SifArtikla))));
                }
                dokumentList = new List<Dokument>();
                foreach (var dokument in dokumentDalProvider.FetchAll().OrderBy(t => t.BrDokumenta))
                {
                    dokumentList.Add(new Dokument(dokument, stavkeList.Where(t => t.IdDokumenta == dokument.IdDokumenta).ToList()));
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<Dokument>>(dokumentList, errorMessage);
        }

        public ResultWrapper<Dokument> AddItem(Dokument item)
        {
            Dokument dokument = null;
            string errorMessage = null;
            try
            {
                DTO.Dokument dokumentDto = dokumentDalProvider.AddItem(item.ToDTO());
                item.IdDokumenta = dokumentDto.IdDokumenta;
                StavkaDalProvider stavkaDal = new StavkaDalProvider();
                foreach (var stavka in item.Stavke)
                {
                    stavka.IdDokumenta = item.IdDokumenta;
                    var stavkaDto = stavkaDal.AddItem(stavka.ToDTO());
                    stavka.IdStavke = stavkaDto.IdStavke;
                }
                dokument = item;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Dokument>(dokument, errorMessage);
        }

        public ResultWrapper<Dokument> UpdateItem(Dokument newItem, Dokument oldItem)
        {
            Dokument dokument = null;
            string errorMessage = string.Empty;
            try
            {
                dokumentDalProvider.UpdateItem(newItem.ToDTO());
                StavkaDalProvider stavkaDal = new StavkaDalProvider();
                List<Stavka> updatedStavkaList = new List<Stavka>();
                foreach (var newStavka in newItem.Stavke)
                {
                    var oldStavka = oldItem.Stavke.FirstOrDefault(t => t.IdStavke == newStavka.IdStavke);
                    // update
                    if (oldStavka != null)
                    {
                        stavkaDal.UpdateItem(newStavka.ToDTO());
                        oldItem.Stavke.Remove(oldStavka);
                    }
                    else // insert
                    {
                        newStavka.IdDokumenta = newItem.IdDokumenta;
                        var newStavkaDto = stavkaDal.AddItem(newStavka.ToDTO());
                        newStavka.IdStavke = newStavkaDto.IdStavke;
                    }
                }
                if (oldItem.Stavke.Count > 1)
                {
                    // delete
                    foreach (var oldStavka in oldItem.Stavke)
                    {
                        stavkaDal.DeleteItem(oldStavka.ToDTO());
                    }
                }
                dokument = newItem;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Dokument>(newItem, errorMessage);
        }

        public ResultWrapper<Dokument> DeleteItem(Dokument item)
        {
            Dokument dokument = null;
            string errorMessage = string.Empty;
            try
            {
                if (dokumentDalProvider.FetchDokumentsWithIdPrethDokumenta(item.IdDokumenta) != null)
                    errorMessage = "Nije moguće obrisati jer postoje dokumenti koji se referenciraju na ovaj.";
                else
                {
                    // stavke:
                    StavkaDalProvider stavkaDal = new StavkaDalProvider();
                    foreach (var stavka in item.Stavke)
                    {
                        stavkaDal.DeleteItem(stavka.ToDTO());
                    }
                    dokumentDalProvider.DeleteItem(item.ToDTO());
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Dokument>(dokument, errorMessage);
        }

        #region Validation

        public void ValidateProperty(Dokument dokument, DokumentValidationModel validationModel, string propertyName)
        {
            validationModel.ClearProperty(propertyName);
            switch(propertyName)
            {
                case nameof(Dokument.BrDokumenta):
                    {
                        if (dokument.BrDokumenta <= 0)
                            validationModel.BrDokumenta = "Pogrešan format broja dokumenta!";
                        break;
                    }
                case nameof(Dokument.VrDokumenta):
                    {
                        if (string.IsNullOrEmpty(dokument.VrDokumenta))
                            validationModel.VrDokumenta = "Vrsta dokumenta je obavezno polje!";
                        break;
                    }
                case nameof(Dokument.DatDokumenta):
                    {
                        break;
                    }
                case nameof(Dokument.IdPartnera):
                    {
                        if (dokument.IdPartnera < 0)
                            validationModel.IdPartnera = "Partner je obavezno polje!";
                        break;
                    }
                case nameof(Dokument.PostoPorez):
                    {
                        if (dokument.PostoPorez < 0)
                            validationModel.PostoPorez = "Neispravan porez!";
                        break;
                    }
            }
        }

        public void ValidateModel(Dokument dokument, DokumentValidationModel validationModel)
        {
            ValidateProperty(dokument, validationModel, nameof(Dokument.BrDokumenta));
            ValidateProperty(dokument, validationModel, nameof(Dokument.VrDokumenta));
            ValidateProperty(dokument, validationModel, nameof(Dokument.DatDokumenta));
            ValidateProperty(dokument, validationModel, nameof(Dokument.IdPartnera));
            ValidateProperty(dokument, validationModel, nameof(Dokument.PostoPorez));
        }

        public void ValidateStavkaProperty(Stavka stavka, StavkaValidationModel validationModel, string propertyName)
        {
            validationModel.ClearProperty(propertyName);
            switch (propertyName)
            {
                case nameof(Stavka.JedCijArtikla):
                    {
                        if (stavka.JedCijArtikla < 0)
                            validationModel.JedCijArtikla = "Neispravan unos.";
                        break;
                    }
                case nameof(Stavka.KolArtikla):
                    {
                        if (stavka.KolArtikla <= 0)
                            validationModel.KolArtikla = "Neispravan unos.";
                        break;
                    }
                case nameof(Stavka.PostoRabat):
                    {
                        if (stavka.PostoRabat < 0)
                            validationModel.PostoRabat = "Neispravan unos.";
                        break;
                    }
            }
        }

        public void ValidateStavka(Stavka stavka, StavkaValidationModel validationModel)
        {
            ValidateStavkaProperty(stavka, validationModel, nameof(Stavka.JedCijArtikla));
            ValidateStavkaProperty(stavka, validationModel, nameof(Stavka.KolArtikla));
            ValidateStavkaProperty(stavka, validationModel, nameof(Stavka.PostoRabat));
        }

        #endregion
    }
}
