namespace PostIt.Application.Dto;
public class PostDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } 
    public string? Caption { get; set; }
    public byte[]? ImageData { get; set; }
    public List<string>? Comments { get; set; }
    public int LikeCount { get; set; }
    public List<string>? WhoHasLiked { get; set; }
}

