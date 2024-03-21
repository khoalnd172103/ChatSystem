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

    public static async Task SeedPhotos(DataContext context)
    {
        if (await context.Photos.AnyAsync()) return;

        var photoData = await File.ReadAllTextAsync("PhotoSeedData.json");

        var photos = JsonSerializer.Deserialize<List<Photo>>(photoData);

        
        foreach (var photo in photos)
        {
            context.Photos.Add(photo);
        }

        await context.SaveChangesAsync();
    }
}
