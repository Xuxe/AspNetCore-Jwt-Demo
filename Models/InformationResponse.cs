using System;
using System.Collections.Generic;

namespace AuthService.Api.Models
{
    public class InformationResponse
    {
        public bool Active { get; set; }
        public DateTime Expires { get; set; }
        public string Username { get; set; }
        public IEnumerable<string> Roles {get; set;}
    }
}