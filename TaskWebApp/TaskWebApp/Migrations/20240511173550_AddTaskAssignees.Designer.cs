﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TaskWebApp.DbStuff;

#nullable disable

namespace TaskWebApp.Migrations
{
    [DbContext(typeof(WebDbContext))]
    [Migration("20240511173550_AddTaskAssignees")]
    partial class AddTaskAssignees
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.18")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TaskInfoUser", b =>
                {
                    b.Property<int>("AssignedTasksId")
                        .HasColumnType("int");

                    b.Property<int>("AssigneesId")
                        .HasColumnType("int");

                    b.HasKey("AssignedTasksId", "AssigneesId");

                    b.HasIndex("AssigneesId");

                    b.ToTable("TaskInfoUser");
                });

            modelBuilder.Entity("TaskWebApp.DbStuff.Models.TaskInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OwnerId")
                        .HasColumnType("int");

                    b.Property<int>("Priority")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("TaskInfos");
                });

            modelBuilder.Entity("TaskWebApp.DbStuff.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PreferLocale")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("TaskInfoUser", b =>
                {
                    b.HasOne("TaskWebApp.DbStuff.Models.TaskInfo", null)
                        .WithMany()
                        .HasForeignKey("AssignedTasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TaskWebApp.DbStuff.Models.User", null)
                        .WithMany()
                        .HasForeignKey("AssigneesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskWebApp.DbStuff.Models.TaskInfo", b =>
                {
                    b.HasOne("TaskWebApp.DbStuff.Models.User", "Owner")
                        .WithMany("OwnedTasks")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("TaskWebApp.DbStuff.Models.User", b =>
                {
                    b.Navigation("OwnedTasks");
                });
#pragma warning restore 612, 618
        }
    }
}
