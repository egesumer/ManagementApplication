public class SupportForm : BaseEntity
{   
    public Guid FilledById { get; set; }
    public User FilledBy { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public FormStatus FormStatus { get; set; }
}