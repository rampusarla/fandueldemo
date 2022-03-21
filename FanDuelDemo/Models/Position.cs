using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FanDuelDemo.Models
{
    public class Position
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PositionName { get; set; }
        [Required]
        public int PositionDepth { get; set; }

        [Required]
        public int PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        [Required]
        public int SportId { get; set; }
        [ForeignKey("SportId")]
        public Sport Sport { get; set; }

        [Required]
        public int TeamId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
    }
}
