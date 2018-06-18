using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Helpers
{
    public abstract class ValidationModelBase : ObservableModel
    {
        public ValidationModelBase()
        {
            ClearErrors(false);
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

        public abstract void ClearErrors(bool notify);
    }
}
