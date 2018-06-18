using Firma.Helpers;
using Firma.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModel
{
    public class MainPageViewModel : ObservableModel
    {
        public MainPageViewModel()
        {
            if (korisnik == null)
            {
                Korisnik = new Korisnik();
                signedIn = false;
            }
        }

        private static bool signedIn;
        private static Korisnik korisnik;
        public Korisnik Korisnik
        {
            get { return korisnik; }
            set
            {
                korisnik = value;
                OnPropertyChanged();
            }
        }

        public bool SignedIn
        {
            get { return signedIn; }
            set
            {
                signedIn = value;
                OnPropertyChanged();
            }
        }
    }
}
