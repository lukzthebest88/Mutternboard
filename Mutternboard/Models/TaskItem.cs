using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mutterboard.Models
{
    public enum TaskPriority
    {
        Niedrig,
        Mittel,
        Hoch
    }

    public class TaskItem
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Titel ist erforderlich.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Beschreibung ist erforderlich.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Startdatum ist erforderlich.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Enddatum ist erforderlich.")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Priorität ist erforderlich.")]
        public string Priority { get; set; }
        public string UserId { get; set; }  // <- UserId als Fremdschlüssel
        public ApplicationUser? User { get; set; }  // <- Beziehung zu Identity User
    }

}
