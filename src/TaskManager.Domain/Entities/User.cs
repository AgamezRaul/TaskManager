using TaskManager.Domain.Common;

namespace TaskManager.Domain.Entities;

public class User : BaseEntity
{
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;
    public ICollection<TaskItem> Tasks { get; private set; } = [];

    private User() { }

    public static User Create(string email, string passwordHash)
    {
        return new User
        {
            Email = email.ToLowerInvariant().Trim(),
            PasswordHash = passwordHash
        };
    }

    public void UpdatePasswordHash(string newHash) => PasswordHash = newHash;
}
