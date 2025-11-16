namespace User.Domain.Entity;
public class Invitation
{
    public int Id { get; set; }

    public Guid Key { get; set; }

    public string Identifier { get; set; } = null!;

    public DateTime ExpireTime { get; set; }

    public bool IsExpired { get; set; }
}