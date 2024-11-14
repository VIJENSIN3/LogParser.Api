namespace LogParser.Api.Models
{
    public class LogEntry
    {
        public string Host { get; set; }
        public string Timestamp { get; set; }
        public string Request { get; set; }
        public int StatusCode { get; set; }
        public int? ResponseSize { get; set; }
    }
}
