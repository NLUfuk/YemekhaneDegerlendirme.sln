using AutoMapper;
using Yemekhane.Business.DTOs;
using Yemekhane.Entities;

public class MealSuggestionProfile : Profile
{
    public MealSuggestionProfile()
    {
        // Entity <-> DTO
        CreateMap<MealSuggestion, MealSuggestionDto>().ReverseMap()
            .ForMember(d => d.Id, opt => opt.Ignore()); // identity ise

        //// Oy verme için istek DTO'su kullanıyorsan (opsiyonel)
        //CreateMap<SuggestionVoteDto, MealSuggestion>()  // genelde kullanılmaz
        //    .ForAllMembers(opt => opt.Ignore());        // güvenlik: map etme
    }
}
