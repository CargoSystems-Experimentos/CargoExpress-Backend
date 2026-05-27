namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record UpdateConfigurationResource(string Theme, string View, bool AllowDataCollection, bool UpdateDataSharing);