﻿// <auto-generated />
using LearnVocabulary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearnVocabulary.Migrations
{
    [DbContext(typeof(LearnContext))]
    [Migration("20240214030118_mig1")]
    partial class mig1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LearnVocabulary.Models.DefinitionInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Antonyms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Example")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MeaningId")
                        .HasColumnType("int");

                    b.Property<string>("Synonyms")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MeaningId");

                    b.ToTable("Definitions");
                });

            modelBuilder.Entity("LearnVocabulary.Models.LicanseInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LicanseInfos");
                });

            modelBuilder.Entity("LearnVocabulary.Models.MeaningInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("PartOfSpeech")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("Meanings");
                });

            modelBuilder.Entity("LearnVocabulary.Models.PhoneticInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Audio")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WordId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WordId");

                    b.ToTable("Phonetics");
                });

            modelBuilder.Entity("LearnVocabulary.Models.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("LicenseId")
                        .HasColumnType("int");

                    b.Property<string>("Phonetic")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceUrls")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WordText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("LicenseId");

                    b.ToTable("Words");
                });

            modelBuilder.Entity("LearnVocabulary.Models.DefinitionInfo", b =>
                {
                    b.HasOne("LearnVocabulary.Models.MeaningInfo", "Meaning")
                        .WithMany("Definitions")
                        .HasForeignKey("MeaningId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meaning");
                });

            modelBuilder.Entity("LearnVocabulary.Models.MeaningInfo", b =>
                {
                    b.HasOne("LearnVocabulary.Models.Word", "Word")
                        .WithMany("Meanings")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("LearnVocabulary.Models.PhoneticInfo", b =>
                {
                    b.HasOne("LearnVocabulary.Models.Word", "Word")
                        .WithMany("Phonetics")
                        .HasForeignKey("WordId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Word");
                });

            modelBuilder.Entity("LearnVocabulary.Models.Word", b =>
                {
                    b.HasOne("LearnVocabulary.Models.LicanseInfo", "License")
                        .WithMany()
                        .HasForeignKey("LicenseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("License");
                });

            modelBuilder.Entity("LearnVocabulary.Models.MeaningInfo", b =>
                {
                    b.Navigation("Definitions");
                });

            modelBuilder.Entity("LearnVocabulary.Models.Word", b =>
                {
                    b.Navigation("Meanings");

                    b.Navigation("Phonetics");
                });
#pragma warning restore 612, 618
        }
    }
}
