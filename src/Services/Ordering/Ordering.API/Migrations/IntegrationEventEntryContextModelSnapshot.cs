﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence;

namespace Ordering.API.Migrations
{
    [DbContext(typeof(IntegrationEventEntryContext))]
    partial class IntegrationEventEntryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("ZhangJian.YunFeiShop.BuildingBlocks.IntegrationEvents.Persistence.IntegrationEventEntry", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("EventTypeName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TimesSent")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TransactionId")
                        .HasColumnType("TEXT");

                    b.HasKey("EventId");

                    b.ToTable("IntegrationEventEntries");
                });
#pragma warning restore 612, 618
        }
    }
}