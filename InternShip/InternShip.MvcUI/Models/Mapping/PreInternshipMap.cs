using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class PreInternshipMap : EntityTypeConfiguration<PreInternship>
    {
        public PreInternshipMap()
        {
            // Primary Key
            this.HasKey(t => t.PreInternshipID);

            // Properties
            this.Property(t => t.CompanyName)
                .HasMaxLength(150);

            this.Property(t => t.CompanyAddress)
                .HasMaxLength(250);

            this.Property(t => t.CompanyPhone)
                .IsFixedLength()
                .HasMaxLength(50);

            this.Property(t => t.CompanyMail)
                .HasMaxLength(150);

            this.Property(t => t.Department)
                .HasMaxLength(100);

            this.Property(t => t.Activity)
                .HasMaxLength(4000);

            this.Property(t => t.Tech)
                .HasMaxLength(4000);

            this.Property(t => t.Subject)
                .HasMaxLength(4000);

            this.Property(t => t.EmployeeDesc)
                .HasMaxLength(4000);

            this.Property(t => t.StudentNumber)
                .HasMaxLength(15);

            this.Property(t => t.Time)
                .IsFixedLength()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("PreInternship");
            this.Property(t => t.PreInternshipID).HasColumnName("PreInternshipID");
            this.Property(t => t.StudentID).HasColumnName("StudentID");
            this.Property(t => t.InternshipID).HasColumnName("InternshipID");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
            this.Property(t => t.CompanyAddress).HasColumnName("CompanyAddress");
            this.Property(t => t.CompanyPhone).HasColumnName("CompanyPhone");
            this.Property(t => t.CompanyMail).HasColumnName("CompanyMail");
            this.Property(t => t.Department).HasColumnName("Department");
            this.Property(t => t.Activity).HasColumnName("Activity");
            this.Property(t => t.Tech).HasColumnName("Tech");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.EmployeeDesc).HasColumnName("EmployeeDesc");
            this.Property(t => t.StartDate).HasColumnName("StartDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.CrtDate).HasColumnName("CrtDate");
            this.Property(t => t.DelDate).HasColumnName("DelDate");
            this.Property(t => t.StudentNumber).HasColumnName("StudentNumber");
            this.Property(t => t.Time).HasColumnName("Time");

            // Relationships
            this.HasOptional(t => t.InternShip)
                .WithMany(t => t.PreInternships)
                .HasForeignKey(d => d.InternshipID);
            this.HasOptional(t => t.Student)
                .WithMany(t => t.PreInternships)
                .HasForeignKey(d => d.StudentID);

        }
    }
}
