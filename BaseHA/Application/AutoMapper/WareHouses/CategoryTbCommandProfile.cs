using AutoMapper;
using BaseHA.Application.ModelDto;
using BaseHA.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace BaseHA.Application.AutoMapper.CategoryTbs
{
    public class CategoryTbCommandProfile : Profile
    {
        public CategoryTbCommandProfile()
        {
            CreateMap<CategoryTbCommands, CategoryTb>()
                .ForMember(x => x.Intents, opt => opt.Ignore());
            //.ForMember(x => x.Category, opt => opt.Ignore())
            //.ForMember(x => x.IntentCodeEn, opt => opt.Ignore())
            //.ForMember(x => x.IntentCodeVn, opt => opt.Ignore())
            //.ForMember(x => x.Description, opt => opt.Ignore());

            CreateMap<CategoryTb, CategoryTbCommands>();
                //.ForMember(x => x.AvailableCategoryTb, opt => opt.Ignore());
        }   

    }

    public static class MappingExtensions
    {

        public static CategoryTbCommands ToModel(this CategoryTb entity)
        {
            return AutoMapperConfiguration.Mapper.Map<CategoryTb, CategoryTbCommands>(entity);
        }

        public static CategoryTb ToEntity(this CategoryTbCommands model)
        {
            return AutoMapperConfiguration.Mapper.Map<CategoryTbCommands, CategoryTb>(model);
        }

        public static CategoryTb ToEntity(this CategoryTbCommands model, CategoryTb destination)
        {
            return AutoMapperConfiguration.Mapper.Map(model, destination);
        }

    }

    public static class AutoMapperConfiguration
    {
        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }

        public static IList<Profile> Profiles = new List<Profile>();

        public static void Init(MapperConfiguration config)
        {
            MapperConfiguration = config;
            Mapper = config.CreateMapper();
        }
    }
}