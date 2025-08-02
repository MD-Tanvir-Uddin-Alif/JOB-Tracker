namespace JobTrackerApi.Dtos
{
    public class CompanyCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Website { get; set; }
        public string? Industry { get; set; }
        public string? Location { get; set; }
    }

    public class CompanyDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Website { get; set; }
        public string? Industry { get; set; }
        public string? Location { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
