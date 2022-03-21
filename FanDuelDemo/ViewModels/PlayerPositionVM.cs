using FanDuelDemo.Middleware;
using FanDuelDemo.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FanDuelDemo.ViewModels
{
    public class PlayerPositionVM : AbstractValidatableObject
    {
        public int SportId { get; set; }
        public int TeamId { get; set; }
        public string PositionName { get; set; }
        public int PlayerId { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Only whole number is allowed for Position Depth")]
        public int? PositionDepth { get; set; }

        public override async Task<IEnumerable<ValidationResult>> ValidateAsync(ValidationContext validationContext,
         CancellationToken cancellation)
        {
            var errors = new List<ValidationResult>();
            var dbContext = validationContext.GetService<IPlayerPositionsService>();

            if (!await dbContext.IsSportExist(SportId))
            {
                errors.Add(new ValidationResult("Sport not found. Please add a sport and try again.", new[] { nameof(SportId) }));
            }

            if (!await dbContext.IsTeamExist(SportId, TeamId))
            {
                errors.Add(new ValidationResult("Team not found. Please add a Team to a corresponding Sport and try again.", new[] { nameof(TeamId) }));
            }

            if (!await dbContext.IsPlayerExist(SportId, TeamId, PlayerId))
            {
                errors.Add(new ValidationResult("Player not found. Please add a Player to the corresponding Sport & Team and try again.", new[] { nameof(PlayerId) }));
            }

            if (await dbContext.IsPlayerAlreadyExistsInPosition((PlayerPositionVM)validationContext.ObjectInstance))
            {
                errors.Add(new ValidationResult("Player already exists for the same position. " +
                    "Please choose a different position and try again.", new[] { nameof(PlayerId) }));
            }

            if(await dbContext.IsPositionDepthAlreadyTaken((PlayerPositionVM)validationContext.ObjectInstance))
            {
                errors.Add(new ValidationResult("Position Depth has already been to another player. " +
                    "Please choose a different position depth and try again.", new[] { nameof(PositionDepth) }));
            }



            return errors;
        }
    }
}
