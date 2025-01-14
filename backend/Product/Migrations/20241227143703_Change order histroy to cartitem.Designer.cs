﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Product.Dal;

#nullable disable

namespace Product.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241227143703_Change order histroy to cartitem")]
    partial class Changeorderhistroytocartitem
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Product.Domain.Entities.CartItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("AccountId")
                        .HasColumnType("bigint");

                    b.Property<long?>("AccountId1")
                        .HasColumnType("bigint");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("AccountId1");

                    b.HasIndex("ProductId");

                    b.ToTable("CartItems");
                });

            modelBuilder.Entity("Product.Domain.Entities.ChangeAccountRoleRequest", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<bool?>("IsCommite")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReviewed")
                        .HasColumnType("bit");

                    b.Property<string>("NewRole")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ChangeRoleRequests");
                });

            modelBuilder.Entity("Product.Domain.Models.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Product.Domain.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long>("SealerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SealerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Product.Domain.Entities.CartItem", b =>
                {
                    b.HasOne("Product.Domain.Models.Account", null)
                        .WithMany("ShopingCart")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Product.Domain.Models.Account", null)
                        .WithMany("ShopingHistory")
                        .HasForeignKey("AccountId1")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Product.Domain.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Product.Domain.Models.Product", b =>
                {
                    b.HasOne("Product.Domain.Models.Account", "Sealer")
                        .WithMany("SoldProducts")
                        .HasForeignKey("SealerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Sealer");
                });

            modelBuilder.Entity("Product.Domain.Models.Account", b =>
                {
                    b.Navigation("ShopingCart");

                    b.Navigation("ShopingHistory");

                    b.Navigation("SoldProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
