using System.Collections;
using System.Collections.Generic;
using System.Net;

namespace ArcherMobilApp.BLL.Tests.Controllers.User
{
    public class AthorizedMethodUserControllerTestData : IEnumerable<object[]>
    {
        /// <summary>
        /// Usage var1 - testing url
        /// var2 - type request
        /// var 3 - expected status code 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "api/user/Get", "Get", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/user/Current", "Get", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/user/Current/ICRConfirm", "Put", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/user/ICRReset", "Put", HttpStatusCode.Unauthorized };
            //yield return new object[] { "api/user/Mobile/Update", "Put", HttpStatusCode.OK };
            yield return new object[] { "api/user", "Put", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/user", "Post", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/user/Remove", "Delete", HttpStatusCode.Unauthorized };
            yield return new object[] { "api/user/Lockout", "Post", HttpStatusCode.Unauthorized };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}