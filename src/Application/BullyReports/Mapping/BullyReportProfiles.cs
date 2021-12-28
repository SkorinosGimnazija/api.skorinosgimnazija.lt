namespace SkorinosGimnazija.Application.BullyReports.Mapping;
using AutoMapper;
using SkorinosGimnazija.Application.BullyReports.Dtos;

using SkorinosGimnazija.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Bullies;
using Domain.Entities.Teacher;

public class BullyReportProfiles : Profile
{
    public BullyReportProfiles()
    {
        CreateMap<BullyReport, BullyReportDto>();

        CreateMap<BullyReportCreateDto, BullyReport>();
    }
}
