

using System.Threading.Tasks;
using SyZero.Application.Routing;
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
        : IAsyncCrudAppService<TEntityDto, TGetAllInput, TCreateInput, TEntityDto>
        where TEntityDto : IEntityDto
    {

    }

    public interface IAsyncCrudAppService<TEntityDto, in TGetAllInput, in TCreateInput, in TUpdateInput>
        : IApplicationService
        where TEntityDto : IEntityDto
        where TUpdateInput : IEntityDto
    {
        [ApiMethod(HttpMethod.GET, "{id}")]
        Task<TEntityDto> Get(long input);

        [ApiMethod(HttpMethod.GET)]
        Task<PageResultDto<TEntityDto>> List(TGetAllInput input);

        [ApiMethod(HttpMethod.POST, "")]
        Task<TEntityDto> Create(TCreateInput input);

        [ApiMethod(HttpMethod.PUT, "{id}")]
        Task<TEntityDto> Update(long id, TUpdateInput input);

        [ApiMethod(HttpMethod.DELETE, "{id}")]
        Task<bool> Delete(long id);
    }
}
