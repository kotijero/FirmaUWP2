﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firma.DTO
{
    public class Partner
    {
        public Partner()
        {
            this.Dokument = new HashSet<Dokument>();
        }

        public int IdPartnera { get; set; }
        public string TipPartnera { get; set; }
        public Nullable<int> IdMjestaPartnera { get; set; }
        public string AdrPartnera { get; set; }
        public Nullable<int> IdMjestaIsporuke { get; set; }
        public string AdrIsporuke { get; set; }
        public string OIB { get; set; }

        public virtual ICollection<Dokument> Dokument { get; set; }
        public virtual Mjesto MjestoIsporuke { get; set; }
        public virtual Mjesto MjestoSjedista { get; set; }
    }
}
