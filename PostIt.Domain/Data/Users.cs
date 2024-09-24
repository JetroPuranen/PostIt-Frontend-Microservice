

namespace PostIt.Domain.Data
{
    public class Users
    {
        public string? UserId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; } 
        public string? FirstName { get; set; }
        public string? SurName { get; set; }
        public string? EmailAddress { get; set; }
        public string? HomeAddress { get; set; }
        public DateTime BirthDay { get; set; }
        public string? ProfilePicture { get; set; } 



        public virtual ICollection<Users> Followers { get; set; } = new List<Users>();
        public virtual ICollection<Users> Following { get; set; } = new List<Users>();
        
        
  
       
        public Users ToEntity()
        {
            return new Users
            {
                Username = this.Username,
                FirstName = this.FirstName,
                SurName = this.SurName,
                EmailAddress = this.EmailAddress,
                HomeAddress = this.HomeAddress,
                BirthDay = this.BirthDay,
                ProfilePicture = this.ProfilePicture,
                Password = this.Password, 
                Followers = this.Followers,
                Following = this.Following,
                
            };
        }
    }
}
