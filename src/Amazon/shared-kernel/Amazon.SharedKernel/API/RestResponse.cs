using System.Net;

namespace Amazon.SharedKernel.API
{
    public class RestResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public object? Error { get; private set; }
        public Exception? Exception { get; private set; }
        public bool IsSuccess => ((int)StatusCode >= 200) && ((int)StatusCode <= 299);

        public RestResponse(HttpStatusCode statusCode, object error = null, Exception exception = null)
        {
            StatusCode = statusCode;
            Error = error;
            Exception = exception;
        }

        public static RestResponse Success() => new(default, HttpStatusCode.OK);
        public static RestResponse BadRequest(BadRequestModel error) => new RestResponse(HttpStatusCode.BadRequest, error);
        public static RestResponse NotFound(object error) => new RestResponse(HttpStatusCode.NotFound, error);
        public static RestResponse Failure(Exception exception) => new RestResponse(HttpStatusCode.InternalServerError, null, exception);
        public static RestResponse Conflict(object error) => new RestResponse(HttpStatusCode.Conflict, error);

    }

    public class RestResponse<TValue> : RestResponse
    {
        public TValue Value { get; set; }
        public string Id { get; set; }
        public RestResponse(TValue value, HttpStatusCode statusCode, object error = null, Exception exception = null, string id = null)
            : base(statusCode, error, exception)
        {
            Value = value;
            Id = id;
        }

        public static RestResponse<TValue> Success(TValue value) => new(value, HttpStatusCode.OK);
        public static RestResponse<TValue> Created(TValue value, string id) => new(value, HttpStatusCode.Created, id: id);
        public new static RestResponse<TValue> NotFound(object error) => new(default, HttpStatusCode.NotFound, error);
        public new static RestResponse<TValue> Conflict(object error) => new(default, HttpStatusCode.Conflict, error);
        public new static RestResponse<TValue> BadRequest(BadRequestModel error) => new(default, HttpStatusCode.BadRequest, error);
        public static new RestResponse<TValue> Failure(Exception exception) => new(default, HttpStatusCode.InternalServerError, null, exception);
    }
}
