

dotnet ef migrations add Auth_InitialSetup --context ApplicationDbContext
dotnet ef migrations add Bizchat_InitialSetup --context BizchatDbContext

dotnet ef database update --context ApplicationDbContext
dotnet ef database update --context BizchatDbContext