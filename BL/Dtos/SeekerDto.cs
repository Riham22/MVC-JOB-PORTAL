using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class SeekerDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string? Headline { get; set; }
        public string? Summary { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public byte? YearsOfExperience { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? PhotoUrl { get; set; }
        public string? UserEmail { get; set; }

        // Navigation properties
        public List<ProfileCVFileDto> CVFiles { get; set; } = new();
        public List<ProfileApplicationDto> Applications { get; set; } = new();
        public List<ProfileSavedJobDto> SavedJobs { get; set; } = new();

    }
}
