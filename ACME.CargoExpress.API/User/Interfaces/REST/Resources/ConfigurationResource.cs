namespace ACME.CargoExpress.API.User.Interfaces.REST.Resources;

public record ConfigurationResource(int Id,int UserId, string Theme, string View, bool AllowDataCollection, bool UpdateDataSharing);