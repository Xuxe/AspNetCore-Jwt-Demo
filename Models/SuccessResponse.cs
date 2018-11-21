using System;

namespace AuthService.Api.Models
{
    public class SuccessResponse
    {
        public int Code { get; set; }
        public dynamic Data { get; set; }
    }
}