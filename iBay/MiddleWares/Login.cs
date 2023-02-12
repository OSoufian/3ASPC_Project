namespace iBay.MiddleWares
{
    public class Login
    {
        public string Pseudo { get; set; }
        public string Password { get; set; }

        public Login()
        {
            Pseudo = "ps";
            Password = "pas";
        }

        public Login(string pseudo, string password)
        {
            Pseudo = pseudo;
            Password = password;
        }
    }
}
