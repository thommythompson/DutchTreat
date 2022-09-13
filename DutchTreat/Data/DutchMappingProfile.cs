using AutoMapper;
using DutchTreat.Data.Entities;
using DutchTreat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchMappingProfile : Profile
    {
        public DutchMappingProfile()
        {
            CreateMap<Order, OrderModel>()
                .ForMember(o => o.OrderId, ex => ex.MapFrom(o => o.Id))
                .ForMember(o => o.OrderItems, ex => ex.MapFrom(o => o.Items))
                .ReverseMap();

            CreateMap<OrderItem, OrderItemModel>()
                .ReverseMap();
        }
    }
}
