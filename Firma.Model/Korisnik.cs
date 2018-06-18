using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Model
{
    public class Korisnik : ObservableModel
    {
        public Korisnik()
        {
            username = string.Empty;
            password = string.Empty;
            isAdmin = false;
        }

        public Korisnik(DTO.Korisnik korisnik)
        {
            username = korisnik.Username;
            password = korisnik.Password;
            isAdmin = korisnik.IsAdmin;
        }

        private string username;
        private string password;
        private bool isAdmin;

        public string Username
        {
            get { return username; }
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged();
                }
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

        public bool IsAdmin
        {
            get { return isAdmin; }
            set
            {
                isAdmin = value;
                OnPropertyChanged();
            }
        }

        public DTO.Korisnik ToDTO()
        {
            return new DTO.Korisnik
            {
                Username = username,
                Password = password,
                IsAdmin = isAdmin
            };
        }
    }
}
