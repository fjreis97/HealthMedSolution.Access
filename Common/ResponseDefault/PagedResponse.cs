using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HealthMed.API.Access.Common.ResponseDefault;

public class PagedResponse<TData> : Response<TData>
{

    [JsonConstructor]
    public PagedResponse(TData data, int maximumResults) : base(data)
    {
        Data = data;
        MaximumResults = maximumResults;
    }

    public PagedResponse(TData? data, int code = 200, string? message = null) : base(data, code, message)
    {

    }

    public int MaximumResults { get; set; }
}
