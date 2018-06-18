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
    public class SignInViewModel : ObservableModel
    {
        public Korisnik Korisnik;
        public SignInViewModel()
        {
            Korisnik = null;
        }

        private string username;
        private string password;

        private string usernameErrorMessage;
        private string passwordErrorMessage;
        private string generalErrorMessage;

        private bool loading;

        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged();
            }
        }

        public string UsernameErrorMessage
        {
            get { return usernameErrorMessage; }
            set
            {
                usernameErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public string PasswordErrorMessage
        {
            get { return passwordErrorMessage; }
            set
            {
                passwordErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public string GeneralErrorMessage
        {
            get { return generalErrorMessage; }
            set
            {
                generalErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public bool Loading
        {
            get { return loading; }
            set
            {
                loading = value;
                OnPropertyChanged();
            }
        }

        public async Task<bool> Login()
        {
            
            if (string.IsNullOrEmpty(username))
            {
                UsernameErrorMessage = "Korisničko ime je obavezno!";
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                PasswordErrorMessage = "Lozinka je obavezna!";
                return false;
            }
            Loading = true;
            BlKorisnik blKorisnik = new BlKorisnik();
            var response = await Task.Run(() => blKorisnik.Login(username, password));
            if (!string.IsNullOrEmpty(response.ErrorMessage))
            {
                GeneralErrorMessage = response.ErrorMessage;
                return false;
            }
            Korisnik = response.Value;
            Loading = false;
            if (Korisnik == null)
            {
                GeneralErrorMessage = "Pogrešno kosiničko ime ili lozinka.";
                return false;
            }
            else return true;
        }
    }
}
