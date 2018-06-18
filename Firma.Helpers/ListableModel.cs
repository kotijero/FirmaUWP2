using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.Helpers
{
    public abstract class ListableModel : ObservableModel
    {
        public abstract string Title { get; }
        public abstract string Subtitle { get; }
        public abstract string Subsubtitle { get; }
    }
}
