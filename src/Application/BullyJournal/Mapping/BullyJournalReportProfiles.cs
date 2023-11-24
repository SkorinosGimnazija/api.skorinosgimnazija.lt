namespace SkorinosGimnazija.Application.BullyJournal.Mapping;

using AutoMapper;
using BullyReports.Dtos;
using Domain.Entities.Bullies;
using Dtos;

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