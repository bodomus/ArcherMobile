using System;

using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.DTO;

namespace ArcherMobilApp.Models.Swagger
{
    public class AppoinmentModelExample : IExamplesProvider<AppoinmentDTO>
    {
        public AppoinmentDTO GetExamples()
        {
            return new AppoinmentDTO()
            {
                Id = 1,
                Appoinment = "Appoinment 1",
                OwnerId = "232cbd28-0ac4-48ba-97fa-69b44c976e8d",
                RoomId = 1,
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
        }
    }
}