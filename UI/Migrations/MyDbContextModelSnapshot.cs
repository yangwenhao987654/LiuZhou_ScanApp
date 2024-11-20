﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScanApp.DAL.DBContext;

#nullable disable

namespace ScanApp.Migrations
{
    [DbContext(typeof(MyDbContext))]
    partial class MyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.11");

            modelBuilder.Entity("ScanApp.DAL.Entity.BarcodeRecordEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Barcode")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("ErrInfo")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT")
                        .HasDefaultValue("");

                    b.Property<int>("ProductCode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Result")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<string>("UseDateStr")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UseTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("tbBarcodeRecord", (string)null);
                });

            modelBuilder.Entity("ScanApp.DAL.Entity.BarcodeRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("tbBarcodeRule", (string)null);
                });

            modelBuilder.Entity("ScanApp.DAL.Entity.BarcodeRuleParameter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("BarcodeRuleId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FixedValue")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Format")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Length")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MatchPattern")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("NeedCheckLength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Sequence")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BarcodeRuleId");

                    b.ToTable("tbRuleParameter", (string)null);
                });

            modelBuilder.Entity("ScanApp.DAL.Entity.ProductFormulaEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AcupointNumber")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("BarcodeLength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("BarcodeType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DateLength")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FixedValue1")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("MatchRule")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PartCode")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductCode")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductName")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("ProductType")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("SerialNum")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("SupplierCode")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("ID");

                    b.ToTable("tbProductFormula", (string)null);
                });

            modelBuilder.Entity("ScanApp.DAL.Entity.BarcodeRuleParameter", b =>
                {
                    b.HasOne("ScanApp.DAL.Entity.BarcodeRule", null)
                        .WithMany("Parameters")
                        .HasForeignKey("BarcodeRuleId");
                });

            modelBuilder.Entity("ScanApp.DAL.Entity.BarcodeRule", b =>
                {
                    b.Navigation("Parameters");
                });
#pragma warning restore 612, 618
        }
    }
}
