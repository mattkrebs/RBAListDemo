using System;
using System.IO;
using RBAList.Core.Models;

namespace RBAList.Core
{
    public class ItemViewModel
    {
        #region Properties

        public Item Item { get; set; }

        public ItemImage ItemImage { get; set; }

        public bool IsLoading { get; set; }

        #endregion


        #region Methods

        public void AddPhoto(Stream base64ImageStream)
        {
            byte[] imgData;
            using (var ms = new MemoryStream())
            {
                base64ImageStream.CopyTo(ms);
                imgData = ms.ToArray();
            }

            var strImg = Convert.ToBase64String(imgData);

            if (ItemImage == null)
            {
                ItemImage = new ItemImage {
                    ImageBase64 = strImg
                };
            }
            else
            {
                ItemImage.ImageBase64 = strImg;
            }
        }

        public void AddPhoto(byte[] imgData)
        {
            var strImg = Convert.ToBase64String(imgData);

            if (ItemImage == null)
            {
                ItemImage = new ItemImage {
                    ImageBase64 = strImg
                };
            }
            else
            {
                ItemImage.ImageBase64 = strImg;
            }
        }

        #endregion
    }
}