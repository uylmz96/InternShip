using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace InternShip.MvcUI.Models.Mapping
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            // Primary Key
            this.HasKey(t => t.StudentID);

            // Properties
            this.Property(t => t.StudentNumber)
                .HasMaxLength(15);

            this.Property(t => t.Name)
                .HasMaxLength(150);

            this.Property(t => t.Surname)
                .HasMaxLength(150);

            this.Property(t => t.Mail)
                .HasMaxLength(150);

            this.Property(t => t.Phone)
                .IsFixedLength()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Student");
            this.Property(t => t.StudentID).HasColumnName("StudentID");
            this.Property(t => t.StudentNumber).HasColumnName("StudentNumber");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Surname).HasColumnName("Surname");
            this.Property(t => t.Mail).HasColumnName("Mail");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.EntryDate).HasColumnName("EntryDate");
            this.Property(t => t.isGraduate).HasColumnName("isGraduate");
            this.Property(t => t.GraduateDate).HasColumnName("GraduateDate");
            this.Property(t => t.CrtDate).HasColumnName("CrtDate");
            this.Property(t => t.DelDate).HasColumnName("DelDate");
        }
    }
}
