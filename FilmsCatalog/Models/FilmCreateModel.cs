using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FilmsCatalog.Models
{
    public class FilmCreateModel
    {
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Название")]
        [MaxLength(200)]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Год выпуска")]
        public Int32 Year { get; set; }

        [Display(Name = "Режиссёр")]
        public string Director { get; set; }

        [Display(Name = "Постер")]
        public IFormFile Poster { get; set; }
    }
}
