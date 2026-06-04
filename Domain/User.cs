using System;
using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Identity;

namespace Domain;

public class User : IdentityUser
{
    private static readonly string[] Adjectives = ["Happy", "Funky", "Dancing", "Cosmic", "Sneaky", "Chilled", "Bouncing"];
    private static readonly string[] Nouns = ["Panda", "Potato", "Gamer", "Astronaut", "Ninja", "Capybara", "Wombat"];

    public string DisplayName { get; set; } = GenerateRandomTag();

    public string? Bio { get; set; }
    public string? ImageUrl { get; set; }
    public ICollection<ActivityAttendee> Activities { get; set; } = [];
    public ICollection<Image> Images { get; set; } = [];

    private static string GenerateRandomTag()
    {
        var random = new Random();
        string adj = Adjectives[random.Next(Adjectives.Length)];
        string noun = Nouns[random.Next(Nouns.Length)];
        int number = random.Next(1000, 9999);

        return $"{adj}{noun}{number}";
    }
}