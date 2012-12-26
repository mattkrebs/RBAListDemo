using RBAList.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace RBAList.Core
{
    public class ItemViewModel
    {
        public Item Item { get; set; }

        public ItemImage ItemImage { get; set; }

        public bool IsLoading { get; set; }


        public void AddPhoto(Stream base64ImageStream)
        {
            byte[] imgData;
            using (var ms = new MemoryStream())
            {
                base64ImageStream.CopyTo(ms);
                imgData = ms.ToArray();
            }

            var strImg = Convert.ToBase64String(imgData);

            if (this.ItemImage == null)
                this.ItemImage = new ItemImage() { ImageBase64 = strImg };
            else
                this.ItemImage.ImageBase64 = strImg;
        
        }

        public void AddPhoto(byte[] imgData)
        {          
            var strImg = Convert.ToBase64String(imgData);

            if (this.ItemImage == null)
                this.ItemImage = new ItemImage() { ImageBase64 = strImg };
            else
                this.ItemImage.ImageBase64 = strImg;

        }

        

        
    }
}