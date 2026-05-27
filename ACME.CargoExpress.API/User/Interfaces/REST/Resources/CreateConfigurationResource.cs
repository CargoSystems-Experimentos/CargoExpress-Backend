namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record CreateConfigurationResource(int UserId, string Theme, string View, bool AllowDataCollection, bool UpdateDataSharing);