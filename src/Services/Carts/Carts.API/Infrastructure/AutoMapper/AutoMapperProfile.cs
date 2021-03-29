using System;
using AutoMapper;
using ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.RequestModels;
using ZhangJian.YunFeiShop.Services.Carts.Application.Commands;

namespace ZhangJian.YunFeiShop.Services.Carts.API.Infrastructure.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            SourceMemberNamingConvention = new ExactMatchNamingConvention();
            DestinationMemberNamingConvention = new ExactMatchNamingConvention();

            CreateMap<UpdateCartItemRequest, UpdateCartItemCommand>()
                .ForMember(dest => dest.BuyerId, options => options.MapFrom<UserIdentityResolver>());
            CreateMap<RemoveCartItemsRequest, RemoveCartItemsCommand>()
                .ForMember(dest => dest.BuyerId, options => options.MapFrom<UserIdentityResolver>());
        }
    }
}