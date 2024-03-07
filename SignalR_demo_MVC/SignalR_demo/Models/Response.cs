namespace SignalR_demo.Models
{
    public class Response
    {

        public dynamic Data { get; set; }

        public string Message { get; set; }
        public string Status { get; set; }

        public DateTime EndTime = DateTime.Now;
    }
}
