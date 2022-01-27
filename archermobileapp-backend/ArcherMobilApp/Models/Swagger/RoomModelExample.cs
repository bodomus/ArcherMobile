using Swashbuckle.AspNetCore.Filters;

using System.Collections.Generic;
using Archer.AMA.DTO;

namespace ArcherMobilApp.Models.Swagger
{
    public class RoomModelExample : IExamplesProvider<IEnumerable<RoomDTO>>
    {
        public IEnumerable<RoomDTO> GetExamples()
        {
            return new List<RoomDTO>()
            {
                new RoomDTO(){
                    Id = 1,
                    Name = "Test name",
                    Description = "Description",
                    LinkToGoogleCalendar = "Link",
                    PhysicalAddress = "PhysicalAddress" 
                },
                new RoomDTO(){
                    Id = 1,
                    Name = "Test name",
                    Description = "Description",
                    LinkToGoogleCalendar = "Link",
                    PhysicalAddress = "PhysicalAddress"
                }
            };
        }
    }
}