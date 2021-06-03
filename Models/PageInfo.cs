using System;

namespace WebStore.Models
{
    public class PageInfo
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int ItemPerPage { get; set; }
        public string UrlParam { get; set; }
        public int TotalPage => (int)Math.Ceiling((decimal)TotalItems / ItemPerPage);
    }
}