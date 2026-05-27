namespace ACME.CargoExpress.API.User.Domain.Model.Entities;

public class Configuration
{
    public Configuration()
    {
        User = new IAM.Domain.Model.Aggregates.User();
        Theme = string.Empty;
        View = string.Empty;
        AllowDataCollection = false;
        UpdateDataSharing = false;
    }

    public Configuration(int userId, string theme, string view, bool allowDataCollection, bool updateDataSharing, IAM.Domain.Model.Aggregates.User user)
    {
        UserId = userId;
        Theme = theme;
        View = view;
        AllowDataCollection = allowDataCollection;
        UpdateDataSharing = updateDataSharing;
        User = user;
    }
    
    public int Id { get; set; }
    public IAM.Domain.Model.Aggregates.User User { get; set; }
    public int UserId { get; set; }
    public string Theme { get; set; }
    public string View { get; set; }
    public bool AllowDataCollection { get; set; }
    public bool UpdateDataSharing { get; set; }
}