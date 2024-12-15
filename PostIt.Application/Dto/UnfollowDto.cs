namespace PostIt.Application.Dto
{
    public class UnfollowDto
    {
        public string? UserId { get; set; }  //user who wants to unfollow
        public string? UnfollowUserId { get; set; }  //user to be unfollowed
    }
}
