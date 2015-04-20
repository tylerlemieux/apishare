using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APIShare.ViewModels;

namespace APIShare.Models.Search
{
    public static class SearchModel
    {
        /// <summary>
        /// Searches database for users and apis
        /// </summary>
        /// <returns>SearchVM with list of users and libraries</returns>
        public static SearchVM Search(string searchString) 
        {
            SearchVM viewModel = new SearchVM();
            viewModel.Libraries = SearchLibraries(searchString);
            viewModel.Users = SearchUsers(searchString);

            return viewModel;
        }

        public static List<SearchLibraryResult> SearchLibraries(string searchString)
        {
            using (APIToolEntities context = new APIToolEntities())
            {
                var searchResult = 
                (from l in context.Bookmarks
                 where l.Name.Contains("/" + searchString + "/")
                    || l.Description.Contains("/" + searchString + "/")
                 select new SearchLibraryResult
                 {
                     LibraryID = l.BookmarkID,
                     LibraryName = l.Name,
                     Description = l.Description
                 }).ToList();

                return searchResult;
            }
        }
        public static List<SearchUserResult> SearchUsers(string searchString)
        {
            using (APIToolEntities context = new APIToolEntities())
            {
                var searchResult =
                (from u in context.Users
                 where u.Username.Contains("/" + searchString + "/")
                    || u.Name.Contains("/" + searchString + "/")
                 select new SearchUserResult
                 {
                     UserID = u.UserID,
                     Username = u.Username,
                     Avatar = u.Avatar,
                     Bio = u.Bio, 
                     Name = u.Name
                 }).ToList();

                return searchResult;
            }
        }
    }

}