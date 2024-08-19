namespace TriangleAPI.Models.Request
{
    public class ApiLogRequest
    {
        public string Path { get; set; }
        public string PathBase { get; set; }
        public string TraceIdentifier { get; set; }
        public string QueryString { get; set; }
        public string Protocol { get; set; }
        public string Method { get; set; }
        public string Scheme { get; set; }
        public string Host { get; set; }
        public bool IsRequest { get; set; } // true if its a Request log, and false if its a Response log
        public object RequestResponseData { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }

}
