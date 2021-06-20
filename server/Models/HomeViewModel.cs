using System;

namespace server.Models
{
    public class HomeViewModel
    {
        public DateTime ZhihuLast { get; set; }
        public DateTime WeiboLast { get; set; }

        public int ZhihuId { get; set; }
        public int WeiboId { get; set; }
    }
}