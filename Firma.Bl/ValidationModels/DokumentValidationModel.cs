using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public class DokumentValidationModel : ValidationModelBase
    {
        public DokumentValidationModel()
            : base()
        {

        }

        public override void ClearErrors(bool notify)
        {
            idDokumenta = string.Empty;
            vrDokumenta = string.Empty;
            brDokumenta = string.Empty;
            datDokumenta = string.Empty;
            idPartnera = string.Empty;
            postoPorez = string.Empty;
            iznosDokumenta = string.Empty;

            if (notify)
            {
                OnPropertyChanged(nameof(IdDokumenta));
                OnPropertyChanged(nameof(VrDokumenta));
                OnPropertyChanged(nameof(BrDokumenta));
                OnPropertyChanged(nameof(DatDokumenta));
                OnPropertyChanged(nameof(IdPartnera));
                OnPropertyChanged(nameof(PostoPorez));
                OnPropertyChanged(nameof(IznosDokumenta));
            }
        }

        public void ClearProperty(string propertyName)
        {
            switch (propertyName)
            {
                case nameof(IdDokumenta):
                    idDokumenta = string.Empty;
                    break;
                case nameof(VrDokumenta):
                    vrDokumenta = string.Empty;
                    break;
                case nameof(BrDokumenta):
                    brDokumenta = string.Empty;
                    break;
                case nameof(DatDokumenta):
                    datDokumenta = string.Empty;
                    break;
                case nameof(IdPartnera):
                    idPartnera = string.Empty;
                    break;
                case nameof(PostoPorez):
                    postoPorez = string.Empty;
                    break;
                case nameof(IznosDokumenta):
                    iznosDokumenta = string.Empty;
                    break;
                default:
                    return;
            }
            OnPropertyChanged(propertyName);
        }

        #region Base

        public override bool CheckDirty()
        {
            if (!string.IsNullOrEmpty(idDokumenta)
                || !string.IsNullOrEmpty(vrDokumenta)
                || !string.IsNullOrEmpty(brDokumenta)
                || !string.IsNullOrEmpty(datDokumenta)
                || !string.IsNullOrEmpty(idPartnera)
                || !string.IsNullOrEmpty(postoPorez)
                || !string.IsNullOrEmpty(iznosDokumenta))
                IsDirty = true;
            else
                IsDirty = false;
            return isDirty;
        }

        #endregion

        #region Private

        private string idDokumenta;
        private string vrDokumenta;
        private string brDokumenta;
        private string datDokumenta;
        private string idPartnera;
        private string postoPorez;
        private string iznosDokumenta;

        #endregion

        #region Public

        public string IdDokumenta
        {
            get { return idDokumenta; }
            set
            {
                if (string.IsNullOrEmpty(idDokumenta))
                    idDokumenta = value;
                else
                    idDokumenta += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string VrDokumenta
        {
            get { return vrDokumenta; }
            set
            {
                if (string.IsNullOrEmpty(vrDokumenta))
                    vrDokumenta = value;
                else
                    vrDokumenta += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string BrDokumenta
        {
            get { return brDokumenta; }
            set
            {
                if (string.IsNullOrEmpty(brDokumenta))
                    brDokumenta = value;
                else
                    brDokumenta += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string DatDokumenta
        {
            get { return datDokumenta; }
            set
            {
                if (string.IsNullOrEmpty(datDokumenta))
                    datDokumenta = value;
                else
                    datDokumenta += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string IdPartnera
        {
            get { return idPartnera; }
            set
            {
                if (string.IsNullOrEmpty(idPartnera))
                    idPartnera = value;
                else
                    idPartnera += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string PostoPorez
        {
            get { return postoPorez; }
            set
            {
                if (string.IsNullOrEmpty(postoPorez))
                    postoPorez = value;
                else
                    postoPorez += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        public string IznosDokumenta
        {
            get { return iznosDokumenta; }
            set
            {
                if (string.IsNullOrEmpty(iznosDokumenta))
                    iznosDokumenta = value;
                else
                    iznosDokumenta += Environment.NewLine + value;
                CheckDirty();
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
