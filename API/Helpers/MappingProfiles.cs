using API.Dtos;
using API.Entities;
using AutoMapper;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Employee, DetailedEmployeeDto>();
            CreateMap<IncomeDetails, IncomeDetailsDto>();
            CreateMap<Employee, EmployeeDto>();
        }
    }
}
