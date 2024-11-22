using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts.Response
{
    public enum StatusCode
    {
        Ok,
        NotFound,
        InternalServerError,
        NotAuthorize,
        BadRequest
    }
}
