using Firma.Bl.ValidationModels;
using Firma.DAL;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class BlDrzava : BlBase
    {
        public ResultWrapper<List<Drzava>> FetchAll()
        {
            List<Drzava> drzavaList = null;
            string errorMessage = string.Empty;
            try
            {
                // fetch mjesta:
                MjestoDalProvider mjestoDal = new MjestoDalProvider();
                List<DTO.Mjesto> mjestoDTOList = mjestoDal.FetchAll();

                // fetch drzave:
                DrzavaDalProvider drzavaDal = new DrzavaDalProvider();
                List<DTO.Drzava> drzavaDTOList = drzavaDal.FetchAll();

                drzavaList = new List<Drzava>();
                foreach (var drzava in drzavaDTOList)
                {
                    drzavaList.Add(new Drzava(drzava, mjestoDTOList.Where(t => t.OznDrzave.Equals(drzava.SifDrzave)).ToList()));
                }
            } catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<Drzava>>(drzavaList, errorMessage);
        }

        public ResultWrapper<Drzava> AddItem(Drzava item)
        {
            Drzava drzava = null;
            string errorMessage = string.Empty;
            try
            {
                DrzavaDalProvider drzavaDal = new DrzavaDalProvider();
                if (drzavaDal.Fetch(item.OznDrzave) != null)
                {
                    errorMessage = "Već postoji država sa ovom oznakom.";
                }
                else
                {
                    drzavaDal.AddItem(item.ToDTO());
                }
                drzava = item;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Drzava>(drzava, errorMessage);
        }

        public ResultWrapper<Drzava> UpdateItem(Drzava item, string oldOznDrzave = null)
        {
            Drzava drzava = null;
            string errorMessage = string.Empty;
            try
            {
                DrzavaDalProvider drzavaDal = new DrzavaDalProvider();
                if (oldOznDrzave != null && drzavaDal.Fetch(item.OznDrzave) != null)
                {
                    errorMessage = "Već postoji država sa ovom oznakom!";
                }
                else
                {
                    drzavaDal.UpdateItem(item.ToDTO(), string.IsNullOrEmpty(oldOznDrzave) ? item.OznDrzave : oldOznDrzave);
                }
                
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Drzava>(drzava, errorMessage);
        }

        public ResultWrapper<Drzava> DeleteItem(Drzava item)
        {
            Drzava drzava = null;
            string errorMessage = string.Empty;
            try
            {
                MjestoDalProvider mjestoDal = new MjestoDalProvider();
                if (mjestoDal.CheckMjestoForDrzava(item.OznDrzave))
                {
                    errorMessage = "Nije moguće obrisati jer u bazi postoje mjesta u ovoj državi.";
                }
                else
                {
                    DrzavaDalProvider drzavaDal = new DrzavaDalProvider();
                    drzavaDal.DeleteItem(item.ToDTO());
                }
                drzava = item;
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Drzava>(drzava, errorMessage);
        }

        public void ValidateProperty(Drzava drzava, string propertyName, DrzavaValidationModel vaidationModel)
        {
            // nije implementirano
        }
    }
}
