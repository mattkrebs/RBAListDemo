using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;
using System.Threading.Tasks;
using System.Diagnostics;

namespace RBAList.Core
{
    public static class RBAListRepository
    {

        public static readonly MobileServiceClient MobileService = new MobileServiceClient("https://choicetest.azure-mobile.net/", "YEvwSAlNbuwdouUHcJjZEUeEQgnyJP18");

        //public static void GetItemsAsync(Action<List<Item>> success)
        //{
        //    List<Item> RummageItems = new List<Item>();
        //    MobileService.GetTable<Item>().Where(e => !e.Sold).ToListAsync().ContinueWith(t =>
        //    {
        //        var ex = t.Exception;

        //        if (t.Status == TaskStatus.RanToCompletion)
        //        {
        //            RummageItems = t.Result;
        //            success(RummageItems);
        //        }
        //    });

        //}
        public static void GetItemsAsync(Action<List<ItemViewModel>> success)
        {
            List<ItemViewModel> RummageItems = new List<ItemViewModel>();
            MobileService.GetTable<Item>().Where(e => !e.Sold).ToListAsync().ContinueWith(t =>
            {
                var ex = t.Exception;

                if (t.Status == TaskStatus.RanToCompletion)
                {
                    foreach (var item in t.Result)
                    {
                        ItemViewModel ivm = new ItemViewModel();
                        ivm.Item = item;

                       var image = MobileService.GetTable<ItemImage>().LookupAsync(item.ImageId).Result;
                       if (image != null)
                           ivm.ItemImage = image;

                       RummageItems.Add(ivm);
                    } 
                    success(RummageItems);
                }
            },TaskScheduler.FromCurrentSynchronizationContext());

        }
        
        public static void GetItemAsync(int id, Action<Item> success)
        {
            MobileService.GetTable<Item>().LookupAsync(id).ContinueWith(t =>
            {
                var ex = t.Exception;

                if (t.Status == TaskStatus.RanToCompletion)
                {
                    success(t.Result);
                }
            });
        }


        public static void AddItem(Item item)
        {
            if (item.Id == 0)
                MobileService.GetTable<Item>().InsertAsync(item).ContinueWith(t => {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine(t.Exception);
                    }
                
                });
            else
                MobileService.GetTable<Item>().UpdateAsync(item);
            
        }

        public static void AddItemAsync(ItemViewModel item, Action success)
        {
            var itemImage = item.ItemImage;
            var tItem = item.Item;


            if (itemImage != null && !string.IsNullOrEmpty(itemImage.ImageBase64))
            {
                tItem.ImageId = itemImage.Id;

                IMobileServiceTable<ItemImage> entryImageTable = MobileService.GetTable<ItemImage>();

                entryImageTable.InsertAsync(itemImage).ContinueWith(t2 => { 
                    var ex2 = t2.Exception;
                    if (ex2 != null)
                        Debug.WriteLine("Error Adding Image: " + ex2.InnerException.StackTrace); 

                    
                });
                    
                
            }
            else
                tItem.ImageId = null;

            var continuation = new Action<Task>(t =>
            {
                success();
            });

            if (tItem.Id <= 0)
                MobileService.GetTable<Item>().InsertAsync(tItem).ContinueWith(continuation);
            else
                MobileService.GetTable<Item>().UpdateAsync(tItem).ContinueWith(continuation);
        }


    }
}