

using ShoeStoreManagement.Core.Enums;

namespace ShoeStoreManagement.Core.Models;

public class Profile
{
    public string? _fullName { get; set; }
    public string? _avatar { get; set; }
    public Genders _gender { get; set; }
    public int _age { get; set; }
    public string[] _addresses { get; set; } = new string[0];
}
