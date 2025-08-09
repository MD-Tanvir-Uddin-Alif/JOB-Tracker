using System;
using System.ComponentModel.DataAnnotations;

namespace JobTrackerApi.DTOs
{
    public class NoteCreateDto
    {
        [Required]
        public Guid ApplicationId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}
