using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIShare.Models.Helpers
{
    public static class TagHelper
    {
        /// <summary>
        /// Checks to see if a tag exists... Creates it if it doesnt and returns the id
        /// </summary>
        /// <param name="tagName">Name of the tag to check</param>
        /// <returns>tag id</returns>
        public static int CheckTag(string tagName)
        {
            APIToolEntities context = new APIToolEntities();
            int tagCount = context.Tags.Where(t => t.Tag1 == tagName).Count();

            if(tagCount == 0)
            {
                //Tag doesnt exist... create it
                Tag newTag = new Tag();
                newTag.Tag1 = tagName;
                newTag.AddedDate = DateTime.Now;

                context.Tags.Add(newTag);
                context.SaveChanges();

                return newTag.TagID;
            }
            else 
            { 
                //Tag exists... return the id
                return context.Tags.Where(t => t.Tag1 == tagName).Select(t => t.TagID).First();
            }
        }
    }
}