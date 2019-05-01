using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class InternShipMap : EntityTypeConfiguration<InternShip>
    {
        public InternShipMap()
        {
            // Primary Key
            this.HasKey(t => t.InternShipID);

            // Properties
            this.Property(t => t.AdviserID)
                .HasMaxLength(256);

            this.Property(t => t.Time)
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("InternShip");
            this.Property(t => t.InternShipID).HasColumnName("InternShipID");
            this.Property(t => t.StudentID).HasColumnName("StudentID");
            this.Property(t => t.CompanyID).HasColumnName("CompanyID");
            this.Property(t => t.AdviserID).HasColumnName("AdviserID");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.Time).HasColumnName("Time");
            this.Property(t => t.CrtDate).HasColumnName("CrtDate");
            this.Property(t => t.DelDate).HasColumnName("DelDate");

            // Relationships
            this.HasOptional(t => t.City1)
                .WithMany(t => t.InternShips)
                .HasForeignKey(d => d.City);
            this.HasOptional(t => t.Company)
                .WithMany(t => t.InternShips)
                .HasForeignKey(d => d.CompanyID);
            this.HasOptional(t => t.Student)
                .WithMany(t => t.InternShips)
                .HasForeignKey(d => d.StudentID);

        }
    }
}
