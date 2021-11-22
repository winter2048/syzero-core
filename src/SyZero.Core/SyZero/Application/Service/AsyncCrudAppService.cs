
using System.Linq;
using System.Threading.Tasks;
using SyZero.Application.Service.Dto;
using SyZero.Domain.Entities;
using SyZero.Domain.Repository;

namespace SyZero.Application.Service
{


    public abstract class AsyncCrudAppService<TEntity, TEntityDto>
        : AsyncCrudAppService<TEntity, TEntityDto, PageAndSortQueryDto>
        where TEntity : class, IEntity
        where TEntityDto : IEntityDto
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TEntityDto, TEntityDto>
        where TEntity : class, IEntity
        where TEntityDto : IEntityDto
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TCreateInput>
        where TEntity : class, IEntity
        where TEntityDto : IEntityDto
        where TCreateInput : IEntityDto
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>
        : AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, EntityDto>
        where TEntity : class, IEntity
        where TEntityDto : IEntityDto
        where TUpdateInput : IEntityDto
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, TGetInput>
    : AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, EntityDto>
        where TEntity : class, IEntity
        where TEntityDto : IEntityDto
        where TUpdateInput : IEntityDto
        where TGetInput : IEntityDto
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
    }

    public abstract class AsyncCrudAppService<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
       : CrudAppServiceBase<TEntity, TEntityDto, TGetAllInput, TCreateInput, TUpdateInput>,
        IAsyncCrudAppService<TEntityDto, TGetAllInput, TCreateInput, TUpdateInput, TGetInput, TDeleteInput>
           where TEntity : class, IEntity
           where TEntityDto : IEntityDto
           where TUpdateInput : IEntityDto
           where TGetInput : IEntityDto
           where TDeleteInput : IEntityDto
    {
        protected AsyncCrudAppService(IRepository<TEntity> repository)
            : base(repository)
        {

        }
        public virtual async Task<TEntityDto> Get(TGetInput input)
        {
            CheckGetPermission();

            var entity = await GetEntityByIdAsync(input.Id);
            return MapToEntityDto(entity);
        }

        public virtual async Task<PageResultDto<TEntityDto>> GetAll(TGetAllInput input)
        {
            CheckGetAllPermission();

            var query = await CreateFilteredQueryAsync(input);

            var totalCount = query.Count();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = query.ToList();

            return new PageResultDto<TEntityDto>(
                totalCount,
                entities.Select(MapToEntityDto).ToList()
            );
        }

        public virtual async Task<TEntityDto> Create(TCreateInput input)
        {
            CheckCreatePermission();

            var entity = MapToEntity(input);

            await Repository.AddAsync(entity);

            return MapToEntityDto(entity);
        }

        public virtual async Task<TEntityDto> Update(TUpdateInput input)
        {
            CheckUpdatePermission();

            var entity = await GetEntityByIdAsync(input.Id);

            MapToEntity(input, entity);

            return MapToEntityDto(entity);
        }

        public virtual async Task<bool> Delete(TDeleteInput input)
        {
            CheckDeletePermission();
            await Repository.DeleteAsync(input.Id);
            return true;
        }

        protected virtual async Task<TEntity> GetEntityByIdAsync(long id)
        {
            return await Repository.GetModelAsync(id);
        }
    }
}
