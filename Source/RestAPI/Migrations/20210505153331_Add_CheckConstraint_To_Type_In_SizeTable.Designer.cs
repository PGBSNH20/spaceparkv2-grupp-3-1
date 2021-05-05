﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RestAPI.Data;

namespace RestAPI.Migrations
{
    [DbContext(typeof(SpaceParkDbContext))]
    [Migration("20210505153331_Add_CheckConstraint_To_Type_In_SizeTable")]
    partial class Add_CheckConstraint_To_Type_In_SizeTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RestAPI.Models.Parking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("Arrival")
                        .HasColumnType("datetime2");

                    b.Property<string>("CharacterName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.Property<int>("SpacePortId")
                        .HasColumnType("int");

                    b.Property<string>("SpaceshipName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SizeId");

                    b.HasIndex("SpacePortId");

                    b.ToTable("Parkings");
                });

            modelBuilder.Entity("RestAPI.Models.Receipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Arrival")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Departure")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SizeId")
                        .HasColumnType("int");

                    b.Property<string>("StarshipName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("SizeId");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("RestAPI.Models.Size", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Sizes");

                    b.HasCheckConstraint("CK_Only_Range_Between_1_and_4", "[Type] >= 1 AND [Type] < 5");
                });

            modelBuilder.Entity("RestAPI.Models.SpacePort", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SpacePorts");
                });

            modelBuilder.Entity("RestAPI.Models.Parking", b =>
                {
                    b.HasOne("RestAPI.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RestAPI.Models.SpacePort", null)
                        .WithMany("Parkings")
                        .HasForeignKey("SpacePortId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Size");
                });

            modelBuilder.Entity("RestAPI.Models.Receipt", b =>
                {
                    b.HasOne("RestAPI.Models.Size", "Size")
                        .WithMany()
                        .HasForeignKey("SizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Size");
                });

            modelBuilder.Entity("RestAPI.Models.SpacePort", b =>
                {
                    b.Navigation("Parkings");
                });
#pragma warning restore 612, 618
        }
    }
}