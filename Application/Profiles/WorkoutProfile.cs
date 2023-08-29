using Application.DTOs.ExerciseDTOs;
using Application.DTOs.WorkoutDTOs;
using Application.VMs.WorkoutVMs;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class WorkoutProfile : Profile
    {
        public WorkoutProfile()
        {
            CreateMap<Workout, WorkoutDTO>().ReverseMap();
            CreateMap<Workout, WorkoutVM>().ReverseMap();
            CreateMap<WorkoutDTO, WorkoutVM>().ReverseMap();
        }
    }
}
