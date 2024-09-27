namespace PostIt.Application.Dto;
public class PostDto
{
    public Guid UserId { get; set; } 
    public string? Caption { get; set; }
    public List<string>? Comments { get; set; }
    public int LikeCount { get; set; }
    public List<string>? WhoHasLiked { get; set; }
}

