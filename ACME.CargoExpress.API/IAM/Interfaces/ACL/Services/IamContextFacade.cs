using ACME.CargoExpress.API.IAM.Domain.Model.Queries;
using ACME.CargoExpress.API.IAM.Domain.Services;

namespace ACME.CargoExpress.API.IAM.Interfaces.ACL.Services;

public class IamContextFacade(IUserQueryService userQueryService) : IIamContextFacade
{
    public async Task<int> FetchUserIdByUsername(string username)
    {
        var getUserByUsernameQuery = new GetUserByUsernameQuery(username);
        var result = await userQueryService.Handle(getUserByUsernameQuery);
        return result?.Id ?? 0;
    }

    public async Task<string> FetchUsernameByUserId(int userId)
    {
        var getUserByIdQuery = new GetUserByIdQuery(userId);
        var result = await userQueryService.Handle(getUserByIdQuery);
        return result?.Username ?? string.Empty;
    }
}