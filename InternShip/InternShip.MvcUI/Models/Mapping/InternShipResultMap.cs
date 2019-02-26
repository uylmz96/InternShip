using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class InternShipResultMap : EntityTypeConfiguration<InternShipResult>
    {
        public InternShipResultMap()
        {
            // Primary Key
            this.HasKey(t => t.ResultID);

            // Properties
            this.Property(t => t.Desc)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("InternShipResult");
            this.Property(t => t.ResultID).HasColumnName("ResultID");
            this.Property(t => t.InternShipID).HasColumnName("InternShipID");
            this.Property(t => t.RefusalReasonID).HasColumnName("RefusalReasonID");
            this.Property(t => t.AcceptedTime).HasColumnName("AcceptedTime");
            this.Property(t => t.Desc).HasColumnName("Desc");

            // Relationships
            this.HasOptional(t => t.InternShip)
                .WithMany(t => t.InternShipResults)
                .HasForeignKey(d => d.InternShipID);
            this.HasOptional(t => t.RefusalReason)
                .WithMany(t => t.InternShipResults)
                .HasForeignKey(d => d.RefusalReasonID);

        }
    }
}
