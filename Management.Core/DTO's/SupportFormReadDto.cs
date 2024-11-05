public class SupportFormReadDto
{
    public Guid Id { get; set; }
    public Guid FilledById { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
    public FormStatus FormStatus { get; set; }
}
