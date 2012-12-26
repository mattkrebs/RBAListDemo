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
		}

        public int Id { get; set; }
        public string ImageBase64 { get; set; }

    }
}