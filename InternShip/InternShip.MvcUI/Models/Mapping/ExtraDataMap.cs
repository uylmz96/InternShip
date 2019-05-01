using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class ExtraDataMap : EntityTypeConfiguration<ExtraData>
    {
        public ExtraDataMap()
        {
            // Primary Key
            this.HasKey(t => t.DataType);

            // Properties
            this.Property(t => t.DataType)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Data)
                .HasMaxLength(4000);

            // Table & Column Mappings
            this.ToTable("ExtraDatas");
            this.Property(t => t.DataType).HasColumnName("DataType");
            this.Property(t => t.Data).HasColumnName("Data");
        }
    }
}
