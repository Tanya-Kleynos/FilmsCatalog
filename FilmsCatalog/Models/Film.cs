using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class Film
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string CreatorId { get; set; }

        public User Creator { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Int32 Year { get; set; }

        public string Director { get; set; }

        public String PosterPath { get; set; }

        public ICollection<CatalogFilm> Catalogs { get; set; } 
    }
}
