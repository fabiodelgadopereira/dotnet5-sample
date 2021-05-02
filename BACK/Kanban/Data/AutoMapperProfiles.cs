using Kanban.Dto;
using Kanban.Models;
using AutoMapper;

namespace Kanban.Data
{
     public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForLoginDto,User>();
        }
    }

}