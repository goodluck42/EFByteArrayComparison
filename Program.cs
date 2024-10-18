using EFByteArrayComparison;

using var dbContext = new AppDbContext();

dbContext.Database.EnsureCreated();

// [128, 127, 126, 125, 124, 123, 122] (Id: -4)
// [1, 2, 3] (Id: -3)
// [10, 13, 20, 30, 40, 42] (Id: -2)
// [42, 13, 69, 255, 128] (Id: -1)

byte[] sample = [42, 13, 69, 255, 128];
var res = dbContext.EntityObjects.FirstOrDefault(x => x.Hash == sample);

if (res is null)
{
	Console.WriteLine("Entity not found");

	return;
}

Console.WriteLine(res.Id);