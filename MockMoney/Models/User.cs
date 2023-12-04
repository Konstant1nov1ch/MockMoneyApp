namespace MockMoney.Models;

public class User
{
        public string Username { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    
        public string Token { get; set; }


        public User(string username, string login, string password)
        {
            Username = username;
            Login = login;
            Password = password;
        }
    
}