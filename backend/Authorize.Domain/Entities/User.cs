using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Authorize.Domain.Entities
{
    public class User
    {
        private const int MAX_EMAIL_LENGTH = 30;
        private const int MAX_PASSWORD_LENGTH = 20;
        private const int MIN_PASSWORD_LENGTH = 5;

        public User()
        {

        }

        private User(long id, string email, string password, Roles role)
        {
            Id = id;
            Email = email;
            Password = password;
            Role = role;
        }


        public long Id { get; private set; }
         
        public string Email { get; private set; } = string.Empty;

        public string Password { get; private set; } = string.Empty;

        public Roles Role { get; private set; }

        public static Result<User> Initialize(string email, string password)
        {
            return Initialize(-1, email, password, new Roles() { Role = "User"});
        }

        public static Result<User> Initialize(long id, string email, string password, Roles role)
        {
            if(string.IsNullOrEmpty(email) || email.Length > MAX_EMAIL_LENGTH)
            {
                return Result.Failure<User>("user email shoud has lengh 1 to 30");
            }

            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if (!emailRegex.IsMatch(email))
            {
                return Result.Failure<User>("User email shoud contains @ and dot after this simbol");
            }

            Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d).+$");

            if(password.Length < MIN_PASSWORD_LENGTH 
                || password.Length > MAX_PASSWORD_LENGTH
                || !passwordRegex.IsMatch(password))
            {
                return Result.Failure<User>("User password should contains one letter and one number and be in range 5 - 30");
            }

            if (role == null)
            {
                return Result.Failure<User>("Role shoudn't null");
            }

            return Result.Success<User>(new User(id, email, password, role));
        }

    }
}
