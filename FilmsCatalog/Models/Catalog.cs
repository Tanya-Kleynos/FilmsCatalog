using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class Catalog
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public string CreatorId { get; set; }

        public User Creator { get; set; }

        public DateTime Date { get; set; }

        public ICollection<CatalogFilm> Films { get; set; }
    }
}
