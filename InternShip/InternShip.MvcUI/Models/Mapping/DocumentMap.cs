using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class DocumentMap : EntityTypeConfiguration<Document>
    {
        public DocumentMap()
        {
            // Primary Key
            this.HasKey(t => t.DocumentID);

            // Properties
            this.Property(t => t.DocName)
                .HasMaxLength(100);

            this.Property(t => t.Path)
                .HasMaxLength(1000);

            this.Property(t => t.Desc)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Document");
            this.Property(t => t.DocumentID).HasColumnName("DocumentID");
            this.Property(t => t.InternShipID).HasColumnName("InternShipID");
            this.Property(t => t.DocName).HasColumnName("DocName");
            this.Property(t => t.Path).HasColumnName("Path");
            this.Property(t => t.Desc).HasColumnName("Desc");
            this.Property(t => t.CrtDate).HasColumnName("CrtDate");
            this.Property(t => t.DelDate).HasColumnName("DelDate");

            // Relationships
            this.HasOptional(t => t.InternShip)
                .WithMany(t => t.Documents)
                .HasForeignKey(d => d.InternShipID);

        }
    }
}
