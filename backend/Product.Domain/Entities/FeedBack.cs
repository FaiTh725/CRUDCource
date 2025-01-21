using CSharpFunctionalExtensions;
using Product.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Formats.Tar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductEntity = Product.Domain.Models.Product;

namespace Product.Domain.Entities
{
    public class FeedBack
    {
        public const int MAX_RATE = 5;
        public const int MIN_RATE = 0;

        public long Id { get; }

        public string FeedBackText { get; set; } = string.Empty;

        public DateTime SendTime { get; init; }

        public int Rate { get; set; }

        public string ImageFilder
        {
            get => "FeedBacks/" + Id.ToString();
        }

        public ProductEntity Product { get; init; }

        public Account Owner { get; init; }

        public FeedBack() { }

        private FeedBack(
            string text,
            int rate,
            ProductEntity productEntity,
            Account owner)
        {
            SendTime = DateTime.Now;

            FeedBackText = text;
            Product = productEntity;
            Owner = owner;
            Rate = rate;
        }

        public static Result<FeedBack> Initialize(
            int rate,
            ProductEntity productEntity,
            Account owner,
            string text="")
        {
            if(productEntity is null)
            {
                return Result.Failure<FeedBack>("Product is required field");
            }

            if(owner is null)
            {
                return Result.Failure<FeedBack>("Owner is required field");
            }

            if(rate < MIN_RATE || rate > MAX_RATE)
            {
                return Result.Failure<FeedBack>($"Rate should be {MIN_RATE} to {MAX_RATE}");
            }

            if(text is null)
            {
                return Result.Failure<FeedBack>("Text should not be null");
            }

            return Result.Success(new FeedBack(
                text,
                rate,
                productEntity,
                owner));
        }
    }
}
