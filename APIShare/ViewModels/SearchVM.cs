using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIShare.ViewModels
{
    public class SearchVM
    {
        public List<SearchLibraryResult> Libraries { get; set; }
        public List<SearchUserResult> Users { get; set; }
    }

    public class SearchUserResult
    {
        public int UserID { get; set; }
        public string AlreadyFollowing { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public byte[] Avatar { get; set; }
        public string Bio { get; set; }

        public List<string> Tags { get; set; }
    }

    public class SearchLibraryResult
    {
        public int LibraryID { get; set; }
        public string LibraryName { get; set; }
        public string Description { get; set; }

        public List<string> Tags { get; set; }
    }

}