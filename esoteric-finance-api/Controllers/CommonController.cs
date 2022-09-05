using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Esoteric.Finance.Api.Controllers
{
    public abstract class CommonController
    {
        public JsonResult JsonOk<TValue>(TValue value) => new(value) { StatusCode = 200 };
        public JsonResult JsonNotFound<TValue>(TValue value) => new(value) { StatusCode = 404 };
        public JsonResult JsonError<TValue>(TValue value) => new(value) { StatusCode = 500 };
        public JsonResult JsonWithStatus<TValue>(TValue value, HttpStatusCode statusCode) => new(value) { StatusCode = (int)statusCode };
        public ObjectResult ObjectOk<TValue>(TValue value) => new(value) { StatusCode = 200 };
        public ObjectResult ObjectNotFound<TValue>(TValue value) => new(value) { StatusCode = 404 };
        public ObjectResult ObjectError<TValue>(TValue value) => new(value) { StatusCode = 500 };
        public ObjectResult ObjectWithStatus<TValue>(TValue value, HttpStatusCode statusCode) => new(value) { StatusCode = (int)statusCode };
    }
}
