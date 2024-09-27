namespace PostIt.Application.Dto
{
    public class UnfollowDto
    {
        public string? Username { get; set; }  //user who wants to unfollow
        public string? UnfollowUsername { get; set; }  //user to be unfollowed
    }
}
