using System.Threading.Tasks;
using SyZero.Application.Service;
using SyZero.Application.Service.Dto;
using SyZero.Example2.Core.Entities;
using SyZero.Example2.Core.Repository;
using SyZero.Example2.IApplication.Examples;
using SyZero.Example2.IApplication.Examples.Dto;

namespace SyZero.Example2.Application.Examples
{
    public class ExampleAppService : AsyncCrudAppService<Example, ExampleDto, PageAndSortQueryDto, CreateExampleDto>, IExampleAppService
    {
        private readonly IExampleRepository _exampleRepository;

        public ExampleAppService(IExampleRepository exampleRepository) : base(exampleRepository)
        {
            _exampleRepository = exampleRepository;
        }

        public async Task<ExampleDto> GetExample(long id)
        {
            var example = await _exampleRepository.GetModelAsync(p => p.Id == id);
            return ObjectMapper.Map<ExampleDto>(example);
        }

        public async Task<ExampleDto> GetByName(string name)
        {
            var example = await _exampleRepository.GetModelAsync(p => p.Name == name);
            return ObjectMapper.Map<ExampleDto>(example);
        }
    }
}
