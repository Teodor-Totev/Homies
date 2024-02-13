namespace Homies.Data.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using Microsoft.AspNetCore.Identity;

    public class EventParticipant
    {
        public string HelperId { get; set; } = null!;

        [ForeignKey("HelperId")]
        public IdentityUser Helper { get; set; } = null!;

        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; } = null!;
    }
}
