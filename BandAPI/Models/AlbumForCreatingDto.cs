using BandAPI.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BandAPI.Models
{
    [TitleAndDescription(ErrorMessage ="Title must be different")]
    public class AlbumForCreatingDto //: IValidatableObject
    {




        [Required (ErrorMessage ="Moras naslov dodat")]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(400)]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description) 
        //    {

        //        yield return new ValidationResult("The title and the description need to be different", new[] { "AlbumForCreatingDto" });
            
        //    }
        //}
    }
}
