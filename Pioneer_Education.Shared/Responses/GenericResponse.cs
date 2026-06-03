using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pioneer_Education.Shared.Responses
{
    public class GenericResponse<T>
    {
        public int StatusCode { get; set; }

        public string Message { get; set; } = null!;

        public T Data { get; set; }
    }
}
