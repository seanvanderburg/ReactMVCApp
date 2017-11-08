using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Model
{
    public class MovieContext : DbContext
    {

        public DbSet<Actor> Actors { get; set; }
        public DbSet<Movie> Movies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.Entity<MovieActor>()
            // .HasKey(t => new { t.ActorId, t.MovieId });
            // modelBuilder.Entity<MovieActor>()
            // .HasOne(ma => ma.Movie)
            // .WithMany(m => m.Actors)
            // .HasForeignKey(ma => ma.MovieId);
            // modelBuilder.Entity<MovieActor>()
            // .HasOne(ma => ma.Actor)
            // .WithMany(m => m.Movies)
            // .HasForeignKey(ma => ma.ActorId);
        }

        //Added constructor to provide the connection to the database as a service (look at: startup.cs)
        public MovieContext(DbContextOptions<MovieContext> options) : base(options)
        {
        }

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {

        //         // http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings
        //         optionsBuilder.UseNpgsql(@"Host=localhost;Database=MovieDB;Username=postgres;Password=postgres");
        //     }
        // }

        //this is the typed representation of a movie in our project


    }
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Release {get;set;}
        public List<Actor> Actors { get; set; }
    }

    //this is the typed representation of an actor in our project
    public class Actor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Gender {get;set;}
        public DateTime Birth {get;set;}
        public int? MovieId { get; set; }
    }
}