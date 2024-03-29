﻿// <auto-generated />
using System;
using MempApp.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MemoApp.Model.Migrations
{
    [DbContext(typeof(MemoAppContext))]
    partial class MemoAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("MempApp.Model.Config", b =>
                {
                    b.Property<string>("ConfigId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("ConfigId");

                    b.ToTable("Configs");
                });

            modelBuilder.Entity("MempApp.Model.EachTask", b =>
                {
                    b.Property<string>("EachTaskId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("CompleteFlag");

                    b.Property<string>("Content");

                    b.Property<DateTimeOffset>("DeadLine");

                    b.Property<string>("ParentEachTaskId");

                    b.Property<DateTimeOffset>("PlanDate");

                    b.Property<int>("Rank");

                    b.Property<DateTimeOffset>("RegisteredDate");

                    b.Property<bool>("StartedFlag");

                    b.Property<string>("Type");

                    b.Property<bool>("ValidFlag");

                    b.HasKey("EachTaskId");

                    b.ToTable("EachTasks");
                });

            modelBuilder.Entity("MempApp.Model.Memo", b =>
                {
                    b.Property<string>("MemoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<DateTimeOffset>("CreateTime");

                    b.Property<string>("EachTaskId");

                    b.Property<DateTimeOffset>("UpdateTime");

                    b.HasKey("MemoId");

                    b.HasIndex("EachTaskId");

                    b.ToTable("Memos");
                });

            modelBuilder.Entity("MempApp.Model.TimeInfo", b =>
                {
                    b.Property<string>("TimeInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EachTaskId");

                    b.Property<DateTimeOffset>("Start");

                    b.Property<DateTimeOffset>("Stop");

                    b.HasKey("TimeInfoId");

                    b.HasIndex("EachTaskId");

                    b.ToTable("TimeInfos");
                });

            modelBuilder.Entity("MempApp.Model.Memo", b =>
                {
                    b.HasOne("MempApp.Model.EachTask", "EachTask")
                        .WithMany()
                        .HasForeignKey("EachTaskId");
                });

            modelBuilder.Entity("MempApp.Model.TimeInfo", b =>
                {
                    b.HasOne("MempApp.Model.EachTask", "EachTask")
                        .WithMany()
                        .HasForeignKey("EachTaskId");
                });
#pragma warning restore 612, 618
        }
    }
}
