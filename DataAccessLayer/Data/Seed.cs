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
            user.UserPassword = "$2a$10$I9RoqF2B3XKTlhdkuFti4.SPg9BT4LQqEgKyiPkbCi.pM4yoa1bqm";

            context.Users.Add(user);
        }

        await context.SaveChangesAsync();
    }
}
