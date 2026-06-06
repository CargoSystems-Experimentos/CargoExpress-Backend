namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record UpdateExpenseStateCommand(int ExpenseId, bool State);
