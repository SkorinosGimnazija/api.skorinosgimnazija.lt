namespace SkorinosGimnazija.Application.BullyReports.Mapping;

using AutoMapper;
using Domain.Entities.Bullies;
using Dtos;

public class BullyReportProfiles : Profile
{
    public BullyReportProfiles()
    {
        CreateMap<BullyReport, BullyReportDto>();

        CreateMap<BullyReportCreateDto, BullyReport>();
    }
}