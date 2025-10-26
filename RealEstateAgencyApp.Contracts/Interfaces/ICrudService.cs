namespace RealEstateAgencyApp.Contracts.Interfaces;

public interface ICrudService<TGetDto, TEditDto>
{
    public Task<List<TGetDto>> GetAllAsync();
    public Task<TGetDto?> GetByIdAsync(int id);
    public Task<TGetDto> CreateAsync(TEditDto createDto);
    public Task UpdateAsync(int id, TEditDto updateDto);
    public Task DeleteAsync(int id);
}