using System;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using TicketsApp.Components;

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

    public virtual DbSet<Proveedor> Proveedor { get; set; }

    public virtual DbSet<Notificacion> Notificaciones { get; set; }

    public virtual DbSet<ComentarioTicket> Comentarios { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<Plantilla> Plantillas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.HasData(
                new Categoria()
                {
                    Id = 1,
                    Nombre = "Software"
                },
                new Categoria()
                {
                    Id = 2,
                    Nombre = "Hardware"
                },
                new Categoria()
                {
                    Id = 3,
                    Nombre = "Sistemas ERP"
                },
                new Categoria()
                {
                    Id = 4,
                    Nombre = "Red e Internet"
                },
                new Categoria()
                {
                    Id = 5,
                    Nombre = "Impresoras"
                }
            );
        });

        modelBuilder.Entity<Estatus>(entity =>
        {
            entity.ToTable("Estatus");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
            entity.HasData(
                new Estatus
                {
                    Id = 1,
                    Nombre = "Abierto"
                },
                new Estatus
                {
                    Id = 2,
                    Nombre = "En Proceso"
                },
                new Estatus
                {
                    Id = 3,
                    Nombre = "Cerrado Parcial"
                },
                new Estatus
                {
                    Id = 4,
                    Nombre = "Cerrado"
                }
            );
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Anydesk)
                .HasMaxLength(50)
                .HasColumnName("anydesk");
            entity.Property(e => e.Asunto).HasColumnName("asunto");
            entity.Property(e => e.CerradoPorUsuario).HasColumnName("cerrado_por_usuario");
            entity.Property(e => e.RegistradoPorAnalista).HasColumnName("registrado_por_analista");
            entity.Property(e => e.RegistroPropio_Razon).HasColumnName("registro_propio_razon");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.Duracion)
                .HasColumnName("duracion")
                .HasComputedColumnSql("DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)");
            entity.Property(e => e.TiempoAbierto)
                .HasColumnName("TiempoAbierto")
                .HasComputedColumnSql("DATEDIFF(MINUTE, fecha_inicio, FechaTomado)");
            entity.Property(e => e.TiempoHastaCierreParcial)
                .HasColumnName("TiempoHastaCierreParcial")
                .HasComputedColumnSql("DATEDIFF(MINUTE, FechaTomado, FechaCierreParcial)");
            entity.Property(e => e.TiempoHastaCierre)
                .HasColumnName("TiempoHastaCierre")
                .HasComputedColumnSql("DATEDIFF(MINUTE, FechaCierreParcial, fecha_cierre   )");
            entity.Property(e => e.Duracion_Abierto_CierreParcial)
                .HasColumnName("Duracion_Abierto_CierreParcial")
                .HasComputedColumnSql("DATEDIFF(MINUTE, fecha_inicio, FechaCierreParcial)");
            entity.Property(e => e.Duracion_Abierto_Cierre)
                .HasColumnName("Duracion_Abierto_Cierre")
                .HasComputedColumnSql("DATEDIFF(MINUTE, fecha_inicio, fecha_cierre)");
            entity.Property(e => e.FechaCierre).HasColumnName("fecha_cierre").HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnName("fecha_inicio").HasColumnType("datetime");
            entity.Property(e => e.FechaTomado).HasColumnName("FechaTomado").HasColumnType("datetime");
            entity.Property(e => e.FechaCierreParcial).HasColumnName("FechaCierreParcial").HasColumnType("datetime");
            entity.Property(e => e.FkAnalista).HasColumnName("fk_analista");
            entity.Property(e => e.FkCategoria).HasColumnName("fk_categoria");
            entity.Property(e => e.FkEstatus).HasColumnName("fk_estatus");
            entity.Property(e => e.FkUnidad).HasColumnName("fk_unidad");
            entity.Property(e => e.FkUsuario).HasColumnName("fk_usuario");
            entity.Property(e => e.Solucion).HasColumnName("solucion");
            entity.Property(e => e.AsignadoAProvedor).HasColumnName("AsignadoAProvedor");
            entity.Property(e => e.FkProvedor).HasColumnName("Provedor");

            entity.HasOne(d => d.FkProveedorNavigation).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.FkProvedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tickets_Proveedores");

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
            entity.Property(e => e.Activo)
                .HasColumnName("activo");
            entity.HasData(
                new Unidad()
                {
                    Id = 1,
                    Nombre = "Cyrburgos"
                }
            );
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ContraseñaVisible).HasColumnName("contraseña_visible");
            entity.Property(e => e.FkUnidad).HasColumnName("fk_unidad");
            entity.Property(e => e.Nombre).HasColumnName("nombre");

            entity.HasOne(d => d.FkUnidadNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.FkUnidad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Unidades");

            entity.HasOne(d => d.FkDepartamentoNavigation).WithMany(d => d.UsuariosDepartamentoNavigations)
            .HasForeignKey(d => d.FkDepartamento);
        });

        modelBuilder.Entity<Plantilla>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Asunto).HasColumnName("asunto");
            entity.Property(e => e.Descripcion).HasColumnName("descripcion");
            entity.Property(e => e.FkCategoria).HasColumnName("fk_categoria");
            entity.Property(e => e.Solucion).HasColumnName("solucion");
            entity.Property(e => e.RegistroPropio_Razon).HasColumnName("registro_propio_razon");
            entity.Property(e => e.AsignadoAProvedor).HasColumnName("asignado_a_provedor");
            entity.Property(e => e.FkProvedor).HasColumnName("fk_provedor");
            entity.Property(e => e.FkUsuario).HasColumnName("fk_usuario");

            entity.HasOne(d => d.FkCategoriaNavigation).WithMany(p => p.Plantillas)
                .HasForeignKey(d => d.FkCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Plantillas_Categorias");

            entity.HasOne(d => d.FkProveedorNavigation).WithMany(p => p.Plantillas)
                .HasForeignKey(d => d.FkProvedor)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Plantillas_Provedores");

            entity.HasOne(d => d.FkUsuarioNavigation).WithMany(p => p.Plantillas)
                .HasForeignKey(d => d.FkUsuario)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Plantillas_Usuarios");
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.FkCategoria).HasColumnName("fk_categoria");
            entity.Property(e => e.Activo).HasColumnName("activo");

            entity.HasOne(d => d.FkCategoriaNavigation).WithMany(d => d.Proveedores).HasForeignKey(d => d.FkCategoria);
        });

        modelBuilder.Entity<Notificacion>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FkAnalista).HasColumnName("fk_analista");
            entity.Property(e => e.FkUsuario).HasColumnName("fk_usuario");
            entity.Property(e => e.FkTicket).HasColumnName("fk_ticket");
            entity.Property(e => e.Mensaje).HasColumnName("mensaje");
            entity.Property(e => e.Leida).HasColumnName("leida");
            entity.Property(e => e.Fecha).HasColumnName("fecha");

            entity.HasOne(d => d.FkUsuarioNavigation).WithMany(p => p.NotificacionFkUsuarioNavigations)
                .HasForeignKey(d => d.FkUsuario)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Usuarios");

            entity.HasOne(d => d.FkAnalistaNavigation).WithMany(p => p.NotificacionFkAnalistaNavigations)
                .HasForeignKey(d => d.FkAnalista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Analistas");


            entity.HasOne(d => d.FkTicketNavigation).WithMany(p => p.NotificacionesTickets)
                .HasForeignKey(d => d.FkTicket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Notificaciones_Tickets");
        });

        modelBuilder.Entity<ComentarioTicket>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Contenido).HasColumnName("Contenido");
            entity.Property(e => e.FkTicket).HasColumnName("fk_ticket");
            entity.Property(e => e.FkUsuario).HasColumnName("fk_usuario");

            entity.HasOne(d => d.FkUsuarioNavigation).WithMany(d => d.ComentarioTicketFkUsuarioNavigations).HasForeignKey(d => d.FkUsuario).OnDelete(DeleteBehavior.ClientCascade);

            entity.HasOne(d => d.FkTicketNavigation).WithMany(d => d.ComentariosTickets).HasForeignKey(d => d.FkTicket).OnDelete(DeleteBehavior.ClientNoAction);
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nombre).HasColumnName("nombre");
            entity.Property(e => e.Activo).HasColumnName("activo");

            entity.HasData(
                new Departamento()
                {
                    Id = 1,
                    Nombre = "Sistemas",
                    Activo = true
                }
            );
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
