namespace ACME.CargoExpress.API.Registration.Domain.Model.Commands;

public record CreateDriverCommand(string Name, string Dni, string License, string ContactNumber, int EntrepreneurId);