using AutoMapper;
using Yemekhane.Business.DTOs;
using Yemekhane.Entities;

public class MealProfile : Profile
{
    public MealProfile()
    {
        // Entity -> DTO
        CreateMap<Meal, MealDto>()
            .ForMember(d => d.AverageScore,
                opt => opt.MapFrom(s => s.Ratings.Count == 0
                    ? 0
                    : s.Ratings.Average(r => r.Score)))
            .ForMember(d => d.RatingsCount,
                opt => opt.MapFrom(s => s.Ratings.Count));

        // DTO -> Entity (update/add için)
        CreateMap<MealDto, Meal>()
            .ForMember(d => d.Id, opt => opt.Ignore())        // identity
            .ForMember(d => d.Ratings, opt => opt.Ignore());  // ratings'i servis yönetir
    }
}
