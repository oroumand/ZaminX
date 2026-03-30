namespace ZaminX.Mapper.WebApiSample.Models;

public sealed class UserDto
{
    public long Id { get; set; }
    public required string FullName { get; set; }
}