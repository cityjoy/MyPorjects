using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EF_Web_Test.Models.DTO
{
    public class SubjectDTO
    {
        public int SubjectId { get; set; }
        public string Title { get; set; }
        public ICollection<SubjectCommentDTO> CommentList { get; set; }
    }
}