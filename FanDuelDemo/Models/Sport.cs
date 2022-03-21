using FanDuelDemo.Middleware;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using FanDuelDemo.Services;

namespace FanDuelDemo.Models
{
    public class Sport : AbstractValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
         CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();
            var dbContext = validationContext.GetService<ICommonService>();

            if (await dbContext.IsSportAlreadyExist(Name))
            {
                errors.Add(new ValidationResult("Sport Already exists. Please choose a different sport", new[] { nameof(Name) }));
            }

            return errors;
        }
    }
}
