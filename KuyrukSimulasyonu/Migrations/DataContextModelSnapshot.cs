﻿// <auto-generated />
using System;
using KuyrukSimulasyonu.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KuyrukSimulasyonu.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.7");

            modelBuilder.Entity("KuyrukSimulasyonu.Entities.Kuyruk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BeklemeNoktasi")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("FotoTarihi")
                        .HasColumnType("TEXT");

                    b.Property<int?>("KuyrukSuresi")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Kuyruklar");
                });

            modelBuilder.Entity("KuyrukSimulasyonu.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
