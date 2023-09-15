using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;

namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class AnswerCommandProfile : Profile
    {
        public AnswerCommandProfile()
        {
            CreateMap<AnswerCommands, Answer>();



            CreateMap<Answer, AnswerCommands>();
           
        }
    }
    public static class MappingExtensionsAnswer
    {

        public static AnswerCommands ToModel(this Answer entity)
        {
            return AutoMapperConfiguration.Mapper.Map<Answer, AnswerCommands>(entity);
        }

        public static Answer ToEntity(this AnswerCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<AnswerCommands, Answer>(model);
        }

        public static Answer ToEntity(this AnswerCommands model, Answer destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }
}
