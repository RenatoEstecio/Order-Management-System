using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Library.UTIL
{
    public class CustomException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? fieldName = null) : Exception(message)
    {
        public HttpStatusCode StatusCode { get; set; } = statusCode;
        public string? FieldName { get; set; } = fieldName;
    }
}
