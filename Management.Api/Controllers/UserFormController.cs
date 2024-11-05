using System.Security.Claims;
using Management.Api.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class UserFormController : ControllerBase
{
    private readonly ISupportFormService _supportFormService;

    public UserFormController(ISupportFormService supportFormService)
    {
        _supportFormService = supportFormService;
    }

    // Yeni form oluşturma (Customer ve Admin erişebilir)
    [HttpPost("create-form")]
    [Authorize]
    [UserTypeAuthorize(UserType.Customer, UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> CreateForm([FromBody] SupportFormCreateDto formDto)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Oturum açmış kullanıcının Id'si
            var createdForm = await _supportFormService.CreateFormAsync(formDto, userId);
            return Ok(new { message = "Support form created successfully", form = MapToReadDto(createdForm) });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    // Form güncelleme (Customer yalnızca kendi formunu güncelleyebilir, Admin ve SuperAdmin tüm formları güncelleyebilir)
    [HttpPut("{id}/update-form")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin, UserType.Customer)]
    public async Task<IActionResult> UpdateForm(Guid id, [FromBody] SupportFormUpdateDto formDto)
    {
        try
        {
            var form = await _supportFormService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound(new { message = "Form not found" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTypeClaim = User.FindFirst("UserType")?.Value;

            if (userTypeClaim == UserType.Customer.ToString() && form.FilledById.ToString() != userId)
            {
                return Forbid("You can only update your own forms.");
            }

            form.Subject = formDto.Subject;
            form.Message = formDto.Message;
            form.FormStatus = formDto.FormStatus;

            var updated = await _supportFormService.UpdateFormAsync(form);
            if (updated)
                return Ok(new { message = "Support form updated successfully", form = MapToReadDto(form) });
            return BadRequest(new { message = "Failed to update support form" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // Tüm formları getirme (Yalnızca Admin ve SuperAdmin erişebilir)
    [HttpGet("get-all-forms")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetAllForms([FromQuery] bool includeDeleted = false)
    {
        var forms = await _supportFormService.GetAllFormsAsync(includeDeleted);
        var formDtos = forms.Select(MapToReadDto).ToList();
        return Ok(formDtos);
    }

    // Belirli bir formu ID ile getirme (Customer yalnızca kendi formunu görebilir, Admin ve SuperAdmin tüm formları görebilir)
    [HttpGet("{id}/get-form-by-id")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin, UserType.Customer)]
    public async Task<IActionResult> GetFormById(Guid id)
    {
        try
        {
            var form = await _supportFormService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound(new { message = "Form not found" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTypeClaim = User.FindFirst("UserType")?.Value;

            if (userTypeClaim == UserType.Customer.ToString() && form.FilledById.ToString() != userId)
            {
                return Forbid("You can only view your own forms.");
            }

            return Ok(MapToReadDto(form));
        }
        catch (Exception ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    // Formu silme (soft delete) (Customer yalnızca kendi formunu silebilir, Admin ve SuperAdmin tüm formları silebilir)
    [HttpDelete("{id}/delete-form")]
    [Authorize]
    [UserTypeAuthorize(UserType.Admin, UserType.Superadmin, UserType.Customer)]
    public async Task<IActionResult> DeleteForm(Guid id)
    {
        try
        {
            var form = await _supportFormService.GetFormByIdAsync(id);
            if (form == null)
            {
                return NotFound(new { message = "Form not found" });
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userTypeClaim = User.FindFirst("UserType")?.Value;

            if (userTypeClaim == UserType.Customer.ToString() && form.FilledById.ToString() != userId)
            {
                return Forbid("You can only delete your own forms.");
            }

            var deleted = await _supportFormService.DeleteFormAsync(id);
            if (deleted)
                return Ok(new { message = "Support form deleted successfully" });
            return BadRequest(new { message = "Failed to delete support form" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("my-forms")]
    [Authorize]
    [UserTypeAuthorize(UserType.Customer, UserType.Admin, UserType.Superadmin)]
    public async Task<IActionResult> GetFormsByUser()
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Oturum açmış kullanıcının ID'si
            var forms = await _supportFormService.GetFormsByUserIdAsync(Guid.Parse(userId));
            var formDtos = forms.Select(MapToReadDto).ToList();
            return Ok(formDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }


    private SupportFormReadDto MapToReadDto(SupportForm form)
    {
        return new SupportFormReadDto
        {
            Id = form.Id,
            FilledById = form.FilledById,
            Subject = form.Subject,
            Message = form.Message,
            FormStatus = form.FormStatus
        };
    }
}
