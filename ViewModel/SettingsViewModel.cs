using Firma.Bl;
using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace ViewModel
{
    public class SettingsViewModel : ObservableModel
    {
        public SettingsViewModel()
        {
            connectionString = Settings.ConnectionString;
            changed = false;
            _korisnik = new Korisnik();
        }
        
        #region Properties

        private string connectionString;

        private Korisnik _korisnik;

        public Korisnik Korisnik
        {
            set
            {
                _korisnik = value;
                OnPropertyChanged(nameof(IsAdmin));
                OnPropertyChanged(nameof(Username));
            }
        }

        public string ConnectionString
        {
            get { return connectionString; }
            set
            {
                connectionString = value;
                Changed = true;
                OnPropertyChanged();
            }
        }

        private bool changed;

        public bool Changed
        {
            get { return changed; }
            set
            {
                changed = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get { return _korisnik.IsAdmin; }
        }

        public string Username { get { return _korisnik.Username; } }

        private string oldPassword;
        private string newPassword;
        private string newPassword2;

        public string OldPassword
        {
            get { return oldPassword; }
            set
            {
                oldPassword = value;
                OnPropertyChanged();
            }
        }

        public string NewPassword
        {
            get { return newPassword; }
            set
            {
                newPassword = value;
                OnPropertyChanged();
            }
        }

        public string NewPassword2
        {
            get { return newPassword2; }
            set
            {
                newPassword2 = value;
                OnPropertyChanged();
            }
        }

        private string passwordStatusMessage;
        public string PasswordStatusMessage
        {
            get { return passwordStatusMessage; }
            set
            {
                passwordStatusMessage = value;
                OnPropertyChanged();
            }
        }

        private bool isPositiveMessage;
        public bool IsPositiveMessage
        {
            get { return isPositiveMessage; }
            set
            {
                isPositiveMessage = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Actions

        public void Save()
        {
            Settings.ConnectionString = connectionString;
        }

        public async Task<bool> CheckChangesStatus()
        {
            if (changed)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Promjene detektirane",
                    Content = "Želite li spremiti detektirane promjene?",
                    PrimaryButtonText = "Spremi",
                    SecondaryButtonText = "Odbaci promjene",
                    CloseButtonText = "Natrag"
                };
                var res = await dialog.ShowAsync();
                if (res == ContentDialogResult.Primary)
                {
                    Save();
                    return true;
                } else if (res == ContentDialogResult.Secondary)
                {
                    return true;
                } else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public void SavePasswordChanges()
        {
            if (string.IsNullOrEmpty(oldPassword))
            {
                PasswordStatusMessage = "Potrebno je unijeti staru lozinku.";
                IsPositiveMessage = false;
                return;
            }
            else if (string.IsNullOrEmpty(newPassword))
            {
                PasswordStatusMessage = "Potrebno je unijeti novu lozinku.";
                IsPositiveMessage = false;
                return;
            }
            else if (string.IsNullOrEmpty(newPassword2))
            {
                PasswordStatusMessage = "Potrebno ponoviti novu lozinku.";
                IsPositiveMessage = false;
                return;
            }
            else if (!newPassword.Equals(newPassword2))
            {
                passwordStatusMessage = "Nove lozinke nisu jednake.";
                IsPositiveMessage = false;
                return;
            }
            else if (_korisnik.Password.Equals(oldPassword))
            {
                BlKorisnik blKorisnik = new BlKorisnik();
                _korisnik.Password = newPassword;
                try
                {
                    blKorisnik.UpdateItem(_korisnik);
                } catch 
                {
                    PasswordStatusMessage = "Greška prilikom unosa unosa. Kontaktirajte administratora.";
                    IsPositiveMessage = false;
                    _korisnik.Password = oldPassword;
                    return;
                }
                PasswordStatusMessage = "Promjene unesene.";
                IsPositiveMessage = true;
                OldPassword = string.Empty;
                NewPassword = string.Empty;
                NewPassword2 = string.Empty;
            }
            else
            {
                PasswordStatusMessage = "Pogrešna stara lozinka.";
                IsPositiveMessage = false;
            }          
        }

        #endregion
    }
}
