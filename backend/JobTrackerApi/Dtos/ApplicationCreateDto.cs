using System.ComponentModel.DataAnnotations;
using JobTrackerApi.Models;

public class ApplicationCreateDto
{
    [Required]
    public Guid CompanyId { get; set; }

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
}
