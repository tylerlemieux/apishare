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
                 where l.Name.Contains(searchString)
                    || l.Description.Contains(searchString)
                 select new SearchLibraryResult
                 {
                     LibraryID = l.BookmarkID,
                     LibraryName = l.Name,
                     Description = l.Description,
                     Tags = 
                        (from bt in context.BookmarkTags
                         join t in context.Tags on bt.TagID equals t.TagID
                         where bt.BookmarkID == l.BookmarkID
                         select t.Tag1).ToList()
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
                 where u.Username.Contains(searchString)
                    || u.Name.Contains(searchString)
                 select new SearchUserResult
                 {
                     UserID = u.UserID,
                     Username = u.Username,
                     Avatar = u.Avatar,
                     Bio = u.Bio, 
                     Name = u.Name,
                     Tags = 
                        (from us in context.UserSkills
                         join t in context.Tags on us.TagID equals t.TagID
                         where us.UserID == u.UserID
                         select t.Tag1).ToList(),
                     AlreadyFollowing = "Follow"  //TODO: MAKE THIS SEE IF USER IS BEING FOLLOWED
                 }).ToList();

                return searchResult;
            }
        }
    }

}