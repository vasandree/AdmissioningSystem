﻿// <auto-generated />
using System;
using DictionaryService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DictionaryService.Infrastructure.Migrations
{
    [DbContext(typeof(DictionaryDbContext))]
    [Migration("20240506183056_addedIsDealeted")]
    partial class addedIsDealeted
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DictionaryService.Domain.Entities.DocumentType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("EducationLevelId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.ToTable("DocumentTypes");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.EducationLevel", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("DocumentTypeId")
                        .HasColumnType("uuid");

                    b.Property<int>("ExternalId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("DocumentTypeId");

                    b.ToTable("EducationLevels");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.Faculty", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Faculties");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.Program", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EducationForm")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("EducationLevelId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ExternalId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("FacultyId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("EducationLevelId");

                    b.HasIndex("FacultyId");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.DocumentType", b =>
                {
                    b.HasOne("DictionaryService.Domain.Entities.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationLevel");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.EducationLevel", b =>
                {
                    b.HasOne("DictionaryService.Domain.Entities.DocumentType", null)
                        .WithMany("NextEducationLevels")
                        .HasForeignKey("DocumentTypeId");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.Program", b =>
                {
                    b.HasOne("DictionaryService.Domain.Entities.EducationLevel", "EducationLevel")
                        .WithMany()
                        .HasForeignKey("EducationLevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DictionaryService.Domain.Entities.Faculty", "Faculty")
                        .WithMany()
                        .HasForeignKey("FacultyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EducationLevel");

                    b.Navigation("Faculty");
                });

            modelBuilder.Entity("DictionaryService.Domain.Entities.DocumentType", b =>
                {
                    b.Navigation("NextEducationLevels");
                });
#pragma warning restore 612, 618
        }
    }
}
