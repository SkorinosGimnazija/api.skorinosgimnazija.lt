﻿namespace SkorinosGimnazija.Application.TechJournal.Mapping;

using AutoMapper;
using SkorinosGimnazija.Application.TechJournal.Dtos;
using SkorinosGimnazija.Application.BullyReports.Dtos;
using SkorinosGimnazija.Domain.Entities.Bullies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkorinosGimnazija.Domain.Entities.TechReports;

public class TechJournalReportProfiles : Profile
{
    public TechJournalReportProfiles()
    {
        CreateMap<TechJournalReport, TechJournalReportDto>()
            .ForMember(x => x.UserDisplayName, x => x.MapFrom(u => u.User.DisplayName));

        CreateMap<TechJournalReportCreateDto, TechJournalReport>()
            .ForMember(x => x.ReportDate, x => x.MapFrom(_ => DateTime.UtcNow));

        CreateMap<TechJournalReportEditDto, TechJournalReport>()
            .ForMember(x => x.ReportDate, x => x.MapFrom(_ => DateTime.UtcNow))
            .ForMember(x => x.IsFixed, x => x.MapFrom<bool?>(_ => null))
            .ForMember(x => x.Notes, x => x.MapFrom<string?>(_ => null))
            .ForMember(x => x.FixDate, x => x.MapFrom<DateOnly?>(_ => null));

        CreateMap<TechJournalReportPatchDto, TechJournalReport>()
            .ForMember(x => x.FixDate, x => x.MapFrom(_ => DateTime.UtcNow));
    }
}
