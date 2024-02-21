using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

namespace DataAccessLayer.Data;

public class Seed
{
    public static async Task SeedUsers(DataContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("UserSeedData.json");

        var users = JsonSerializer.Deserialize<List<User>>(userData);

        foreach (var user in users)
        {
            user.UserName = user.UserName.ToLower();
            user.UserPassword = "Pa$$w0rd";

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
