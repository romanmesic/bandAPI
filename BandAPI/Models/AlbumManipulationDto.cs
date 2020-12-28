using BandAPI.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Models
{
    [TitleAndDescription(ErrorMessage = "Title must be different")]
    public abstract class AlbumManipulationDto
    {
        [Required(ErrorMessage = "Moras naslov dodat")]
        [MaxLength(200, ErrorMessage = "Title needs to be up to 200 characters")]
        public string Title { get; set; }

        
        [MaxLength(400, ErrorMessage = "Description needs to be up to 400 characters")]
        public virtual string Description { get; set; }
    }
}
