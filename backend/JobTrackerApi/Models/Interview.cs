using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobTrackerApi.Models
{
    public enum InterviewOutcome
    {
        Pass,
        Fail,
        Pending
    }

    public class Interview
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ApplicationId { get; set; }

        [ForeignKey("ApplicationId")]
        public Application Application { get; set; }

        [Required]
        public string RoundName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public InterviewOutcome Outcome { get; set; }

        public string Notes { get; set; }
    }
}
