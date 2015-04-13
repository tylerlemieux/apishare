using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIShare.Models;

namespace APIShare.ViewModels
{
    public class BookmarkVM
    {
        public int BookmarkID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Tag> Tags { get; set; }
    }
}