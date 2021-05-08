﻿// <auto-generated />
using DAL_EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAL_EF.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.5")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DAL_EF.Entity.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WalletId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DAL_EF.Entity.Transaction.TransactionBase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SourceWalletId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SourceWalletId");

                    b.ToTable("Transactions");

                    b.HasDiscriminator<string>("Discriminator").HasValue("TransactionBase");
                });

            modelBuilder.Entity("DAL_EF.Entity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GoogleId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PictureUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("GoogleId")
                        .IsUnique()
                        .HasFilter("[GoogleId] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DAL_EF.Entity.UserSettings", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.ToTable("UserSettings");
                });

            modelBuilder.Entity("DAL_EF.Entity.Wallet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("DAL_EF.Entity.Transaction.CategoryTransaction", b =>
                {
                    b.HasBaseType("DAL_EF.Entity.Transaction.TransactionBase");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.HasIndex("CategoryId");

                    b.HasDiscriminator().HasValue("CategoryTransaction");
                });

            modelBuilder.Entity("DAL_EF.Entity.Transaction.WalletTransaction", b =>
                {
                    b.HasBaseType("DAL_EF.Entity.Transaction.TransactionBase");

                    b.Property<int>("TargetWalletId")
                        .HasColumnType("int");

                    b.HasIndex("TargetWalletId");

                    b.HasDiscriminator().HasValue("WalletTransaction");
                });

            modelBuilder.Entity("DAL_EF.Entity.Category", b =>
                {
                    b.HasOne("DAL_EF.Entity.Wallet", null)
                        .WithMany("Categories")
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL_EF.Entity.Transaction.TransactionBase", b =>
                {
                    b.HasOne("DAL_EF.Entity.Wallet", "SourceWallet")
                        .WithMany()
                        .HasForeignKey("SourceWalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SourceWallet");
                });

            modelBuilder.Entity("DAL_EF.Entity.UserSettings", b =>
                {
                    b.HasOne("DAL_EF.Entity.User", null)
                        .WithOne("Settings")
                        .HasForeignKey("DAL_EF.Entity.UserSettings", "UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL_EF.Entity.Wallet", b =>
                {
                    b.HasOne("DAL_EF.Entity.UserSettings", null)
                        .WithMany("Wallets")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("DAL_EF.Entity.Transaction.CategoryTransaction", b =>
                {
                    b.HasOne("DAL_EF.Entity.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("DAL_EF.Entity.Transaction.WalletTransaction", b =>
                {
                    b.HasOne("DAL_EF.Entity.Wallet", "TargetWallet")
                        .WithMany()
                        .HasForeignKey("TargetWalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TargetWallet");
                });

            modelBuilder.Entity("DAL_EF.Entity.User", b =>
                {
                    b.Navigation("Settings");
                });

            modelBuilder.Entity("DAL_EF.Entity.UserSettings", b =>
                {
                    b.Navigation("Wallets");
                });

            modelBuilder.Entity("DAL_EF.Entity.Wallet", b =>
                {
                    b.Navigation("Categories");
                });
#pragma warning restore 612, 618
        }
    }
}
