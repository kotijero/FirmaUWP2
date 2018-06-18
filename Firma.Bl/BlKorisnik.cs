using Firma.DAL;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl
{
    public class BlKorisnik : BlBase
    {
        /// <summary>
        /// Provjeri postoji li korisnik sa danim podacima u bazi. Ako postoji vrati korisnika, ako ne postoji
        /// vrati <see langword="null"/>.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public ResultWrapper<Korisnik> Login(string username, string password)
        {
            Korisnik korisnik = null;
            string errorMessage = string.Empty;
            try
            {
                KorisnikDalProvider dalProvider = new KorisnikDalProvider();
                DTO.Korisnik dbKorisnik = dalProvider.Fetch(username);
                
                if (dbKorisnik != null && dbKorisnik.Password.Equals(password))
                {
                    korisnik = new Korisnik(dbKorisnik);
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Korisnik>(korisnik, errorMessage);
        }

        public ResultWrapper<List<Korisnik>> FetchAll()
        {
            List<Korisnik> korisnikList = null;
            string errorMessage = string.Empty;
            try
            {
                KorisnikDalProvider dalProvider = new KorisnikDalProvider();
                List<DTO.Korisnik> korisnikDTOList = dalProvider.FetchAll();
                korisnikList = new List<Korisnik>();
                foreach (var korisnik in korisnikDTOList)
                {
                    korisnikList.Add(new Korisnik(korisnik));
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<List<Korisnik>>(korisnikList, errorMessage);
        }

        public string UpdateItem(Korisnik korisnik, string oldUsername = null)
        {
            KorisnikDalProvider dalProvider = new KorisnikDalProvider();

            if (dalProvider.Fetch(string.IsNullOrEmpty(oldUsername) ? korisnik.Username : oldUsername) == null)
                return "Korisnik sa ovim korisničkim imenom ne postoji.";

            dalProvider.UpdateItem(korisnik.ToDTO(), oldUsername);
            return string.Empty;
        }

        public string UpdateItemWithoutPassword(Korisnik korisnik, string oldUsername = null)
        {
            KorisnikDalProvider dalProvider = new KorisnikDalProvider();

            if (dalProvider.Fetch(string.IsNullOrEmpty(oldUsername) ? korisnik.Username : oldUsername) == null)
                return "Korisnik sa ovim korisničkim imenom ne postoji.";
            
            dalProvider.UpdateItemWithoutPassword(korisnik.ToDTO(), oldUsername);
            return string.Empty;
        }

        public ResultWrapper<Korisnik> AddItem(Korisnik item)
        {
            Korisnik korisnik = null;
            string errorMessage = string.Empty;
            try
            {
                KorisnikDalProvider dalProvider = new KorisnikDalProvider();
                if (dalProvider.Fetch(item.Username) != null) errorMessage = "Već postoji korisnik sa ovim korisničkim imenom.";
                else
                {
                    dalProvider.AddItem(item.ToDTO());
                    korisnik = item;
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);   
            }
            return new ResultWrapper<Korisnik>(korisnik, errorMessage);
        }

        public ResultWrapper<Korisnik> DeleteItem(Korisnik item, Korisnik currentKorisnik)
        {
            Korisnik korisnik = null;
            string errorMessage = string.Empty;
            try
            {
                if (currentKorisnik.Username.Equals(item.Username)) errorMessage = "Nije moguće obrisati trenutno prijavljenog korinika.";
                else
                {
                    KorisnikDalProvider dalProvider = new KorisnikDalProvider();
                    dalProvider.DeleteItem(item.ToDTO());
                    korisnik = item;
                }
            }
            catch (Exception exc)
            {
                errorMessage = HandleException(exc);
            }
            return new ResultWrapper<Korisnik>(korisnik, errorMessage);
        }
    }
}
