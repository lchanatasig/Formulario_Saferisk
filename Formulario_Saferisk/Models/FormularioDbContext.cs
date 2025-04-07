using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Formulario_Saferisk.Models;

public partial class FormularioDbContext : DbContext
{
    public FormularioDbContext()
    {
    }

    public FormularioDbContext(DbContextOptions<FormularioDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Broker> Brokers { get; set; }

    public virtual DbSet<Dato> Datos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=FormularioDB;User ID=sa;Password=Sur2o22--;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Broker>(entity =>
        {
            entity.HasKey(e => e.BrokerId).HasName("PK__brokers__FB846654468069A7");

            entity.ToTable("brokers");

            entity.Property(e => e.BrokerId).HasColumnName("broker_id");
            entity.Property(e => e.Categoria)
                .HasMaxLength(255)
                .HasColumnName("categoria");
            entity.Property(e => e.CodAuto)
                .HasMaxLength(255)
                .HasColumnName("cod_auto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(255)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Dato>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__datos__3213E83F42384E98");

            entity.ToTable("datos");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Broker)
                .HasMaxLength(100)
                .HasColumnName("broker");
            entity.Property(e => e.Ciudad)
                .HasMaxLength(150)
                .HasColumnName("ciudad");
            entity.Property(e => e.Correo)
                .HasMaxLength(150)
                .HasColumnName("correo");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecha_registro");
            entity.Property(e => e.NombresCompletos)
                .HasMaxLength(255)
                .HasColumnName("nombres_completos");
            entity.Property(e => e.Perfil)
                .HasMaxLength(100)
                .HasColumnName("perfil");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
