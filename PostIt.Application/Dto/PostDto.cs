namespace PostIt.Application.Dto
{
    public class PostDto
    {
        public Guid? Id { get; set; }
        public Guid? UserId { get; set; }
        public byte[] ImageData { get; set; }
        public string? Caption { get; set; }
        public List<CommentDto> Comments { get; set; } = new();
        public int? LikeCount { get; set; }
        public List<Guid>? WhoHasLiked { get; set; }
        public List<Guid>? WhoHasCommented { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CommentDto
    {
        public Guid? UserId { get; set; }
        public string? Comment { get; set; }
    }
}
