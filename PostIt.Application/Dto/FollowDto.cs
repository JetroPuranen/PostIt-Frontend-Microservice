namespace PostIt.Application.Dto
{
    public class FollowerDto
    {
        public string? Username { get; set; }  //user who is going to follow someone
        public string? FollowerUsername { get; set; }  //username of the user they want to follow
    }
}
