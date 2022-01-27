using ArcherMobilApp.Models.AuxiliaryModels;
using Swashbuckle.AspNetCore.Filters;

namespace ArcherMobilApp.Models.Swagger
{
    public class AgGridParameterRequestModelExample: IExamplesProvider<AgGridModel>
    {
        public AgGridModel GetExamples()
        {
            var model = new AgGridModel()
            {
                StartRow = 1,
                EndRow = 20,
                Sort = new[] { new AgSortModel() { ColId = "colId", Sort = "asc"}, new AgSortModel() { ColId = "colId", Sort = "asc" } }
            };

            return model;
        }
    }
}

