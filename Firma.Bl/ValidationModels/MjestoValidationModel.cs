using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public class MjestoValidationModel : ValidationModelBase
    {
        public MjestoValidationModel()
        {
        }

        #region Private properties

        private string idMjesta;
        private string oznDrzave;
        private string nazMjesta;
        private string postBrMjesta;
        private string postNazMjesta;

        #endregion


        #region Public properties

        public string IdMjesta
        {
            get { return idMjesta; }
            set
            {
                idMjesta += $"\n{value}";
            }
        }
        public string OznDrzave
        {
            get { return oznDrzave; }
            set
            {
                oznDrzave += $"\n{value}";
            }
        }
        public string NazMjesta
        {
            get { return nazMjesta; }
            set
            {
                nazMjesta += $"\n{value}";
            }
        }
        public string PostBrMjesta
        {
            get { return postBrMjesta; }
            set
            {
                postBrMjesta += $"\n{value}";
            }
        }
        public string PostNazMjesta
        {
            get { return postNazMjesta; }
            set { postNazMjesta += $"\n{value}"; }
        }

        #endregion

        public override bool CheckDirty()
        {
            if (!string.IsNullOrEmpty(IdMjesta)
                || !string.IsNullOrEmpty(OznDrzave)
                || !string.IsNullOrEmpty(NazMjesta)
                || !string.IsNullOrEmpty(PostBrMjesta)
                || !string.IsNullOrEmpty(PostNazMjesta)) return true;
            else
                return false;
        }

        public void ClearProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdMjesta):
                    idMjesta = string.Empty;
                    break;
                case nameof(OznDrzave):
                    oznDrzave = string.Empty;
                    break;
                case nameof(NazMjesta):
                    nazMjesta = string.Empty;
                    break;
                case nameof(PostBrMjesta):
                    postBrMjesta = string.Empty;
                    break;
                case nameof(PostNazMjesta):
                    postNazMjesta = string.Empty;
                    break;
                default:    // Ovaj default postoji za slucaj da se pozove
                    return; // za nepostojeci property da se ne poziva OnPropertyChanged().
            }
            OnPropertyChanged(propertyName);
        }

        public override void ClearErrors(bool notify)
        {
            idMjesta = string.Empty;
            oznDrzave = string.Empty;
            nazMjesta = string.Empty;
            postBrMjesta = string.Empty;
            postNazMjesta = string.Empty;
            if (notify)
            {
                OnPropertyChanged(nameof(IdMjesta));
                OnPropertyChanged(nameof(OznDrzave));
                OnPropertyChanged(nameof(NazMjesta));
                OnPropertyChanged(nameof(PostBrMjesta));
                OnPropertyChanged(nameof(PostNazMjesta));
            }
        }
    }
}
