using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;


namespace InternShip.MvcUI.Models.Mapping
{
    public class ExtraDataMap : EntityTypeConfiguration<ExtraData>
    {
        public ExtraDataMap()
        {
            this.HasKey(t => t.DataType);
            this.ToTable("ExtraData");
            this.Property(t => t.DataType).HasColumnName("DataType");
            this.Property(t => t.Data).HasColumnName("Data");
        }
    }
}