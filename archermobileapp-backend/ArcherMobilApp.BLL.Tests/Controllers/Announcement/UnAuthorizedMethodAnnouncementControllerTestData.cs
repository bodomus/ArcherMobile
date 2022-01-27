using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ArcherMobilApp.BLL.Tests.Controllers.Announcement
{
    public class UnAuthorizedMethodAnnouncementControllerTestData : IEnumerable<object[]>
    {
        /// <summary>
        /// Usage var1 - testing url
        /// var2 - type request
        /// var 3 - Unexpected status code 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "api/announcement", "Post", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/announcement", "Put", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/announcement", "Delete", HttpStatusCode.Unauthorized };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}