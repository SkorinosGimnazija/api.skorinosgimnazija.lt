namespace SkorinosGimnazija.Application.BullyJournal.Mapping;

using AutoMapper;
using SkorinosGimnazija.Application.Accomplishments.Dtos;
using SkorinosGimnazija.Domain.Entities.Accomplishments;
using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BullyReports.Dtos;
using SkorinosGimnazija.Application.BullyJournal.Dtos;

public class BullyJournalReportProfiles : Profile
{
    public BullyJournalReportProfiles()
    {
        CreateMap<BullyJournalReport, BullyJournalReportDto>()
            .ForMember(x => x.UserDisplayName, x => x.MapFrom(u => u.User.DisplayName))
            .ForMember(x => x.Date, x => x.MapFrom(c => c.Date.ToDateTime(TimeOnly.MinValue)));

        CreateMap<BullyJournalReport, BullyJournalReportDetailsDto>()
            .ForMember(x => x.UserDisplayName, x => x.MapFrom(u => u.User.DisplayName))
            .ForMember(x => x.Date, x => x.MapFrom(c => c.Date.ToDateTime(TimeOnly.MinValue)));

        CreateMap<BullyJournalReportCreateDto, BullyJournalReport>()
            .ForMember(x => x.Date, x => x.MapFrom(c => DateOnly.FromDateTime(c.Date)));

        CreateMap<BullyJournalReportEditDto, BullyJournalReport>()
            .ForMember(x => x.Date, x => x.MapFrom(c => DateOnly.FromDateTime(c.Date)));
    }
}
