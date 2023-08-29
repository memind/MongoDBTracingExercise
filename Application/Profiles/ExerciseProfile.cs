using Application.DTOs.ExerciseDTOs;
using Application.VMs.ExerciseVMs;
using AutoMapper;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.Profiles
{
    public class ExerciseProfile : Profile
    {
        public ExerciseProfile()
        {
            CreateMap<ExerciseDTO, Exercise>().ReverseMap();
            CreateMap<ExerciseVM, Exercise>().ReverseMap();
            CreateMap<ExerciseVM, ExerciseDTO>().ReverseMap();
        }
    }
}
