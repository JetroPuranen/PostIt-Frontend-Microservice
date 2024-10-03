namespace PostIt.Application.Dto
{
    public class CreateUserDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? EmailAddress { get; set; }
        public string? HomeAddress { get; set; }
        public DateTime BirthDay { get; set; }
    }

    public class UserDetailDto
    {
            public string UserId { get; set; }
            public string? Username { get; set; }
            public string? FirstName { get; set; }
            public string? EmailAddress { get; set; }
            public string? HomeAddress { get; set; }
            public DateTime? BirthDay { get; set; }
            public string? SurName { get; set; }
            public byte[]? ProfilePictureBytes { get; set; }
    }
}
