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
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;
using System.Threading.Tasks;

namespace RBAList.Core
{
    public static class RBAListRepository
    {

        public static readonly MobileServiceClient MobileService = new MobileServiceClient("https://choicetest.azure-mobile.net/", "YEvwSAlNbuwdouUHcJjZEUeEQgnyJP18");

        public static void GetItemsAsync(Action<List<Item>> success)
        {
            List<Item> RummageItems = new List<Item>();
            MobileService.GetTable<Item>().Where(e => !e.Sold).ToListAsync().ContinueWith(t =>
            {
                var ex = t.Exception;

                if (t.Status == TaskStatus.RanToCompletion)
                {
                    RummageItems = t.Result;
                    success(RummageItems);
                }
            });

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

        public static void AddImage()
        {
        }

    }
}