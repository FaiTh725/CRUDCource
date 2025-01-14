using CSharpFunctionalExtensions;
using Product.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Product.Domain.Models
{
    public class Account
    {
        private const int MAX_NAME_LENGTH = 20;
        private const int MIN_NAME_LENGTH = 3;
        private const int MAX_EMAIL_LENGTH = 30;

        public long Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public List<CartItem> ShopingCart {  get; set; } = new List<CartItem>();

        public List<CartItem> ShopingHistory { get; set; } = new List<CartItem>();

        public List<Product> SoldProducts { get; set; } = new List<Product>();
    
        public Account()
        {

        }

        private Account(
            string name,
            string email)
        {
            Name = name;
            Email = email;
        }

        public static Result<Account> Initialize(string name, string email)
        {
            if(string.IsNullOrEmpty(name))
            {
                return Result.Failure<Account>("Name shouldn't be empty");
            }

            if(name.Length > MAX_NAME_LENGTH || name.Length < MIN_NAME_LENGTH)
            {
                return Result.Failure<Account>("Name length should be in the range from 3 to 20");
            }

            Regex emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if(!emailRegex.IsMatch(email))
            {
                return Result.Failure<Account>("Invalid email, should containts @ and dot after @ symbol");
            }

            if(email.Length > MAX_EMAIL_LENGTH)
            {
                return Result.Failure<Account>("Email is too large(greate that 30)");
            }
            

            return Result.Success<Account>(new Account(name, email));
        }
    }
}
