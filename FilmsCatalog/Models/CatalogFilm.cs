using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmsCatalog.Models
{
    public class CatalogFilm
    {
        public Guid FilmId { get; set; }

        public Film Film { get; set; }

        public Guid CatalogId { get; set; }

        public Catalog Catalog { get; set; }
    }
}
