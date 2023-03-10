// <auto-generated />
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace API.Data.Migrations
{
    [DbContext(typeof(GrossToNetContext))]
    partial class GrossToNetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("API.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<decimal>("GrossIncome")
                        .HasPrecision(9, 2)
                        .HasColumnType("decimal(9,2)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<string>("WorkPosition")
                        .IsRequired()
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.HasKey("Id")
                        .HasName("PRIMARY");

                    b.HasIndex(new[] { "Id" }, "Id_UNIQUE")
                        .IsUnique();

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("API.Entities.IncomeDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<decimal>("HealthCare")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("NetIncome")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("PIO")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("Tax")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<decimal>("Unemployment")
                        .HasColumnType("decimal(8, 2)");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId")
                        .IsUnique();

                    b.ToTable("IncomeDetails");
                });

            modelBuilder.Entity("API.Entities.IncomeDetails", b =>
                {
                    b.HasOne("API.Entities.Employee", null)
                        .WithOne("IncomeDetails")
                        .HasForeignKey("API.Entities.IncomeDetails", "EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("API.Entities.Employee", b =>
                {
                    b.Navigation("IncomeDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
