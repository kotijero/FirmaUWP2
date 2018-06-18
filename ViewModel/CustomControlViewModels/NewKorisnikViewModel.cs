using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel.CustomControlViewModels
{
    public class NewKorisnikViewModel : ObservableModel
    {
        public Korisnik Korisnik { get; set; }

        private string username;
        private string password1;
        private string password2;
        private bool isAdmin;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password1
        {
            get { return password1; }
            set
            {
                password1 = value;
                OnPropertyChanged();
            }
        }

        public string Password2
        {
            get { return password2; }
            set
            {
                password2 = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool AddUser()
        {
            if (string.IsNullOrEmpty(username))
            {
                ErrorMessage = "Potrebno unijeti korisničko ime";
            }
            else if (string.IsNullOrEmpty(password1))
            {
                ErrorMessage = "Potrebno unijeti lozinku.";
            }
            else if (string.IsNullOrEmpty(password2))
            {
                ErrorMessage = "Potrebno ponoviti lozinku.";
            }
            else if (!password1.Equals(password2))
            {
                ErrorMessage = "Lozinke nisu jednake.";
            } else
            {
                BlKorisnik blKorisnik = new BlKorisnik();
                try
                {
                    Korisnik korisnik = new Korisnik
                    {
                        Username = username,
                        Password = password1,
                        IsAdmin = isAdmin
                    };
                    var response = blKorisnik.AddItem(korisnik);
                    if (string.IsNullOrEmpty(response.ErrorMessage))
                    {
                        Korisnik = korisnik;
                        return true;
                    }
                    else
                    {
                        ErrorMessage = response.ErrorMessage;
                        return false;
                    }
                } catch
                {
                    ErrorMessage = "Pogreška unosa u bazu.";
                    return false;
                }
            }
            return false;
        }
    }
}
