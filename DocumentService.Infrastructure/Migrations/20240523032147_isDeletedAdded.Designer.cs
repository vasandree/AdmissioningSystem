﻿// <auto-generated />
using System;
using DocumentService.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DocumentService.Infrastructure.Migrations
{
    [DbContext(typeof(DocumentsDbContext))]
    [Migration("20240523032147_isDeletedAdded")]
    partial class isDeletedAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DocumentService.Domain.Entities.DbFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("FileContent")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("DocumentService.Domain.Entities.EducationDocument", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("DocumentType")
                        .HasColumnType("integer");

                    b.Property<Guid?>("EducationDocumentTypeId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("EducationDocuments");
                });

            modelBuilder.Entity("DocumentService.Domain.Entities.Passport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DocumentType")
                        .HasColumnType("integer");

                    b.Property<Guid?>("FileId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("IssueDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("IssuedBy")
                        .HasColumnType("text");

                    b.Property<string>("SeriesAndNumber")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("FileId");

                    b.ToTable("Passports");
                });

            modelBuilder.Entity("DocumentService.Domain.Entities.EducationDocument", b =>
                {
                    b.HasOne("DocumentService.Domain.Entities.DbFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.Navigation("File");
                });

            modelBuilder.Entity("DocumentService.Domain.Entities.Passport", b =>
                {
                    b.HasOne("DocumentService.Domain.Entities.DbFile", "File")
                        .WithMany()
                        .HasForeignKey("FileId");

                    b.Navigation("File");
                });
#pragma warning restore 612, 618
        }
    }
}
