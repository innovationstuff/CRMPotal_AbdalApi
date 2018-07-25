namespace NasAPI.Models
{
    public class Device
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string DeviceId { get; set; }
        public bool IsOnline { get; set; }
    }
}