using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ArcherMobilApp.BLL.Tests.Controllers.Document
{
    public class AuthorizedMethodDocumentControllerTestData : IEnumerable<object[]>
    {
        /// <summary>
        /// Usage var1 - testing url
        /// var2 - type request
        /// var 3 - expected status code 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "api/document", "Put", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/document", "Post", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/document", "Delete", HttpStatusCode.Unauthorized };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}