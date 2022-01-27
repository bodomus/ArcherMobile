using Swashbuckle.AspNetCore.Filters;

using ArcherMobilApp.Models.AuxiliaryModels;

namespace ArcherMobilApp.Models.Swagger
{
    public class DtParametersRequestModelExample : IExamplesProvider<DtParameters>
    {
        public DtParameters GetExamples()
        {
            var model = new DtParameters()
            {
                Draw = 10,
                Length = 40,
                Start = 1,
                Search = new DtSearch() { Regex = true, Value = "tetst" }
            };

            return model;
        }
    }
}