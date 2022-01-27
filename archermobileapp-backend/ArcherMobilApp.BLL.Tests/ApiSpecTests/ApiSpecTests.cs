using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Archer.AMA.DTO;
using ArcherMobilApp.Controllers.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Xunit;

namespace ArcherMobilApp.BLL.Tests.ApiSpecTests
{
    public class ApiSpecTests
    {
        [Fact(DisplayName = "Api Consistency")]
        void ApiConsistencyTest()
        {
            var controllerMethods = typeof(ReadonlyWebApiController<DocumentDTO, int?, DocumentService>).GetTypeInfo().Assembly.GetTypes()
                .Where(type => typeof(ReadonlyWebApiController<DocumentDTO, int?, DocumentService>).IsAssignableFrom(type))
                .Select(controller => controller.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Where(m =>
                {
                    if (m.IsSpecialName) return false; //exclude getters and setters

                    var apiExplorerSettingsAttribute = m.GetCustomAttribute<ApiExplorerSettingsAttribute>();
                    return (apiExplorerSettingsAttribute == null || !apiExplorerSettingsAttribute.IgnoreApi);
                }));

            var methods = new List<MethodInfo>();
            foreach (var controller in controllerMethods)
                foreach (var method in controller)
                    methods.Add(method);

            foreach (var method in methods)
            {
                Assert.True(method.IsDefined(typeof(HttpMethodAttribute)),
                    $"The '{nameof(HttpMethodAttribute)}' (GET, POST, PUT) is NOT defined for public {method.DeclaringType.Name}.{method.Name} method.");

                foreach (var pi in method.GetParameters())
                {
                    var hasBinding = pi.GetCustomAttributes().Any(a => a is IBindingSourceMetadata);
                    Assert.True(hasBinding,
                        $"Paramenter '{pi.Name}' in {method.DeclaringType.Name}.{method.Name} method has NO '{nameof(IBindingSourceMetadata)}' (Form, Body, Route, Query, Header) attributre");

                    /*if ((pi.IsDefined(typeof(FromBodyAttribute)) || pi.IsDefined(typeof(FromFormAttribute))) && !typeof(IFormFile).IsAssignableFrom(pi.ParameterType))
                        Assert.True(typeof(IApiArgs).IsAssignableFrom(pi.ParameterType),
                            $"Paramenter '{pi.Name} - {pi.ParameterType.Name}' in {method.DeclaringType.Name}.{method.Name} method is NOT implement {nameof(IApiArgs)}");*/
                }

                var methodReturnType = method.ReturnType;
                var methodReturnTypeInfo = methodReturnType.GetTypeInfo();

                if (methodReturnTypeInfo.IsGenericType && methodReturnTypeInfo.GetGenericTypeDefinition() == typeof(Task<>))
                    methodReturnType = methodReturnType.GenericTypeArguments[0];

                Assert.True(typeof(IActionResult).IsAssignableFrom(methodReturnType),
                    $"'IActionResult' is not detected for return type '{methodReturnType.Name}' in {method.DeclaringType}.{method.Name}");
            }
        }
    }
}
