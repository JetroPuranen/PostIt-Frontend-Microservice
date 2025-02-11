using Newtonsoft.Json;

namespace PostIt.Domain.Data;

public class Posts
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public byte[] ImageData { get; set; }
    public string Caption { get; set; }
    public int LikeCount { get; set; }
    public ICollection<PostComment> Comments { get; set; } = new List<PostComment>();

    [JsonProperty("whoHasLiked")]
    public ICollection<Guid> WhoHasLiked { get; set; } = new List<Guid>();

    public DateTime CreatedAt { get; set; }
}

public class PostComment
{
    public Guid UserId { get; set; }
    public string Comment { get; set; }
   
}

public class PostLike
{
    public Guid UserId { get; set; }
}
public class UpdatePost
{
    public Guid? Id { get; set; }
    public string? Caption { get; set; }
    public Guid? LikedByUserId { get; set; }
    public PostComment? Comment { get; set; }
}