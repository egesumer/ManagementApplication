public class SupportFormService : ISupportFormService
{
    private readonly ISupportFormRepository _supportFormRepository;

    public SupportFormService(ISupportFormRepository supportFormRepository)
    {
        _supportFormRepository = supportFormRepository;
    }

    // Yeni bir form ekleme
    public async Task<SupportForm> CreateFormAsync(SupportFormCreateDto formDto, string userId)
    {
        var form = new SupportForm
        {
            FilledById = Guid.Parse(userId),
            Subject = formDto.Subject,
            Message = formDto.Message,
            FormStatus = FormStatus.Filled,
            IsDeleted = false
        };

        _supportFormRepository.Add(form);
        return form;
    }

    // Form güncelleme
    public async Task<bool> UpdateFormAsync(SupportForm form)
    {
        return _supportFormRepository.Update(form);
    }

    // Form silme
    public async Task<bool> DeleteFormAsync(Guid formId)
    {
        var form = _supportFormRepository.GetByID(formId);
        if (form == null)
        {
            throw new Exception("Support form not found.");
        }
        form.FormStatus = FormStatus.Deleted;
        form.IsDeleted = true;
        return _supportFormRepository.Update(form);
    }

    // Belirli bir formu ID ile getirme
    public async Task<SupportForm> GetFormByIdAsync(Guid formId)
    {
        return _supportFormRepository.GetByID(formId);
    }

    // Tüm formları getirme
    public async Task<IEnumerable<SupportForm>> GetAllFormsAsync(bool includeDeleted = false)
    {
        return _supportFormRepository.GetAll(includeDeleted);
    }
    public async Task<IEnumerable<SupportForm>> GetFormsByUserIdAsync(Guid userId)
    {
        // Kullanıcının oluşturduğu talepleri getiren sorgu
        return _supportFormRepository.GetWhere(form => form.FilledById == userId && !form.IsDeleted);
    }
}
