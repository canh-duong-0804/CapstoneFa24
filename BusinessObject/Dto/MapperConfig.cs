using AutoMapper;
using BusinessObject.Dto.Blog;
using BusinessObject.Dto.Blog.CreateBlog;
using BusinessObject.Dto.Login;
using BusinessObject.Dto.Member;
using BusinessObject.Dto.Register;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
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
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender)).ReverseMap();
                cfg.CreateMap<RegisterationResponseDTO, Member>()
                .ReverseMap();

                cfg.CreateMap<LoginRequestDTO, Member>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));
                
                cfg.CreateMap<LoginRequestStaffDTO, staff>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

                cfg.CreateMap<LoginRequestStaffDTO, staff>()
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
               .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));


                cfg.CreateMap<RegisterationRequestStaffDTO, staff>()
            .ForMember(dest => dest.StaffId, opt => opt.Ignore())
            .ForMember(dest => dest.StartWorkingDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StaffImage, opt => opt.MapFrom(src => src.StaffImage))
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
            .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            ;
                cfg.CreateMap<StaffDTO, staff>()
           .ForMember(dest => dest.StaffId, opt => opt.Ignore())
           .ForMember(dest => dest.StartWorkingDate, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
           .ForMember(dest => dest.StaffImage, opt => opt.MapFrom(src => src.StaffImage))
           .ForMember(dest => dest.Dob, opt => opt.MapFrom(src => src.Dob))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName)).ReverseMap();
                
                
                cfg.CreateMap<BlogRequestDTO, Blog>()
           .ForMember(dest => dest.BlogId, opt => opt.Ignore())
           .ForMember(dest => dest.CreateDate, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
         
           .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           .ForMember(dest => dest.ThumbnailBlog, opt => opt.MapFrom(src => src.ThumbnailBlog))
           .ReverseMap();  
                
                
                
                cfg.CreateMap<UpdateBlogRequestDTO, Blog>()
           .ForMember(dest => dest.BlogId, opt => opt.Ignore())
           .ForMember(dest => dest.ChangeDate, opt => opt.MapFrom(src => DateTime.Now))
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
         
           .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
           .ForMember(dest => dest.ThumbnailBlog, opt => opt.MapFrom(src => src.ThumbnailBlog))
           .ReverseMap();
           
           
           cfg.CreateMap<LoginResponseMemberDTO, Member>()
           .ForMember(dest => dest.MemberId, opt => opt.Ignore())
         
           .ForMember(dest => dest.Status, opt => opt.MapFrom(src => true))
           .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
           .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
           .ForMember(dest => dest.Height, opt => opt.MapFrom(src => src.Height))
           .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
           .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
           .ForMember(dest => dest.ExerciseLevel, opt => opt.MapFrom(src => src.ExerciseLevel))
           .ForMember(dest => dest.Goal, opt => opt.MapFrom(src => src.Goal))
           .ReverseMap()
           ;
				cfg.CreateMap<MemberProfileDto, BusinessObject.Models.Member>()
				   .ForMember(dest => dest.MemberId, opt => opt.Ignore())
				   .ReverseMap();
			});




            var mapper = new Mapper(config);
            return mapper;
        }
    }
}