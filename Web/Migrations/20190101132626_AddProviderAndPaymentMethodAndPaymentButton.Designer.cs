﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web;

namespace Web.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20190101132626_AddProviderAndPaymentMethodAndPaymentButton")]
    partial class AddProviderAndPaymentMethodAndPaymentButton
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Web.Model.Domain.PaymentButton", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<DateTime>("CreationDate");

                    b.Property<long?>("MethodId");

                    b.HasKey("Id");

                    b.HasIndex("MethodId");

                    b.ToTable("PaymentButtons");
                });

            modelBuilder.Entity("Web.Model.Domain.PaymentMethod", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreationTimestamp");

                    b.Property<long?>("ProviderId");

                    b.Property<string>("Token");

                    b.Property<long?>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("ProviderId");

                    b.HasIndex("UserId");

                    b.ToTable("PaymentMethods");
                });

            modelBuilder.Entity("Web.Model.Domain.Provider", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Company");

                    b.Property<string>("EndPoint");

                    b.Property<string>("Name");

                    b.Property<string>("PaymentEndpoint");

                    b.HasKey("Id");

                    b.ToTable("Providers");
                });

            modelBuilder.Entity("Web.Model.Domain.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Name");

                    b.Property<string>("Password");

                    b.Property<int>("Role");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Web.Model.Domain.PaymentButton", b =>
                {
                    b.HasOne("Web.Model.Domain.PaymentMethod", "Method")
                        .WithMany()
                        .HasForeignKey("MethodId");
                });

            modelBuilder.Entity("Web.Model.Domain.PaymentMethod", b =>
                {
                    b.HasOne("Web.Model.Domain.Provider", "Provider")
                        .WithMany()
                        .HasForeignKey("ProviderId");

                    b.HasOne("Web.Model.Domain.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });
#pragma warning restore 612, 618
        }
    }
}
