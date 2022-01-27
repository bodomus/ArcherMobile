using System.Collections.Generic;

using Swashbuckle.AspNetCore.Filters;

using Archer.AMA.Entity;

namespace ArcherMobilApp.Models.Swagger
{
    public class DocumentModelExample : IExamplesProvider<IEnumerable<DocumentEntity>>
    {
        public IEnumerable<DocumentEntity> GetExamples()
        {
            return new List<DocumentEntity>()
            {
                new DocumentEntity(){
                Id = 1,
                DocumentTypeId = 1,
                Title = "Test name 1",
                Description = "Description",
                Uri= "https://docs.google.com/document/d/e/2PACX-1vR_yyGpg6FpzcjvBDgAoxay_waWOkaolKvirl-uDtsNgglphUM_AFnmKwixkCaednN4lyVzP_XJjUfc/pub"
                },
                new DocumentEntity(){
                Id = 2,
                DocumentTypeId = 2,
                Title = "Test name 2",
                Description = "Description",
                Uri= "https://docs.google.com/document/d/e/2PACX-1vTPUhoSNUxML0WFwEMoCnqN31S-BglX6A-qTr-HXKELW7TJhbqoZHLGUo7TXqKiGZfsfdr0tu3rvAXY/pub"
                }
            };
        }
    }
}