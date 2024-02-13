using System.ComponentModel.DataAnnotations;

namespace Homies.Models
{
	public class JoinedEventVM
	{
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Start date and time are required")]
		public string Start { get; set; }

		[Required(ErrorMessage = "Type is required")]
		public string Type { get; set; }

        public string Organiser { get; set; }
    }
}
