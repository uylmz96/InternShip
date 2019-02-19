using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class RefusalReasonMap : EntityTypeConfiguration<RefusalReason>
    {
        public RefusalReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.ReasonID);

            // Properties
            this.Property(t => t.Reason)
                .HasMaxLength(1000);

            this.Property(t => t.Desc)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("RefusalReason");
            this.Property(t => t.ReasonID).HasColumnName("ReasonID");
            this.Property(t => t.Reason).HasColumnName("Reason");
            this.Property(t => t.Desc).HasColumnName("Desc");
            this.Property(t => t.CrtDate).HasColumnName("CrtDate");
            this.Property(t => t.DelDate).HasColumnName("DelDate");
        }
    }
}
