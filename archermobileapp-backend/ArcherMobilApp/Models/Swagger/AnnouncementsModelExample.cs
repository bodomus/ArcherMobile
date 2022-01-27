using System;
using System.Collections.Generic;

using Swashbuckle.AspNetCore.Filters;

using ArcherMobilApp.DAL.MsSql.Models;

namespace ArcherMobilApp.Models.Swagger
{
    public class AnnouncementsModelExample : IExamplesProvider<IEnumerable<AnnouncementEntity>>
    {
        public IEnumerable<AnnouncementEntity> GetExamples()
        {
            return new List<AnnouncementEntity>()
            {
                new AnnouncementEntity(){
                AnnouncmentTypeId = 1,
                Description = "Description",
                Id = 1, 
                PublishDate = DateTime.Now, 
                ShortDescription = "Short description",
                Title = "Title", 
                UserCreatetorId = "asdads"
                },
               new AnnouncementEntity(){
                AnnouncmentTypeId = 2,
                Description = "Description 2",
                Id = 2,
                PublishDate = DateTime.Now,
                ShortDescription = "Short description",
                Title = "Title",
                UserCreatetorId = "asd"
                }
            };
        }
    }
}