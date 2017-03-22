using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Negative_Infinity_Note
{
    // Note Context for SQLite Database
    public class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=notes.db");
        }
    }

    // Basic Db CRUD manager for Notes SQLite database
    static class NoteDBManager
    {
        public static void Create(Note note)
        {
            using (var db = new NoteContext())
            {
                db.Notes.Add(note);
                db.SaveChanges();
            }
        }

        public static ObservableCollection<Note> Read()
        {
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
            using (var db = new NoteContext())
            {
                foreach (Note note in db.Notes.ToList())
                {
                    notes.Add(note);
                }
                return notes;
            }
        }

        public static ObservableCollection<Note> Read(string query)
        {
            ObservableCollection<Note> notes = new ObservableCollection<Note>();
            using (var db = new NoteContext())
            {
                var set = db.Notes.Where(n => n.Title.Contains(query));
                foreach (Note note in set)
                {
                    notes.Add(note);
                }
                return notes;
            }
        }

        public static void Update(Note note)
        {
            using (var db = new NoteContext())
            {
                db.Notes.Update(note);
                db.SaveChanges();
            }
        }

        public static void Delete(Note note)
        {
            using (var db = new NoteContext())
            {
                db.Notes.Remove(note);
                db.SaveChanges();
            }
        }
    }
}
