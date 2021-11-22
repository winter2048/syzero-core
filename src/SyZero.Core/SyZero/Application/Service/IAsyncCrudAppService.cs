

using System.Threading.Tasks;
using SyZero.Application.Service.Dto;

namespace SyZero.Application.Service
{

    public interface IAsyncCrudAppService<TEntityDto>
        : IAsyncCrudAppService<TEntityDto, PageAndSortQueryDto>
        where TEntityDto : IEntityDto
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TGetAllInput>
        : IAsyncCrudAppService<TEntityDto, TGetAllInput, TEntityDto, TEntityDto>
        where TEntityDto : IEntityDto
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TGetAllInput, in TCreateInput>
        : IAsyncCrudAppService<TEntityDto, TGetAllInput, TCreateInput, TCreateInput>
        where TEntityDto : IEntityDto
        where TCreateInput : IEntityDto
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : IAsyncCrudAppService<TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, EntityDto>
        where TEntityDto : IEntityDto
        where TUpdateInput : IEntityDto
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput>
        : IAsyncCrudAppService<TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto>
        where TEntityDto : IEntityDto
        where TUpdateInput : IEntityDto
        where TGetInput : IEntityDto
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TGetAllInput, in TCreateInput, in TUpdateInput, in TGetInput, in TDeleteInput>
        : IApplicationService
        where TEntityDto : IEntityDto
        where TUpdateInput : IEntityDto
        where TGetInput : IEntityDto
        where TDeleteInput : IEntityDto
    {
        Task<TEntityDto> Get(TGetInput input);

        Task<PageResultDto<TEntityDto>> GetAll(TGetAllInput input);

        Task<TEntityDto> Create(TCreateInput input);

        Task<TEntityDto> Update(TUpdateInput input);

        Task<bool> Delete(TDeleteInput input);
    }
}
