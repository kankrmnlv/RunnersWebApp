namespace RunnersWebApp.ViewModels
{
    public class EditUserProfileViewModel
    {
        public string Id { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public IFormFile Image { get; set; }
    }
}
