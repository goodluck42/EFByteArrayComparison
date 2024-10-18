using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFByteArrayComparison;

internal sealed class AppDbContext : DbContext
{
	private const string DbName = "ByteComparisonDbTest";

	public DbSet<EntityObject> EntityObjects { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseSqlServer(
			$"Server=localhost,1433;Database={DbName};Trusted_Connection=True;TrustServerCertificate=True");
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.Entity<EntityObject>(x =>
		{
			x.Property(a => a.Hash).Metadata
				.SetValueComparer(new ValueComparer<byte[]>(
					(a, b) => ReferenceEquals(a, b),
					a => a.GetHashCode(),
					a => a));

			x.HasData([
				new()
				{
					Id = -4,
					Hash = [128, 127, 126, 125, 124, 123, 122]
				},
				new()
				{
					Id = -3,
					Hash = [1, 2, 3]
				},
				new()
				{
					Id = -2,
					Hash = [10, 13, 20, 30, 40, 42]
				},
				new()
				{
					Id = -1,
					Hash = [42, 13, 69, 255, 128]
				}
			]);

			x.HasKey(e => e.Id);
		});
	}
}