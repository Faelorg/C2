using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Server.Repos;

public partial class ServerContext : DbContext
{
    public ServerContext() { }

    public ServerContext(DbContextOptions<ServerContext> options)
        : base(options) { }

    public virtual DbSet<Diploma> Diplomas { get; set; }

    public virtual DbSet<Lesson> Lessons { get; set; }

    public virtual DbSet<LessonTask> LessonTasks { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SupplementLesson> SupplementLessons { get; set; }

    public virtual DbSet<SupplementTask> SupplementTasks { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<Topic> Topics { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserTask> UserTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Diploma>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("diploma");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnType("datetime").HasColumnName("date");
            entity.Property(e => e.Estimation).HasColumnName("estimation");
            entity.Property(e => e.TopicId).HasMaxLength(45).HasColumnName("topic_id");
        });

        modelBuilder.Entity<Lesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("lesson");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(45).HasColumnName("id");
            entity.Property(e => e.Autor).HasMaxLength(45).HasColumnName("autor");
            entity.Property(e => e.Content).HasColumnType("text").HasColumnName("content");
            entity.Property(e => e.Tittle).HasColumnType("text").HasColumnName("tittle");
            entity.Property(e => e.TopicId).HasMaxLength(45).HasColumnName("topic_id");
        });

        modelBuilder.Entity<LessonTask>(entity =>
        {
            entity.HasKey(e => e.IdlessonId).HasName("PRIMARY");

            entity.ToTable("lesson-task");

            entity.Property(e => e.IdlessonId).HasColumnName("idlesson-id");
            entity.Property(e => e.LessonId).HasMaxLength(45).HasColumnName("lesson_id");
            entity.Property(e => e.TaskId).HasMaxLength(45).HasColumnName("task_id");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("result");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Estimation).HasColumnName("estimation");
            entity
                .Property(e => e.Recommendations)
                .HasMaxLength(45)
                .HasColumnName("recommendations");
            entity.Property(e => e.TopicId).HasMaxLength(45).HasColumnName("topic_id");
            entity.Property(e => e.UserId).HasMaxLength(45).HasColumnName("user_id");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("role");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name).HasMaxLength(255).HasColumnName("name");
        });

        modelBuilder.Entity<SupplementLesson>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("supplement-lesson");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(45).HasColumnName("id");
            entity.Property(e => e.File).HasColumnType("text").HasColumnName("file");
            entity.Property(e => e.LessonId).HasMaxLength(45).HasColumnName("lesson_id");
            entity.Property(e => e.Tittle).HasColumnType("text").HasColumnName("tittle");
        });

        modelBuilder.Entity<SupplementTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("supplement-task");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(45).HasColumnName("id");
            entity.Property(e => e.File).HasColumnType("text").HasColumnName("file");
            entity.Property(e => e.TaskId).HasMaxLength(45).HasColumnName("task_id");
            entity.Property(e => e.Tittle).HasColumnType("text").HasColumnName("tittle");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("task");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(45).HasColumnName("id");
            entity.Property(e => e.Content).HasColumnType("text").HasColumnName("content");
            entity.Property(e => e.Difficalty).HasColumnName("difficalty");
            entity.Property(e => e.Time).HasColumnName("time");
            entity.Property(e => e.Tittle).HasColumnType("text").HasColumnName("tittle");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("topic");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(45).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
            entity.Property(e => e.Tittle).HasColumnType("text").HasColumnName("tittle");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.HasIndex(e => e.Token, "token_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasMaxLength(45).HasColumnName("id");
            entity.Property(e => e.Email).HasMaxLength(255).HasColumnName("email");
            entity.Property(e => e.FullName).HasMaxLength(255).HasColumnName("full_name");
            entity.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");
            entity.Property(e => e.RoleId).HasColumnName("role_id");
            entity.Property(e => e.Token).HasMaxLength(45).HasColumnName("token");
            entity.Property(e => e.TopicId).HasMaxLength(45).HasColumnName("topic_id");
        });

        modelBuilder.Entity<UserTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user-task");

            entity.HasIndex(e => e.Id, "id_UNIQUE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnType("text").HasColumnName("description");
            entity.Property(e => e.Estimation).HasMaxLength(45).HasColumnName("estimation");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TaskId).HasMaxLength(45).HasColumnName("task_id");
            entity.Property(e => e.UserId).HasMaxLength(45).HasColumnName("user_id");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
