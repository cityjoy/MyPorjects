using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EF_Web_Test.Models
{
    public partial class Subject
    {
        public int SubjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Caption { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public string Author { get; set; }
        public DateTime CreateTime { get; set; }
        public virtual ICollection<SubjectComment> CommentList { get; set; } 
    }
}
