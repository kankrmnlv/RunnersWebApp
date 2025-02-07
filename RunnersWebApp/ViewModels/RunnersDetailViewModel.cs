namespace RunnersWebApp.ViewModels
{
    public class RunnersDetailViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }

        public string Image { get; set; }
    }
}
