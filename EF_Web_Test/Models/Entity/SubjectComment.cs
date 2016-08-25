using System;
using System.Collections.Generic;

namespace EF_Web_Test.Models
{
    public partial class SubjectComment
    {
        public long CommentId { get; set; }
        public int SubjectId { get; set; }
        public string Content { get; set; }
        public virtual Subject Subject { get; set; }

    }
}
