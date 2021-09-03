using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserAccessor
    {
        bool IsAdmin();
        bool HasRole(string role);
        bool IsOwner(int ownerId);
    }
}
