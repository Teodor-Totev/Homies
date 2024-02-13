using System.ComponentModel.DataAnnotations;

namespace Homies.Models
{
    public class EventFormVM
    {
        public EventFormVM()
        {
            this.Types = new HashSet<EventType>();
        }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Start date and time are required")]
        [DataType(DataType.DateTime)]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "End date and time are required")]
        [DataType(DataType.DateTime)]
        public DateTime End { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public int TypeId { get; set; }

        // Property to hold the list of available types
        public IEnumerable<EventType> Types { get; set; }
    }

    public class EventType
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
