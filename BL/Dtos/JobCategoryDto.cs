using BL.Dtos.Base;
using Domains;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Dtos
{
    public class JobCategoryDto:BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public List<JobPost> ? JobPosts { get; set; } = new List<JobPost>();

    }
}
