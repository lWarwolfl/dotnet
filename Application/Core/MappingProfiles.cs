using Application.Activities.DTOs;
using Application.Profiles.DTOs;
using AutoMapper;
using Domain;

namespace Application.Core;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        string? currentUserId = null;

        CreateMap<Activity, Activity>();
        CreateMap<CreateActivityDto, Activity>();
        CreateMap<EditActivityDto, Activity>();
        CreateMap<Activity, GetActivityDto>()
            .ForMember(d => d.HostId, o => o.MapFrom(s => s.Attendees.FirstOrDefault(x => x.IsHost)!.UserId));
        CreateMap<ActivityAttendee, ProfileDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
            .ForMember(d => d.Id, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl)).ForMember(d => d.Following, o => o.MapFrom(s => s.User.Followers.Any(x => x.Observer.Id == currentUserId)));
        CreateMap<User, ProfileDto>().ForMember(d => d.FollowerCount, o => o.MapFrom(s => s.Followers.Count)).ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count)).ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(x => x.Observer.Id == currentUserId)));
        CreateMap<User, UserInfoDto>();
        CreateMap<Comment, GetCommentDto>()
            .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
            .ForMember(d => d.UserId, o => o.MapFrom(s => s.User.Id))
            .ForMember(d => d.ImageUrl, o => o.MapFrom(s => s.User.ImageUrl));

    }
}
