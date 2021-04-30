using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ConsumerPortalNetService
{
    
    public class ResponseDTO<T> where T : class
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }

    public class RequestResponseDTO<T>
    {
        public T d { get; set; }
    }
}