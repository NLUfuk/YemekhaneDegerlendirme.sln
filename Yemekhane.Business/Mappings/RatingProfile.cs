using AutoMapper;
using Yemekhane.Business.DTOs;
using Yemekhane.Entities;

public class RatingProfile : Profile
{
    public RatingProfile()
    {
        // Entity -> DTO (liste/okuma)
        CreateMap<Rating, RatingDto>();

        // Create (nested route: POST /api/meals/{mealId}/rate)
        // MealId ve UserId servis/route/claims'ten set edilecek
        CreateMap<RatingDto, Rating>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ForMember(d => d.MealId, opt => opt.Ignore())
            .ForMember(d => d.UserId, opt => opt.Ignore())
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));
    }
}
