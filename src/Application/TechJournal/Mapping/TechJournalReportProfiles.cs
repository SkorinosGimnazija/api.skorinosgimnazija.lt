namespace SkorinosGimnazija.Application.TechJournal.Mapping;

using AutoMapper;
using Domain.Entities.TechReports;
using Dtos;

public class TechJournalReportProfiles : Profile
{
    public TechJournalReportProfiles()
    {
        CreateMap<TechJournalReport, TechJournalReportDto>()
            .ForMember(x => x.UserDisplayName, x => x.MapFrom(u => u.User.DisplayName));

        CreateMap<TechJournalReportCreateDto, TechJournalReport>()
            .ForMember(x => x.ReportDate, x => x.MapFrom(_ => DateTime.UtcNow));

        CreateMap<TechJournalReportEditDto, TechJournalReport>();

        CreateMap<TechJournalReportPatchDto, TechJournalReport>()
            .ForMember(x => x.FixDate, x => x.MapFrom(_ => DateTime.UtcNow));
    }
}