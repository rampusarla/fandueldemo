using FanDuelDemo.Middleware;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using FanDuelDemo.Services;

namespace FanDuelDemo.Models
{
    public class Player : AbstractValidatableObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        public int SportId { get; set; }
        [ForeignKey("SportId")]
        public Sport Sport { get; set; }

        [Required]
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }

        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
         CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();
            var dbContext = validationContext.GetService<ICommonService>();

            if(await dbContext.SportsCount() == 0)
            {
                errors.Add(new ValidationResult("There are no sports available. Please add atleast one new sport and try again", new[] { nameof(Name) }));
            }

            if(await dbContext.TeamsCount() == 0)
            {
                errors.Add(new ValidationResult("There are no Teams available. Please add atleast one new team and try again", new[] { nameof(Name) }));
            }

            if(!await dbContext.IsValidSport(SportId))
            {
                errors.Add(new ValidationResult("Invalid Sport Id provided. Please enter a valid sport Id and try again", new[] { nameof(SportId) }));
            }

            if (!await dbContext.IsValidTeam(TeamId))
            {
                errors.Add(new ValidationResult("Invalid Team Id provided. Please enter a valid team Id and try again", new[] { nameof(TeamId) }));
            }

            if (await dbContext.IsPlayerNumberNotUnique(SportId, TeamId, Name, Number))
            {
                errors.Add(new ValidationResult("Player Number is not unique. Please choose a different player number", new[] { nameof(Number) }));
            }

            return errors;
        }
    }
}
