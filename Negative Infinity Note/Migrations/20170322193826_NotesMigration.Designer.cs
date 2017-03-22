using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Negative_Infinity_Note;

namespace Negative_Infinity_Note.Migrations
{
    [DbContext(typeof(NoteContext))]
    [Migration("20170322193826_NotesMigration")]
    partial class NotesMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Negative_Infinity_Note.Note", b =>
                {
                    b.Property<int>("NoteID")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("Data");

                    b.Property<string>("Title");

                    b.HasKey("NoteID");

                    b.ToTable("Notes");
                });
        }
    }
}
