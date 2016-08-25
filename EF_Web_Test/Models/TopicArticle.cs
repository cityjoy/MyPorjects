using System;
using System.Collections.Generic;

namespace EF_Web_Test.Models
{
    public partial class TopicArticle
    {
        public long ArticleId { get; set; }
        public int ChannelId { get; set; }
        public int Level2ChannelId { get; set; }
        public int Type { get; set; }
        public string Caption { get; set; }
        public string Title { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; }
        public string VideoUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string Brief { get; set; }
        public string Content { get; set; }
        public bool IsValidated { get; set; }
        public Nullable<int> ValidateAdminId { get; set; }
        public int CreateAdminId { get; set; }
        public int UpdateAdminId { get; set; }
        public System.DateTime PublicTime { get; set; }
        public System.DateTime CreateTime { get; set; }
        public Nullable<System.DateTime> UpdateTime { get; set; }
        public bool TopAttr { get; set; }
        public bool RecommendAttr { get; set; }
        public bool HotAttr { get; set; }
        public bool SlideAttr { get; set; }
        public int ViewCount { get; set; }
        public int FakeViewCount { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
    }
}
