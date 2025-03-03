using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TravelAgency.Database.AppDbContextModels;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<TravelPackage> TravelPackages { get; set; }

    public virtual DbSet<Traveler> Travelers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Bookings__3213E83F4782F263");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("id");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("booking_date");
            entity.Property(e => e.InvoiceNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("invoice_number");
            entity.Property(e => e.NumberOfTravelers).HasColumnName("number_of_travelers");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('pending')")
                .HasColumnName("status");
            entity.Property(e => e.TotalPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_price");
            entity.Property(e => e.TravelEnddate)
                .HasColumnType("datetime")
                .HasColumnName("travel_enddate");
            entity.Property(e => e.TravelPackageId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("travel_package_id");
            entity.Property(e => e.TravelStartdate)       
                .HasColumnType("datetime")
                .HasColumnName("travel_startdate");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3213E83FC912D91D");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("booking_id");
            entity.Property(e => e.PaymentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("payment_date");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasDefaultValueSql("('pending')")
                .HasColumnName("payment_status");
            entity.Property(e => e.PaymentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_type");
            entity.Property(e => e.UserId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("user_id");
        });

        modelBuilder.Entity<TravelPackage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TravelPa__3213E83F375A2FB2");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");

            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("title");

            entity.Property(e => e.Destination)
                .HasMaxLength(1000) // Fixed: Matched database schema
                .IsUnicode(false)
                .HasColumnName("destination");

            entity.Property(e => e.Description)
                .HasMaxLength(1000) // Fixed: Matched database schema
                .IsUnicode(false)
                .HasColumnName("description");

            entity.Property(e => e.Price)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("price");

            entity.Property(e => e.Duration)
                .HasColumnName("duration");

            entity.Property(e => e.Inclusions)
                .HasMaxLength(1000) // Fixed: Matched database schema
                .IsUnicode(false)
                .HasColumnName("inclusions");

            entity.Property(e => e.Cancellation_Policy)
                .HasMaxLength(1000) // Fixed: Changed from text to varchar(1000)
                .IsUnicode(false)
                .HasColumnName("cancellation_policy");

            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValueSql("('activate')")
                .HasColumnName("status");

            entity.Property(e => e.Image)
                .HasColumnType("varchar(MAX)") // Fixed: Matched database schema
                .IsUnicode(false)
                .HasColumnName("image");

            entity.Property(e => e.Count)
                .HasColumnName("count");
        });


        modelBuilder.Entity<Traveler>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Traveler__3213E83FEAC31D9B");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Age).HasColumnName("age");
            entity.Property(e => e.BookingId)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("booking_id");
            entity.Property(e => e.Gender)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("('male')")
                .HasColumnName("gender");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3213E83FB677C58E");

            entity.HasIndex(e => e.Email, "UQ__Users__AB6E61640802EF5A").IsUnique();

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .IsUnicode(false)
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password_hash");
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Role)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValueSql("('customer')")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
