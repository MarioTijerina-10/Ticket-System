using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace TicketsApp.Models;

public partial class TicketsDbContext : IdentityDbContext<Usuario, IdentityRole, string>
{
    public TicketsDbContext()
    {
    }

    public TicketsDbContext(DbContextOptions<TicketsDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Estatus> Estatuses { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Unidad> Unidades { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-NQRK101\\MSSQLSERVER01;Database=TicketsDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.ToTable("Estatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anydesk)
                .HasMaxLength(50)
                .HasColumnName("anydesk");
            entity.Property(e => e.Asunto).HasColumnName("asunto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Duracion)
                .HasColumnType("datetime")
                .HasColumnName("duracion");
            entity.Property(e => e.FechaCierre).HasColumnName("fecha_cierre");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio");
            entity.Property(e => e.FkAnalista).HasColumnName("fk_analista");
            entity.Property(e => e.FkCategoria).HasColumnName("fk_categoria");
            entity.Property(e => e.FkEstatus).HasColumnName("fk_estatus");
            entity.Property(e => e.FkUnidad).HasColumnName("fk_unidad");
            entity.Property(e => e.FkUsuario).HasColumnName("fk_usuario");
            entity.Property(e => e.Solucion).HasColumnName("solucion");

            entity.HasOne(d => d.FkAnalistaNavigation).WithMany(p => p.TicketFkAnalistaNavigations)
                .HasForeignKey(d => d.FkAnalista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Analistas");

            entity.HasOne(d => d.FkCategoriaNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FkCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Categorias");

            entity.HasOne(d => d.FkEstatusNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FkEstatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Estatus");

            entity.HasOne(d => d.FkUnidadNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FkUnidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Unidades");

            entity.HasOne(d => d.FkUsuarioNavigation).WithMany(p => p.TicketFkUsuarioNavigations)
                .HasForeignKey(d => d.FkUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Usuarios");
        });

        modelBuilder.Entity<Unidad>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkUnidad).HasColumnName("fk_unidad");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasOne(d => d.FkUnidadNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FkUnidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Unidades");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
