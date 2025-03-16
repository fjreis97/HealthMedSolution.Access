using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.ResponseDefault;

public class Response<TData>
{
    private readonly int _code;
    private const int DefaultStatusCode = 200;

    [JsonConstructor]
    public Response() => _code = DefaultStatusCode;


    public Response(TData? data, int code = DefaultStatusCode, string? message = null, List<string>? errors = null)
    {
        Data = data;
        Message = message;
        _code = code;
        Errors = errors ?? new List<string>();
    }

    public TData? Data { get; set; }
    public string? Message { get; set; }
    public List<string>? Errors { get; set; }
    [JsonIgnore]
    public bool IsSuccess => _code is >= 200 and <= 299;

}

