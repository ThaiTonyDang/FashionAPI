using System.Collections;
using System.Collections.Generic;
using System.Text.Json;

namespace API.Dtos
{
    public class Error : BaseReponse
    {
        public IEnumerable<string> Errors { get; set; }
        public Error(int statusCode, string message, IEnumerable<string> errors) : base(false, message, statusCode)
        {
            Errors = errors;
        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
