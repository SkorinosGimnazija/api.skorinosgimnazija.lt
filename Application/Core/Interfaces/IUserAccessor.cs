namespace Application.Core.Interfaces;

public interface IUserAccessor
{
    bool IsAdmin();

    bool HasRole(string role);

    bool IsOwner(int ownerId);
}