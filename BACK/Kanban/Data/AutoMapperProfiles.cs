using Kanban.Dto;
using Kanban.Models;
using System.Linq;
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