namespace PostIt.Domain.Data;

public class Posts
{
    public Guid UserId { get; set; }
    public string? ImageData { get; set; }  
    public string? Caption { get; set; }
    public List<string>? Comments { get; set; }
    public int LikeCount { get; set; }
    public List<string>? WhoHasLiked { get; set; }
}
