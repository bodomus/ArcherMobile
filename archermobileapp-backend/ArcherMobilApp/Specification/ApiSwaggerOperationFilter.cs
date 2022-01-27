using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ArcherMobilApp.Specification
{
    //public class ApiSwaggerOperationFilter : IOperationFilter
    //{
    //    public void Apply(Operation operation, OperationFilterContext context)
    //    {
    //        var controllerDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
    //        if (controllerDescriptor == null)
    //            return;

    //        string tokenType = null;
    //        var baseType = controllerDescriptor.ControllerTypeInfo.BaseType;
    //        if (baseType == typeof(UserController)) tokenType = "user";
    //        else if (baseType == typeof(OperatorController)) tokenType = "operator";
    //        else if (baseType == typeof(ApiConsumerController)) tokenType = "api consumer";

    //        if (context.ApiDescription.HttpMethod == "POST" || context.ApiDescription.HttpMethod == "PUT")
    //            RemoveFormPostResponses(controllerDescriptor.MethodInfo, operation.Parameters);

    //        var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
    //        var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
    //        var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);
    //        //var userForbid = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is UserForbid);
    //        //var operatorForbid = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is OperatorForbid);

    //        if (isAuthorized && !allowAnonymous)
    //        {
    //            if (operation.Parameters == null)
    //                operation.Parameters = new List<IParameter>();

    //            operation.Parameters.Add(new NonBodyParameter
    //            {
    //                Name = "Authorization",
    //                In = "header",
    //                Description = "access token",
    //                Required = true,
    //                Type = "string",
    //                Default = $"Bearer {{insert {tokenType} token}}"
    //            });
    //        }

    //        var returnType =
    //            controllerDescriptor.MethodInfo.ReturnType.GetTypeArgumentIfGenericTypeIs(typeof(Task<>)) ??
    //            controllerDescriptor.MethodInfo.ReturnType;

    //        if (PreDifinedResults.TryGetValue(returnType, out StatusCodeDescription scd))
    //        {
    //            SetResponses(operation.Responses, scd.StatusCode, scd.Description);
    //        }
    //        else
    //        {
    //            //if (rawReturnType.GetTypeInfo().IsAssignableFrom(typeof(IActionResult)))
    //            {
    //                var genericApiResponseType = typeof(ApiResponse<>).MakeGenericType(returnType);
    //                var schema = context.SchemaRegistry.GetOrRegister(genericApiResponseType);
    //                SetResponses(operation.Responses, HttpStatusCode.OK, "Success object", schema);
    //            }
    //        }

    //        if (!allowAnonymous)
    //        {
    //            SetResponses(operation.Responses, HttpStatusCode.Unauthorized, "Unauthorized - token is required");
    //            SetResponses(operation.Responses, HttpStatusCode.Forbidden, $"Forbidden - {tokenType} token is required");
    //        }
    //    }

    //    private static void SetResponses(IDictionary<string, Response> responses, HttpStatusCode statusCode, string description = null, Schema schema = null)
    //    {
    //        var statusCodeKey = ((int)statusCode).ToString();
    //        if (responses.ContainsKey(statusCodeKey))
    //            responses.Remove(statusCodeKey);

    //        responses[statusCodeKey] = new Response { Description = description, Schema = schema };
    //    }

    //    private class StatusCodeDescription
    //    {
    //        public StatusCodeDescription(HttpStatusCode statusCode, string description)
    //        {
    //            StatusCode = statusCode;
    //            Description = description;
    //        }

    //        public HttpStatusCode StatusCode;
    //        public string Description;
    //    }

    //    private static readonly Dictionary<Type, StatusCodeDescription> PreDifinedResults = new Dictionary<Type, StatusCodeDescription>
    //    {
    //        [typeof(void)] = new StatusCodeDescription(HttpStatusCode.OK, "Empty result"),
    //        [typeof(Task)] = new StatusCodeDescription(HttpStatusCode.OK, "Empty result"),
    //        [typeof(IActionResult)] = new StatusCodeDescription(HttpStatusCode.OK, "Empty result"),
    //        [typeof(ActionResult)] = new StatusCodeDescription(HttpStatusCode.OK, "Empty result"),
    //        [typeof(RedirectResult)] = new StatusCodeDescription(HttpStatusCode.Redirect, "Redirect"),
    //        [typeof(RedirectToActionResult)] = new StatusCodeDescription(HttpStatusCode.Redirect, "Redirect"),
    //        [typeof(FileResult)] = new StatusCodeDescription(HttpStatusCode.OK, "Success - file result"),
    //        [typeof(FileStreamResult)] = new StatusCodeDescription(HttpStatusCode.OK, "Success -file stream result")
    //    };

    //    private static void RemoveFormPostResponses(MethodInfo methodInfo, IList<IParameter> parameters)
    //    {
    //        foreach (var parameter in methodInfo.GetParameters().Where(p => p.GetCustomAttribute<FromFormAttribute>() != null))
    //        {
    //            var properties = parameter.ParameterType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
    //            if (parameter.ParameterType.Name.EndsWith("args", StringComparison.OrdinalIgnoreCase))
    //                properties = properties.Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() != null).ToArray();

    //            foreach (var property in properties)
    //                parameters.Remove(parameters.Single(p => p.Name == property.Name));
    //        }
    //    }
    //}
}

