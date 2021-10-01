using AutoMapper;
using AutoMapper.Configuration;

namespace Erp.Business.Infrastructure
{
    public class ModelMappingProfile
    {
        public static void ModelMappingInitialize()
        {
            var cfg = new MapperConfigurationExpression
            {
                CreateMissingTypeMaps = true,
                ValidateInlineMaps = false
            };

            Mapper.Initialize(cfg);
        }
    }
}
