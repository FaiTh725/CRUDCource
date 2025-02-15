﻿// <auto-generated />
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
    [Migration("20241212122100_Initialize")]
    partial class Initialize
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccountProduct", b =>
                {
                    b.Property<long>("AccountInShopingCartId")
                        .HasColumnType("bigint");

                    b.Property<long>("ShopingCartId")
                        .HasColumnType("bigint");

                    b.HasKey("AccountInShopingCartId", "ShopingCartId");

                    b.HasIndex("ShopingCartId");

                    b.ToTable("AccountProduct");
                });

            modelBuilder.Entity("AccountProduct1", b =>
                {
                    b.Property<long>("AccountInShopingHistoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("ShopingHistoryId")
                        .HasColumnType("bigint");

                    b.HasKey("AccountInShopingHistoryId", "ShopingHistoryId");

                    b.HasIndex("ShopingHistoryId");

                    b.ToTable("AccountProduct1");
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

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Product.Domain.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<long?>("SealerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("SealerId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("AccountProduct", b =>
                {
                    b.HasOne("Product.Domain.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountInShopingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Product.Domain.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ShopingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountProduct1", b =>
                {
                    b.HasOne("Product.Domain.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("AccountInShopingHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Product.Domain.Models.Product", null)
                        .WithMany()
                        .HasForeignKey("ShopingHistoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Product.Domain.Models.Product", b =>
                {
                    b.HasOne("Product.Domain.Models.Account", "Sealer")
                        .WithMany("SoldProducts")
                        .HasForeignKey("SealerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Sealer");
                });

            modelBuilder.Entity("Product.Domain.Models.Account", b =>
                {
                    b.Navigation("SoldProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
