using System;


namespace RBAList.Core.Models
{
    public class ItemImage
    {
        #region Properties

        public int Id { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageGuid { get; set; }

        #endregion


        #region Constructors

        public ItemImage()
        {
            ImageBase64 = string.Empty;
            ImageGuid = Guid.NewGuid().ToString();
        }

        #endregion
    }
}