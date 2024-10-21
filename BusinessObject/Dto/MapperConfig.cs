using AutoMapper;
using BusinessObject.Models;
using HealthTrackingManageAPI.Models.Dto;
using Repository.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Dto
{
    public class MapperConfig
    {
        public static Mapper InitializeAutomapper()
        {
         
            var config = new MapperConfiguration(cfg =>
            {

                cfg.CreateMap<RegisterationRequestDTO, Member>()
                .ForMember(dest => dest.MemberId, opt => opt.Ignore()) 
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true)) 
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender == "Male"));

                cfg.CreateMap<LoginRequestDTO, Member>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UserName)) 
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password)); 
            });



           
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}