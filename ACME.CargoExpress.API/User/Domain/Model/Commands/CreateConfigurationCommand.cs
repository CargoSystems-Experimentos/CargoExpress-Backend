namespace ACME.CargoExpress.API.User.Domain.Model.Commands;

public record CreateConfigurationCommand(int UserId, string Theme, string View, bool AllowDataCollection, bool UpdateDataSharing);