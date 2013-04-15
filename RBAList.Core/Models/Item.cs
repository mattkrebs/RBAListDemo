using System;

namespace RBAList.Core.Models
{
    public class Item
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public double? AskingPrice { get; set; }
        public double? RetailPrice { get; set; }
        public bool Sold { get; set; }
        public string ImageGuid { get; set; }
        public string ImageName { get; set; }
        
        public string SAS { get; set; }
        public string UserId { get; set; }

        #endregion


        #region Constructors

        public Item()
        {
            Name = string.Empty;
            Description = string.Empty;
            UserId = string.Empty;
            ImageGuid = string.Empty;
            ImageName = string.Empty;
        }

        #endregion
    }
}