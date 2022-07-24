namespace SkorinosGimnazija.Infrastructure.Revalidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IRevalidationService
{
    Task<bool> RevalidateAsync(string language, string slug, int postId);

    Task<bool> RevalidateAsync(string language);
}
