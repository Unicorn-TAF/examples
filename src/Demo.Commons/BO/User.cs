namespace Demo.Commons.BO
{
    public class User
    {
        public User(string title, string givenName, string familyName, string email, string password) 
        {
            Title = title;
            GivenName = givenName;
            FamilyName = familyName;
            Email = email;
            Password = password;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Title { get; set; }
        
        public override string ToString() =>
            $"user {Title}. {GivenName} {FamilyName}";
    }
}
