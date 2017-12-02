using System;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Service
{
    public class WeatherServiceCredentialsValidator : UserNamePasswordValidator
    {
        public override void Validate(string userName, string password)
        {
            if (userName == null || password == null)
            {
                throw new ArgumentNullException();
            }

            if (userName != "admin" || password != "password")
            {
                throw new SecurityTokenException("Unknown Username or Incorrect Password.");
            }
        }
    }
}
