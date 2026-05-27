using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using ACME.CargoExpress.API.User.Interfaces.REST.Resources;

namespace ACME.CargoExpress.API.User.Interfaces.REST.Transform;

public static class EntrepreneurResourceFromEntityAssembler
{
   public static EntrepreneurResource ToResourceFromEntity(Entrepreneur entity)
   {
       return new EntrepreneurResource(
           entity.Id, 
           entity.Name,
           entity.Phone,
           entity.Ruc,
           entity.UserId, 
           entity.LogoImage);
   }
}