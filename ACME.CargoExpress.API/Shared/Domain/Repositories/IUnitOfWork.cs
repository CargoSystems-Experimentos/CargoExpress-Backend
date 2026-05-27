namespace ACME.CargoExpress.API.Shared.Domain.Repositories;

public interface IUnitOfWork
{
    Task CompleteAsync();
}