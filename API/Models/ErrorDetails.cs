﻿using System.Text.Json;

namespace API.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Messenger { get; set; }
        public string[] ErrorsDetail { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
