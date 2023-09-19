using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;

namespace BaseHA.Application.AutoMapper.WareHouses
{
    public class CategoryCommandProfile : Profile
    {
        public CategoryCommandProfile() 
        {
            CreateMap<CategoryCommands, Category>()
                .ForMember(x => x.Intents, opt => opt.Ignore())
                .ForMember(x => x.Answers, opt => opt.Ignore());

            CreateMap<Category, CategoryCommands>();
                //.ForMember(x => x.Intents, opt => opt.Ignore());
        }
    }
    public static class MappingExtensionsCategory
    {

        public static CategoryCommands ToModel(this Category entity)
        {
            return AutoMapperConfiguration.Mapper.Map<Category, CategoryCommands>(entity);
        }

        public static Category ToEntity(this CategoryCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<CategoryCommands, Category>(model);
        }

        public static Category ToEntity(this CategoryCommands model, Category destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }
}
