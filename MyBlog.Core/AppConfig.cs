using System;
using System.Collections.Generic;
using System.Text;

namespace MyBlog
{
    public class AppConfig
    {
        public string ConnStr { get; set; }
        public string FaciconUrl { get; set; }
        public string AppName { get; set; }

        public string AppNo { get; set; }

        public string AboutMeImg { get; set; }

        public string AboutMeName { get; set; }

        public string AboutMeIntro { get; set; }

        public string AboutMeUrl { get; set; }

        public string FontFamily { get; set; }

        public string ImgSavePath { get; set; }

        public List<string> ImgExtensions { get; set; }

        public List<KV> RecommendUrls { get; set; }

        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Author { get; set; }

        public string PwdSalt { get; set; }
    }

    public class KV
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
