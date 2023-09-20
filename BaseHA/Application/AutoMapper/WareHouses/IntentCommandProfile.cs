using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;

namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class IntentCommandProfile : Profile
    {
        public IntentCommandProfile()
        {
            CreateMap<IntentCommands, Intent>();
            //.ForMember(x => x.Answers, opt => opt.Ignore());


            CreateMap<Intent, IntentCommands>();
            
        }
    }
    public static class MappingExtensionsIntent
    {

        public static IntentCommands ToModel(this Intent entity)
        {
            return AutoMapperConfiguration.Mapper.Map<Intent, IntentCommands>(entity);
        }

        public static Intent ToEntity(this IntentCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<IntentCommands, Intent>(model);
        }

        public static Intent ToEntity(this IntentCommands model, Intent destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }
}
