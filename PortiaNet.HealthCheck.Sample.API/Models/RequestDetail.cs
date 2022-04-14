namespace PortiaNet.HealthCheck.Sample.API.Models
{
    public class RequestDetail
    {
        public string? IpAddress { get; set; }

        public string? Username { get; set; }

        public string? Host { get; set; }

        public string? Method { get; set; }

        public string? Path { get; set; }

        public string? QueryString { get; set; }

        public string? UserAgent { get; set; }

        public double Duration { get; set; }

        public bool HadError { get; set; }

        public string? NodeName { get; set; }

        public DateTime EventDateTime { get; set; }
    }
}
