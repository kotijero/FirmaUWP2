using Firma.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Bl.ValidationModels
{
    public abstract class ValidationModelBase : ObservableModel
    {
        public ValidationModelBase()
        {
            isDirty = false;
        }

        protected bool isDirty;

        public bool IsDirty
        {
            get { return isDirty; }
            set
            {
                isDirty = value;
                OnPropertyChanged();
            }
        }

        public abstract bool CheckDirty();
    }
}
