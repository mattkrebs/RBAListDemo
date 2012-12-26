using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RBAList.Core.Models
{
    public class ItemImage
    {

        public ItemImage()
		{			
			ImageBase64 = string.Empty;
            ImageGuid = Guid.NewGuid().ToString();
		}

        public int Id { get; set; }
        public string ImageBase64 { get; set; }
        public string ImageGuid { get; set; }

    }
}