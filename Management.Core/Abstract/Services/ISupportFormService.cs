public interface ISupportFormService
{
    Task<bool> UpdateFormAsync(SupportForm form);
    Task<bool> DeleteFormAsync(Guid formId);
    Task<SupportForm> GetFormByIdAsync(Guid formId);
    Task<IEnumerable<SupportForm>> GetAllFormsAsync(bool includeDeleted = false);
    Task<SupportForm> CreateFormAsync(SupportFormCreateDto formDto, string userId);
    Task<IEnumerable<SupportForm>> GetFormsByUserIdAsync(Guid userId);
}