using System;
using System.Collections.Generic;

namespace EF_Web_Test.Models
{
    public partial class TopicArticle
    {
        public long ArticleId { get; set; }
        public string Caption { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public System.DateTime CreateTime { get; set; }
        public string Author { get; set; }
    }
}
