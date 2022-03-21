using AutoMapper;
using FanDuelDemo.Models;
using FanDuelDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FanDuelDemo.Middleware
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PlayerPositionVM, Position>();
        }
    }
}
