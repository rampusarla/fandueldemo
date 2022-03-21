using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using FanDuelDemo.Middleware;
using FanDuelDemo.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FanDuelDemo.Models
{
    public class Team : AbstractValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        
        [Required]
        public int SportId { get; set; }
        [ForeignKey("SportId")]
        public Sport Sport { get; set; }

        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
         CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();
            var dbContext = validationContext.GetService<ICommonService>();

            if(await dbContext.SportsCount() == 0) {
                errors.Add(new ValidationResult("There are no sports available. Please add atleast one new sport and try again", new[] { nameof(Name) }));
            }

            if (await dbContext.IsTeamAlreadyExist(SportId, Name))
            {
                errors.Add(new ValidationResult("Team Already exists. Please choose a different team", new[] { nameof(Name) }));
            }

            return errors;
        }
    }
}
