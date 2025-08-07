using System;
using JobTrackerApi.Models;

public class ApplicationUpdateDto
{
    public Guid CompanyId { get; set; }
    public string JobTitle { get; set; }
    public string JobLocation { get; set; }
    public JobType JobType { get; set; }
    public string JobLink { get; set; }
    public DateTime ApplicationDate { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime? Deadline { get; set; }
}
