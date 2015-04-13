using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIShare.Models;
namespace APIShare.ViewModels
{
    public class UserVM
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Bio { get; set; }
        //{
        //    get { return Bio; } 
        //    set 
        //    {
        //        Bio = value ?? "";
        //    }
        //}

        public List<Tag> Skills { get; set; }
        public List<BookmarkVM> Bookmarks { get; set; }
        public List<FollowerVM> Followers { get; set; }
        public List<FollowerVM> Following { get; set; }
    }

    public class FollowerVM
    {
        public int UserID { get; set; }
        public string Username { get; set; }
    }
}