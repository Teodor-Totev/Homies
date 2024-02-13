namespace Homies.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;

    using static Homies.Common.EntityValidationConstants.EventConstants;
    //using Type = Homies.Data.Models.Type;

    public class Event
    {
        public Event()
        {
            this.EventsParticipants = new HashSet<EventParticipant>();
			this.CreatedOn = DateTime.UtcNow;
		}
         
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string OrganiserId { get; set; } = null!;

        public IdentityUser Organiser { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int TypeId { get; set; }

        [ForeignKey("TypeId")]
        public Type Type { get; set; } = null!;

        public ICollection<EventParticipant> EventsParticipants { get; set; }
    }
}
