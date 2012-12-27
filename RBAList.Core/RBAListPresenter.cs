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
    public class RBAListPresenter
    {
        public MobileServiceClient MobileService = new MobileServiceClient("https://choicetest.azure-mobile.net/", "YEvwSAlNbuwdouUHcJjZEUeEQgnyJP18");


        private static RBAListPresenter _presenter;
        public static RBAListPresenter Current
        {
            get { return _presenter ?? (_presenter = new RBAListPresenter()); }
        }


        public ItemViewModel CurrentViewModel { get; set; }

        #region DataAccess
        public void Logout()
        {
            MobileService = new MobileServiceClient("https://choicetest.azure-mobile.net/", "YEvwSAlNbuwdouUHcJjZEUeEQgnyJP18");

        }
        public void GetItemsAsync(Action<List<ItemViewModel>> success)
        {
            List<ItemViewModel> RummageItems = new List<ItemViewModel>();
            MobileService.GetTable<Item>().Where(e => !e.Sold).ToListAsync().ContinueWith(t =>
            {
                var ex = t.Exception;
                Console.WriteLine("Starting Get Item Continuation");
                if (t.Status == TaskStatus.RanToCompletion)
                {
                    Task<List<ItemImage>>[] tasks = new Task<List<ItemImage>>[t.Result.Count];
                    for (int i = 0; i < t.Result.Count; i++)
                    {
                  
                        ItemViewModel ivm = new ItemViewModel();
                        ivm.Item = t.Result[i];

                        tasks[i] = MobileService.GetTable<ItemImage>().Where(x => x.ImageGuid == t.Result[i].ImageGuid).ToListAsync();
                        tasks[i].ContinueWith(t2 =>
                        {
                            Console.WriteLine("Starting Get ItemImage Continuation");
                            if (t2.Status == TaskStatus.RanToCompletion && ((List<ItemImage>)t2.Result).Count > 0)
                            {
                                var image = ((List<ItemImage>)t2.Result)[0];
                                if (image != null)
                                    ivm.ItemImage = image;
                            }
                            Console.WriteLine("Finding Image");
                        });
                      

                       RummageItems.Add(ivm);
                    }
                    Task.WaitAll(tasks);
                    Console.WriteLine("All ItemImages tasks completed");
                    success(RummageItems);
                }
            },TaskScheduler.FromCurrentSynchronizationContext());

        }
        
        public void GetItemAsync(int id, Action<Item> success)
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

        public void AddItem(Item item)
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

        public void AddItemAsync(ItemViewModel item, Action success)
        {
            var itemImage = item.ItemImage;
            var tItem = item.Item;


            if (itemImage != null && !string.IsNullOrEmpty(itemImage.ImageBase64))
            {
                tItem.ImageGuid = itemImage.ImageGuid;
                
                IMobileServiceTable<ItemImage> entryImageTable = MobileService.GetTable<ItemImage>();

                entryImageTable.InsertAsync(itemImage).ContinueWith(t2 => { 
                    var ex2 = t2.Exception;
                    if (ex2 != null)
                        Debug.WriteLine("Error Adding Image: " + ex2.InnerException.StackTrace);                     
                });
                    
                
            }
            else
                tItem.ImageGuid = string.Empty;

            var continuation = new Action<Task>(t =>
            {               
                success();
            });

            if (tItem.Id <= 0)
                MobileService.GetTable<Item>().InsertAsync(tItem).ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
            else
                MobileService.GetTable<Item>().UpdateAsync(tItem).ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
        }
        #endregion

        #region Authentication

       

        #endregion

    }
}