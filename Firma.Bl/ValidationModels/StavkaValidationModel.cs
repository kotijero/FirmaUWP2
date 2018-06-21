using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public class StavkaValidationModel : ValidationModelBase
    {
        public StavkaValidationModel()
            : base()
        {
        }

        public override void ClearErrors(bool notify)
        {
            kolArtikla = string.Empty;
            jedCijArtikla = string.Empty;
            postoRabat = string.Empty;

            if (notify)
            {
                OnPropertyChanged(nameof(KolArtikla));
                OnPropertyChanged(nameof(JedCijArtikla));
                OnPropertyChanged(nameof(PostoRabat));
            }
        }

        public void ClearProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(KolArtikla):
                    kolArtikla = string.Empty;
                    break;
                case nameof(JedCijArtikla):
                    jedCijArtikla = string.Empty;
                    break;
                case nameof(PostoRabat):
                    postoRabat = string.Empty;
                    break;
                default:
                    return;
            }
            OnPropertyChanged(propertyName);
        }

        #region Base

        public override bool CheckDirty()
        {
            if (!string.IsNullOrEmpty(kolArtikla)
                || !string.IsNullOrEmpty(jedCijArtikla)
                || !string.IsNullOrEmpty(postoRabat))
                IsDirty = true;
            else
                IsDirty = false;
            return isDirty;
        }

        #endregion

        #region Private

        private string kolArtikla;
        private string jedCijArtikla;
        private string postoRabat;

        #endregion

        #region Public 

        public string KolArtikla
        {
            get { return kolArtikla; }
            set
            {
                if (string.IsNullOrEmpty(kolArtikla))
                    kolArtikla = value;
                else
                    kolArtikla += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string JedCijArtikla
        {
            get { return jedCijArtikla; }
            set
            {
                if (string.IsNullOrEmpty(jedCijArtikla))
                    jedCijArtikla = value;
                else
                    jedCijArtikla += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string PostoRabat
        {
            get { return postoRabat; }
            set
            {
                if (string.IsNullOrEmpty(postoRabat))
                    postoRabat = value;
                else
                    postoRabat += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
