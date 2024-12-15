namespace PostIt.Application.Dto
{
    public class FollowerDto
    {
        public string? UserId { get; set; }  //user who is going to follow someone
        public string? FollowerUserId { get; set; }  //username of the user they want to follow
    }
}
