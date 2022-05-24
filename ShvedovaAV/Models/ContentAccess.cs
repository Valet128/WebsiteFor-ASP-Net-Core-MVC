namespace ShvedovaAV.Models
{
    public class ContentAccess
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = "";
        public int ContentId { get; set; }
        public string ContentName { get; set; } = "";
        public DateTime ExpirationDate { get; set; }
    }
}
