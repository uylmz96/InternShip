using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CityName)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("City");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.CityName).HasColumnName("CityName");
        }
    }
}
