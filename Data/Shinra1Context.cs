using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TriagemCaminhaoAPI.Data;

namespace TriagemCaminhao.Data;

public partial class Shinra1Context : DbContext
{
    public Shinra1Context()
    {
    }

    public Shinra1Context(DbContextOptions<Shinra1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Caminhoes> Caminhoes { get; set; }

    public virtual DbSet<Doca> Docas { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<StatusTriagem> StatusTriagems { get; set; }

    public virtual DbSet<Triagem> Triagems { get; set; }
    public virtual DbSet<PrioridadeTriagem> PrioridadeTriagems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Caminhoes>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Caminhoe__3214EC27BEA84D1F");

            entity.ToTable("Caminhoes", "TriagemCaminhao");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NomeTransportadora).HasMaxLength(100);
            entity.Property(e => e.Whatsapp);
            entity.Property(e => e.Mensagem);
        });

        modelBuilder.Entity<Doca>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Docas__3214EC270EA25515");

            entity.ToTable("Docas", "TriagemCaminhao");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.NomeDoca).HasMaxLength(100);
            entity.Property(e => e.StatusDoca).HasMaxLength(50);
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC27D773A27D");

            entity.ToTable("Logs", "TriagemCaminhao");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Acao).HasMaxLength(200);
            entity.Property(e => e.CaminhaoId).HasColumnName("CaminhaoID");
            entity.Property(e => e.DataHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Usuario).HasMaxLength(100);

            entity.HasOne(d => d.Caminhao).WithMany(p => p.Logs)
                .HasForeignKey(d => d.CaminhaoId)
                .HasConstraintName("FK__Logs__CaminhaoID__3CF40B7E");
        });

        modelBuilder.Entity<StatusTriagem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StatusCa__3214EC27A8958E0F");

            entity.ToTable("StatusTriagem", "TriagemCaminhao");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<PrioridadeTriagem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_PrioridadeTriagem");

            entity.ToTable("PrioridadeTriagem", "TriagemCaminhao");

            entity.Property(e => e.Id).HasColumnName("ID");

            entity.Property(e => e.IsPrioridade)
                .HasDefaultValue(false);

            entity.Property(e => e.Volume)
                .HasMaxLength(255);

            entity.Property(e => e.Peso)
                .HasMaxLength(255);
        });

        modelBuilder.Entity<Triagem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Triagem__3214EC27CB98CB60");

            entity.ToTable("Triagem", "TriagemCaminhao");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CaminhaoId).HasColumnName("CaminhaoID");
            entity.Property(e => e.DataAtendimento).HasColumnType("datetime");
            entity.Property(e => e.DataChegada).HasColumnType("datetime");
            entity.Property(e => e.DocaId).HasColumnName("DocaID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.PrioridadeID).HasColumnName("PrioridadeID");

            entity.HasOne(d => d.Caminhao).WithMany(p => p.Triagems)
                .HasForeignKey(d => d.CaminhaoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Triagem__Caminha__373B3228");

            entity.HasOne(d => d.Doca).WithMany(p => p.Triagems)
                .HasForeignKey(d => d.DocaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Triagem__DocaID__382F5661");

            entity.HasOne(d => d.StatusTriagem).WithOne(p => p.Triagem)
                .HasForeignKey<Triagem>(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Triagem__StatusI__39237A9A");

            entity.HasOne(d => d.PrioridadeTriagem).WithOne(p => p.Triagem)
                .HasForeignKey<Triagem>(d => d.PrioridadeID)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Triagem_PrioridadeTriagem");

        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
