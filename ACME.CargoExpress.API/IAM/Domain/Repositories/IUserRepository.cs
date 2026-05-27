using ACME.CargoExpress.API.Shared.Domain.Repositories;

namespace ACME.CargoExpress.API.IAM.Domain.Repositories;

/**
 * <summary>
 *     The user repository
 * </summary>
 * <remarks>
 *     This repository is used to manage users
 * </remarks>
 */
public interface IUserRepository : IBaseRepository<Model.Aggregates.User>
{
    /**
     * <summary>
     *     Find a user by id
     * </summary>
     * <param name="username">The username to search</param>
     * <returns>The user</returns>
     */
    Task<Model.Aggregates.User?> FindByUsernameAsync(string username);

    /**
     * <summary>
     *     Check if a user exists by username
     * </summary>
     * <param name="username">The username to search</param>
     * <returns>True if the user exists, false otherwise</returns>
     */
    bool ExistsByUsername(string username);
}