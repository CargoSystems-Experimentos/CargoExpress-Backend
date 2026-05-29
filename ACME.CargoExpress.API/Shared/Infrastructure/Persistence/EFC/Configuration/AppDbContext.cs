
using ACME.CargoExpress.API.Registration.Domain.Model.Aggregates;
using ACME.CargoExpress.API.Registration.Domain.Model.Entities;
using ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration.Extensions;
using ACME.CargoExpress.API.User.Domain.Model.Aggregates;
using ACME.CargoExpress.API.User.Domain.Model.Entities;
using EntityFrameworkCore.CreatedUpdatedDate.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ACME.CargoExpress.API.Shared.Infrastructure.Persistence.EFC.Configuration;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
        // Enable Audit Fields Interceptors
        builder.AddCreatedUpdatedInterceptor();
    }
    
    public DbSet<IAM.Domain.Model.Aggregates.User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Entrepreneur> Entrepreneurs { get; set; }
    public DbSet<User.Domain.Model.Entities.Configuration> Configurations { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Expense> Expenses { get; set; }
    public DbSet<OngoingTrip> OngoingTrips { get; set; }
    public DbSet<Alert> Alerts { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Registration Context
        
        //Driver Table
        builder.Entity<Driver>().HasKey(d => d.Id);
        builder.Entity<Driver>().Property(d => d.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Driver>().Property(d => d.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Driver>().Property(d => d.Dni).IsRequired().HasMaxLength(8);
        builder.Entity<Driver>().Property(d => d.License).IsRequired().HasMaxLength(10);
        builder.Entity<Driver>().Property(d => d.ContactNumber).IsRequired().HasMaxLength(9);
        builder.Entity<Driver>()
            .HasOne(d => d.Entrepreneur)
            .WithMany(e => e.Drivers)
            .HasForeignKey(d => d.EntrepreneurId)
            .HasPrincipalKey(e => e.Id);
        
        //Vehicle Table
        builder.Entity<Vehicle>().HasKey(v => v.Id);
        builder.Entity<Vehicle>().Property(v => v.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Vehicle>().Property(v => v.Model).IsRequired().HasMaxLength(100);
        builder.Entity<Vehicle>().Property(v => v.TractorPlate).IsRequired().HasMaxLength(100);
        builder.Entity<Vehicle>().Property(v => v.MaxLoad).IsRequired().HasPrecision(6, 2);
        builder.Entity<Vehicle>().Property(v => v.Volume).IsRequired().HasPrecision(6, 2);
        builder.Entity<Vehicle>()
            .HasOne(v => v.Entrepreneur)
            .WithMany(e => e.Vehicles)
            .HasForeignKey(v => v.EntrepreneurId)
            .HasPrincipalKey(e => e.Id);
        
        
        //Trip Table
        builder.Entity<Trip>().HasKey(t => t.Id);
        builder.Entity<Trip>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Trip>().Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Trip>().Property(t => t.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Trip>().Property(t => t.Weight).IsRequired();
        builder.Entity<Trip>().Property(t => t.LoadLocation).IsRequired().HasMaxLength(100);
        builder.Entity<Trip>().Property(t => t.LoadDate).IsRequired();
        builder.Entity<Trip>().Property(t => t.UnloadLocation).IsRequired().HasMaxLength(100);
        builder.Entity<Trip>().Property(t => t.UnloadDate).IsRequired();
        builder.Entity<Trip>().Property(t => t.EvidenceImg);
        
        //Expense Table
        builder.Entity<Expense>().HasKey(e => e.Id);
        builder.Entity<Expense>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Expense>().Property(e => e.FuelAmount).IsRequired();
        builder.Entity<Expense>().Property(e => e.FuelDescription).IsRequired();
        builder.Entity<Expense>().Property(e => e.ViaticsAmount).IsRequired();
        builder.Entity<Expense>().Property(e => e.ViaticsDescription).IsRequired();
        builder.Entity<Expense>().Property(e => e.TollsAmount).IsRequired();
        builder.Entity<Expense>().Property(e => e.TollsDescription).IsRequired();
        
        //Alert Table
        builder.Entity<Alert>().HasKey(a => a.Id);
        builder.Entity<Alert>().Property(a => a.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Alert>().Property(a => a.Title).IsRequired().HasMaxLength(20);
        builder.Entity<Alert>().Property(a => a.Description).IsRequired();
        builder.Entity<Alert>().Property(a => a.Date).IsRequired();
        
        //OngoingTrip Table
        builder.Entity<OngoingTrip>().HasKey(ot => ot.Id);
        builder.Entity<OngoingTrip>().Property(ot => ot.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<OngoingTrip>().Property(ot => ot.Latitude).IsRequired();
        builder.Entity<OngoingTrip>().Property(ot => ot.Longitude).IsRequired();
        builder.Entity<OngoingTrip>().Property(ot => ot.Speed).IsRequired();
        builder.Entity<OngoingTrip>().Property(ot => ot.Distance).IsRequired();
        builder.Entity<OngoingTrip>().Property(ot => ot.State).IsRequired().HasMaxLength(20);
        
        //Trips Table Relationships
        builder.Entity<Trip>()
            .HasOne(t => t.Client)
            .WithMany(c => c.Trips)
            .HasForeignKey(t => t.ClientId)
            .HasPrincipalKey(c => c.Id);
        
        builder.Entity<Trip>()
            .HasOne(t => t.Entrepreneur)
            .WithMany(e => e.Trips)
            .HasForeignKey(t => t.EntrepreneurId)
            .HasPrincipalKey(e => e.Id);
        
        builder.Entity<Trip>()
            .HasOne(t => t.Driver)
            .WithMany(d => d.Trips)
            .HasForeignKey(t => t.DriverId)
            .HasPrincipalKey(d => d.Id);

        builder.Entity<Trip>()
            .HasOne(t => t.Vehicle)
            .WithMany(v => v.Trips)
            .HasForeignKey(t => t.VehicleId)
            .HasPrincipalKey(v => v.Id);
        
        
        //Expenses Table Relationships
        builder.Entity<Expense>()
            .HasOne(e => e.Trip)
            .WithOne(t => t.Expense)
            .HasForeignKey<Expense>(e => e.TripId)
            .HasPrincipalKey<Trip>(t => t.Id);
        
        //Alerts Table Relationships
        builder.Entity<Alert>()
            .HasOne(a => a.OngoingTrip)
            .WithMany()
            .HasForeignKey(a => a.OngoingTripId)
            .HasPrincipalKey(ot => ot.Id);
        
        //OngoingTrips Table Relationships
        builder.Entity<OngoingTrip>()
            .HasOne(ot => ot.Trip)
            .WithOne(t => t.OngoingTrip)
            .HasForeignKey<OngoingTrip>(ot => ot.TripId)
            .HasPrincipalKey<Trip>(t => t.Id);
        
        // IAM Bounded Context
        
        builder.Entity<IAM.Domain.Model.Aggregates.User>().HasKey(u => u.Id);
        builder.Entity<IAM.Domain.Model.Aggregates.User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<IAM.Domain.Model.Aggregates.User>().Property(u => u.Username).IsRequired();
        builder.Entity<IAM.Domain.Model.Aggregates.User>().HasIndex(u => u.Username).IsUnique();
        builder.Entity<IAM.Domain.Model.Aggregates.User>().Property(u => u.PasswordHash).IsRequired();
        builder.Entity<IAM.Domain.Model.Aggregates.User>().Property(u => u.Phone).IsRequired().HasMaxLength(9);
        builder.Entity<IAM.Domain.Model.Aggregates.User>().HasIndex(u => u.Phone).IsUnique();
        builder.Entity<IAM.Domain.Model.Aggregates.User>().Property(u => u.CreatedAt).IsRequired();

        //User Bounded Context
        
        //Client table
        builder.Entity<Client>().HasKey(c => c.Id);
        builder.Entity<Client>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        
        //Client table relationships
        builder.Entity<Client>()
            .HasOne(c => c.User)
            .WithOne(u => u.Client)
            .HasForeignKey<Client>(c => c.UserId)
            .HasPrincipalKey<IAM.Domain.Model.Aggregates.User>(u => u.Id);
        
       
        //Entrepreneur table
        builder.Entity<Entrepreneur>().HasKey(e => e.Id);
        builder.Entity<Entrepreneur>().Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Entrepreneur>().Property(e => e.Name).IsRequired().HasMaxLength(60);
        builder.Entity<Entrepreneur>().Property(e => e.Ruc).IsRequired().HasMaxLength(11);
        builder.Entity<Entrepreneur>().HasIndex(e => e.Name).IsUnique();
        builder.Entity<Entrepreneur>().HasIndex(e => e.Ruc).IsUnique();

        //Entrepreneur table relationships

        builder.Entity<Entrepreneur>()
            .HasOne(e => e.User)
            .WithOne(u => u.Entrepreneur)
            .HasForeignKey<Entrepreneur>(e => e.UserId)
            .HasPrincipalKey<IAM.Domain.Model.Aggregates.User>(u => u.Id);
        
        // Configuration table
        
       builder.Entity<User.Domain.Model.Entities.Configuration>().HasKey(c => c.Id);
       builder.Entity<User.Domain.Model.Entities.Configuration>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
       builder.Entity<User.Domain.Model.Entities.Configuration>().Property(c => c.Theme).IsRequired().HasMaxLength(100);
       builder.Entity<User.Domain.Model.Entities.Configuration>().Property(c => c.View).IsRequired().HasMaxLength(100);
       builder.Entity<User.Domain.Model.Entities.Configuration>().Property(c => c.AllowDataCollection).IsRequired();
       builder.Entity<User.Domain.Model.Entities.Configuration>().Property(c => c.UpdateDataSharing).IsRequired();
       // Configuration table relationships
       
       builder.Entity<User.Domain.Model.Entities.Configuration>()
           .HasOne(c => c.User)
           .WithOne(u => u.Configuration)
           .HasForeignKey<User.Domain.Model.Entities.Configuration>(c => c.UserId)
           .HasPrincipalKey<IAM.Domain.Model.Aggregates.User>(u => u.Id);
       
        // Apply SnakeCase Naming Convention
        builder.UseSnakeCaseWithPluralizedTableNamingConvention();
    }
}