using System.ComponentModel.DataAnnotations;

namespace TrackItAPI.DTOs.Issue
{
    public class IssueUpdateDto
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Status { get; set; }
    }
}
