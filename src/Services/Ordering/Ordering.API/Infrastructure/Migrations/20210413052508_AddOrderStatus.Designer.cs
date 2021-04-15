﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZhangJian.YunFeiShop.Services.Ordering.Infrastructure;

namespace Ordering.API.Infrastructure.Migrations
{
    [DbContext(typeof(OrderingContext))]
    [Migration("20210413052508_AddOrderStatus")]
    partial class AddOrderStatus
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("ZhangJian.YunFeiShop.BuildingBlocks.SeedWork.Infrastructure.Idempotency.ClientRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("BuyerId")
                        .HasColumnType("TEXT");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("orders");
                });

            modelBuilder.Entity("ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate.OrderLine", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("orderLines");
                });

            modelBuilder.Entity("ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate.OrderLine", b =>
                {
                    b.HasOne("ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate.Order", null)
                        .WithMany("OrderLines")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("ZhangJian.YunFeiShop.Services.Ordering.Domain.AggregatesModel.OrderAggregate.Order", b =>
                {
                    b.Navigation("OrderLines");
                });
#pragma warning restore 612, 618
        }
    }
}
