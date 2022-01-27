using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.DTO;

namespace ArcherMobilApp.Models.Swagger
{
    public class DocumentRequestModelExample : IExamplesProvider<DocumentDTO>
    {
        public DocumentDTO GetExamples()
        {
            return new DocumentDTO()
            {
                Id = 1,
                DocumentTypeId = 1,
                Title = "Test name 1",
                Description = "Description",
                Uri= "https://docs.google.com/document/d/e/2PACX-1vR_yyGpg6FpzcjvBDgAoxay_waWOkaolKvirl-uDtsNgglphUM_AFnmKwixkCaednN4lyVzP_XJjUfc/pub"
            };
        }
    }
}