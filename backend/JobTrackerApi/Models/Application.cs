using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobTrackerApi.Models
{
    public enum JobType
    {
        Remote,
        Onsite,
        Hybrid
    }

    public enum ApplicationStatus
    {
        Saved,
        Applied,
        Interviewing,
        Offered,
        Rejected
    }

    public class Application
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [ForeignKey("CompanyId")]
        public Company Company { get; set; }

        [Required]
        public string JobTitle { get; set; }

        public string JobLocation { get; set; }

        [Required]
        public JobType JobType { get; set; }

        public string JobLink { get; set; }

        [Required]
        public DateTime ApplicationDate { get; set; }

        [Required]
        public ApplicationStatus Status { get; set; }

        public DateTime? Deadline { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Interview> Interviews { get; set; }
        public ICollection<Note> Notes { get; set; }
    }
}
