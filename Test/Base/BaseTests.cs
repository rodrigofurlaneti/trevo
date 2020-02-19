using Aplicacao.Mappers;
using Test.Start;

namespace Test
{
    public class BaseTests
    {
        public BaseTests()
        {
            SimpleInjectorInitializer.Initialize();

            AutoMapper.Mapper.Reset();
            AutoMapperConfig.RegisterMappings();
        }
    }
}
