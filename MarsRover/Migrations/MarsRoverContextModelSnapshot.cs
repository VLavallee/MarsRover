﻿// <auto-generated />
using System;
using MarsRover.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarsRover.Migrations
{
    [DbContext(typeof(MarsRoverContext))]
    partial class MarsRoverContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarsRover.Models.Rover", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Input")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("fDir")
                        .HasColumnType("nvarchar(1)");

                    b.Property<int?>("fPosX")
                        .HasColumnType("int");

                    b.Property<int?>("fPosY")
                        .HasColumnType("int");

                    b.Property<string>("sDir")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.Property<int>("sPosX")
                        .HasColumnType("int");

                    b.Property<int>("sPosY")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Rover");
                });
#pragma warning restore 612, 618
        }
    }
}
