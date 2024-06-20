﻿// <auto-generated />
using System;
using Backend.DbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    [DbContext(typeof(ToursDbContext))]
    [Migration("20240603135725_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shared.Models.Tour", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("Distance")
                        .HasColumnType("double precision");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<string>("From")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("To")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TourDescription")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("TransportationType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Tours");
                });

            modelBuilder.Entity("Shared.Models.TourLog", b =>
                {
                    b.Property<int>("IdTourLog")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdTourLog"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Difficulty")
                        .HasColumnType("integer");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<double>("TotalDistance")
                        .HasColumnType("double precision");

                    b.Property<TimeSpan>("TotalTime")
                        .HasColumnType("interval");

                    b.Property<int>("TourId")
                        .HasColumnType("integer");

                    b.HasKey("IdTourLog");

                    b.HasIndex("TourId");

                    b.ToTable("TourLogs");
                });

            modelBuilder.Entity("Shared.Models.TourLog", b =>
                {
                    b.HasOne("Shared.Models.Tour", "Tour")
                        .WithMany()
                        .HasForeignKey("TourId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tour");
                });
#pragma warning restore 612, 618
        }
    }
}
