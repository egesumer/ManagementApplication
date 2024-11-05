public class SupportRepository : GenericRepository<SupportForm>, ISupportFormRepository
{
    private readonly AppDbContext db;
    public SupportRepository(AppDbContext db) : base(db)
    {
        this.db = db;
    }
}