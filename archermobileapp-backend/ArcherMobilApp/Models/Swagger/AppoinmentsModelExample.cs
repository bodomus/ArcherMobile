using System.Collections.Generic;

using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.Entity;
using Archer.AMA.DTO;
using System;

namespace ArcherMobilApp.Models.Swagger
{
    public class AppoinmentsModelExample : IExamplesProvider<IEnumerable<AppoinmentDTO>>
    {
        public IEnumerable<AppoinmentDTO> GetExamples()
        {
            return new List<AppoinmentDTO>()
            {
                new AppoinmentDTO(){
                    Id = 1,
                    Appoinment = "Appoinment 1",
                    OwnerId = "232cbd28-0ac4-48ba-97fa-69b44c976e8d",
                    RoomId = 1, 
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays (1)

                },
                new AppoinmentDTO(){
                    Id = 2,
                    Appoinment = "Appoinment 1",
                    OwnerId = "232cbd28-0ac4-48ba-97fa-69b44c976e8d",
                    RoomId = 2,
                    Start = DateTime.Now,
                    End = DateTime.Now.AddDays (1)
                }
            };
        }
    }
}