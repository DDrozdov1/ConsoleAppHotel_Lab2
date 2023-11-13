using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ConsoleAppHotel_Lab2.Models;

public partial class HotelDbContext : DbContext
{
    public HotelDbContext()
    {
    }

    public HotelDbContext(DbContextOptions<HotelDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<ClientService> ClientServices { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<HotelService> HotelServices { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomPrice> RoomPrices { get; set; }

    public virtual DbSet<RoomService> RoomServices { get; set; }

    public virtual DbSet<RoomsWithPrice> RoomsWithPrices { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ConfigurationBuilder builder = new();
        // установка пути к текущему каталогу
        builder.SetBasePath(Directory.GetCurrentDirectory());
        // получаем конфигурацию из файла appsettings.json
        builder.AddJsonFile("appsettings.json");
        // создаем конфигурацию
        IConfigurationRoot config = builder.Build();
        // получаем строку подключения
        string connectionString = config.GetConnectionString("DefaultConnection");
        _ = optionsBuilder
            .UseSqlServer(connectionString)
            .Options;
        optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));

    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.ClientId).HasName("PK__Clients__E67E1A24EFFF09D7");

            entity.Property(e => e.ClientId).ValueGeneratedNever();
            entity.Property(e => e.CheckInDate).HasColumnType("date");
            entity.Property(e => e.CheckOutDate).HasColumnType("date");
            entity.Property(e => e.ClientFullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ClientPassportDetails)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Room).WithMany(p => p.Clients)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__Clients__RoomId__18EBB532");
        });

        modelBuilder.Entity<ClientService>(entity =>
        {
            entity.HasKey(e => e.ClientServiceId).HasName("PK__ClientSe__A414866D0BA3B8E3");

            entity.Property(e => e.ClientServiceId).ValueGeneratedNever();
            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Client).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("FK__ClientSer__Clien__14270015");

            entity.HasOne(d => d.HotelService).WithMany(p => p.ClientServices)
                .HasForeignKey(d => d.HotelServiceId)
                .HasConstraintName("FK__ClientSer__Hotel__151B244E");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF11E185B72");

            entity.Property(e => e.EmployeeId)
                .ValueGeneratedNever()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.EmployeeFullName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EmployeePosition)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<HotelService>(entity =>
        {
            entity.HasKey(e => e.HotelServiceid).HasName("PK__HotelSer__0DE9E71510922B97");

            entity.Property(e => e.HotelServiceid).ValueGeneratedNever();
            entity.Property(e => e.HotelServiceCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.HotelServiceDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.HotelServiceName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.RoomId).HasName("PK__Rooms__32863939C3E5CE4E");

            entity.Property(e => e.RoomId).ValueGeneratedNever();
            entity.Property(e => e.RoomDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<RoomPrice>(entity =>
        {
            entity.HasKey(e => e.RoomPriceId).HasName("PK__RoomPric__CA1979843328A28A");

            entity.Property(e => e.RoomPriceId).ValueGeneratedNever();
            entity.Property(e => e.RoomCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomPrices)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__RoomPrice__RoomI__17F790F9");
        });

        modelBuilder.Entity<RoomService>(entity =>
        {
            entity.HasKey(e => e.RoomServiceId).HasName("PK__RoomServ__A11E84C152321727");

            entity.ToTable("RoomService");

            entity.Property(e => e.RoomServiceId).ValueGeneratedNever();

            entity.HasOne(d => d.Employee).WithMany(p => p.RoomServices)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__RoomServi__Emplo__17036CC0");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomServices)
                .HasForeignKey(d => d.RoomId)
                .HasConstraintName("FK__RoomServi__RoomI__160F4887");
        });

        modelBuilder.Entity<RoomsWithPrice>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("RoomsWithPrices");

            entity.Property(e => e.RoomCost).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RoomDescription)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.RoomType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
