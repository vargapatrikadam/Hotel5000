﻿// <auto-generated />
using System;
using Infrastructure.Lodgings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Lodgings.Migrations
{
    [DbContext(typeof(LodgingDbContext))]
    [Migration("20200302184920_extended_uq_index_on_lodging_address")]
    partial class extended_uq_index_on_lodging_address
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Core.Entities.LodgingEntities.ApprovingData", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("IdentityNumber")
                        .HasColumnType("nvarchar(8)")
                        .HasMaxLength(8);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RegistrationNumber")
                        .HasColumnType("nvarchar(12)")
                        .HasMaxLength(12);

                    b.Property<string>("TaxNumber")
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("ApprovingData_PK");

                    b.HasIndex("IdentityNumber")
                        .IsUnique()
                        .HasName("ApprovingData_IdentityNumber_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.HasIndex("RegistrationNumber")
                        .IsUnique()
                        .HasName("ApprovingData_RegistrationNumber_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.HasIndex("TaxNumber")
                        .IsUnique()
                        .HasName("ApprovingData_TaxNumber_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ApprovingData");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MobileNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(13)")
                        .HasMaxLength(13);

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("Contact_PK");

                    b.HasIndex("MobileNumber")
                        .IsUnique()
                        .HasName("Contact_MobileNumber_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.HasIndex("UserId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(2)")
                        .HasMaxLength(2);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("Country_PK");

                    b.HasIndex("Code")
                        .IsUnique()
                        .HasName("Country_CountyCode_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("Country_CountryName_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.HasKey("Id")
                        .HasName("Currency_PK");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("Currency_Name_UQ");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Lodging", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LodgingTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("Lodging_PK");

                    b.HasIndex("LodgingTypeId");

                    b.HasIndex("UserId");

                    b.ToTable("Lodgings");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.LodgingAddress", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<string>("County")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("DoorNumber")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Floor")
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("HouseNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LodgingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("LodgingAddress_PK");

                    b.HasIndex("LodgingId");

                    b.HasIndex("CountryId", "County", "City", "PostalCode", "Street", "HouseNumber", "Floor", "DoorNumber")
                        .IsUnique()
                        .HasName("LodgingAddress_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("LodgingAddresses");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.LodgingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id")
                        .HasName("LodgingType_PK");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("LodgingType_Name_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("LodgingTypes");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.PaymentType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id")
                        .HasName("PaymentType_PK");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("PaymentType_Name_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("PaymentTypes");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("PaymentTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("Reservation_PK");

                    b.HasIndex("PaymentTypeId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.ReservationItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReservationId")
                        .HasColumnType("int");

                    b.Property<int>("ReservationWindowId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReservedFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ReservedTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("RoomId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("ReservationItem_PK");

                    b.HasIndex("ReservationId");

                    b.HasIndex("ReservationWindowId");

                    b.HasIndex("RoomId");

                    b.ToTable("ReservationItems");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.ReservationWindow", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("From")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LodgingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("To")
                        .HasColumnType("datetime2");

                    b.HasKey("Id")
                        .HasName("ReservationWindow_PK");

                    b.HasIndex("LodgingId");

                    b.ToTable("ReservationWindows");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id")
                        .HasName("Role_PK");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasName("Role_Name_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("AdultCapacity")
                        .HasColumnType("int");

                    b.Property<int>("ChildrenCapacity")
                        .HasColumnType("int");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("LodgingId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.HasKey("Id")
                        .HasName("Room_PK");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("LodgingId");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Token", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(45)")
                        .HasMaxLength(45);

                    b.Property<DateTime>("UsableFrom")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("Token_PK");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AddedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(255)")
                        .HasMaxLength(255);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.HasKey("Id")
                        .HasName("User_PK");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasName("User_Email_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.HasIndex("RoleId");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasName("User_Username_UQ")
                        .HasFilter("IsDeleted = 0");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.ApprovingData", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.User", "User")
                        .WithOne("ApprovingData")
                        .HasForeignKey("Core.Entities.LodgingEntities.ApprovingData", "UserId")
                        .HasConstraintName("ApprovingData_User_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Contact", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.User", "User")
                        .WithMany("Contacts")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Contact_User_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Lodging", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.LodgingType", "LodgingType")
                        .WithMany("Lodgings")
                        .HasForeignKey("LodgingTypeId")
                        .HasConstraintName("Lodging_LodgingType_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.LodgingEntities.User", "User")
                        .WithMany("Lodgings")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Lodging_User_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.LodgingAddress", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.Country", "Country")
                        .WithMany("LodgingAddresses")
                        .HasForeignKey("CountryId")
                        .HasConstraintName("LodgingAddress_Country_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.LodgingEntities.Lodging", "Lodging")
                        .WithMany("LodgingAddresses")
                        .HasForeignKey("LodgingId")
                        .HasConstraintName("LodgingAddress_Lodging_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Reservation", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.PaymentType", "PaymentType")
                        .WithMany("UserReservations")
                        .HasForeignKey("PaymentTypeId")
                        .HasConstraintName("UserReservation_PaymentType_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.ReservationItem", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.Reservation", "Reservation")
                        .WithMany("ReservationItems")
                        .HasForeignKey("ReservationId")
                        .HasConstraintName("ReservationItem_Reservation_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Entities.LodgingEntities.ReservationWindow", "ReservationWindow")
                        .WithMany("ReservationItems")
                        .HasForeignKey("ReservationWindowId")
                        .HasConstraintName("ReservationItem_ReservationWindow_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Entities.LodgingEntities.Room", "Room")
                        .WithMany("ReservationItems")
                        .HasForeignKey("RoomId")
                        .HasConstraintName("ReservationItem_Room_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.ReservationWindow", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.Lodging", "Lodging")
                        .WithMany("ReservationWindows")
                        .HasForeignKey("LodgingId")
                        .HasConstraintName("ReservationWindow_Lodging_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Room", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.Currency", "Currency")
                        .WithMany("Rooms")
                        .HasForeignKey("CurrencyId")
                        .HasConstraintName("Room_Currency_FK")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Core.Entities.LodgingEntities.Lodging", "Lodging")
                        .WithMany("Rooms")
                        .HasForeignKey("LodgingId")
                        .HasConstraintName("Room_Lodging_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.Token", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .HasConstraintName("Token_User_Id_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Core.Entities.LodgingEntities.User", b =>
                {
                    b.HasOne("Core.Entities.LodgingEntities.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("User_Role_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
