using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ArcherMobilApp.BLL.Tests.Controllers.Room
{
    public class UnAuthorizedMethodRoomControllerTestData : IEnumerable<object[]>
    {
        /// <summary>
        /// Usage var1 - testing url
        /// var2 - type request
        /// var 3 - Unexpected status code 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "api/room", "Post", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/room", "Put", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/room", "Delete", HttpStatusCode.Unauthorized };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}