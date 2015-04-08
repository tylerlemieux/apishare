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

            return new SearchVM();
        }
    }

}