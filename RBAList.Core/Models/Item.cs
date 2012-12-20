using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RBAList.Core.Models
{
    public class Item
    {

        public Item()
        {
            Name = string.Empty;
            Description = string.Empty;
            UserId = string.Empty;
            ImageGuid = string.Empty;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public double? AskingPrice { get; set; }
        public double? RetailPrice { get; set; }  
        public bool Sold { get; set; }
        public string ImageGuid { get; set; }
        public string UserId { get; set; }

    }
}