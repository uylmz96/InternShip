using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class CompanyMap : EntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            // Primary Key
            this.HasKey(t => t.CompanyID);

            // Properties
            this.Property(t => t.CompanyName)
                .HasMaxLength(150);

            this.Property(t => t.Address)
                .HasMaxLength(250);

            this.Property(t => t.Phone)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.Mail)
                .HasMaxLength(150);

            this.Property(t => t.Desc)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("Company");
            this.Property(t => t.CompanyID).HasColumnName("CompanyID");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.Desc).HasColumnName("Desc");
            this.Property(t => t.IsBlackCompany).HasColumnName("IsBlackCompany");
            this.Property(t => t.CrtDate).HasColumnName("CrtDate");
            this.Property(t => t.DelDate).HasColumnName("DelDate");
        }
    }
}
