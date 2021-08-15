using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Core.MappingProfiles
{
    using AutoMapper;
    using Domain.CMS;
    using Domains.Dtos;

    public class DomainProfiles :Profile
    {
        public DomainProfiles()
        {
            CreateMap<Domain, Domain>();
            CreateMap<DomainCreateDto, Domain>();
        }
    }
}
