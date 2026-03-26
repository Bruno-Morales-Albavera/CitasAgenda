using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DL;

public partial class CitasAgendaContext : DbContext
{
    public CitasAgendaContext()
    {
    }

    public CitasAgendaContext(DbContextOptions<CitasAgendaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Citum> Cita { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=BRUNO-PC\\SQLEXPRESS; Database=CitasAgenda; TrustServerCertificate=True; Trusted_Connection=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Citum>(entity =>
        {
            entity.HasKey(e => e.IdCita).HasName("PK__Cita__394B02021B9D9394");

            entity.Property(e => e.Estatus)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.NombreCliente)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
