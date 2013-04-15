using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Web.Services;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;
using System.Net;
using System.IO;

namespace RBAList.Core
{
    public class RBAListPresenter
    {
        #region Variables

        private static RBAListPresenter _presenter;
        public MobileServiceClient MobileService = new MobileServiceClient("https://choicetest.azure-mobile.net/", "YEvwSAlNbuwdouUHcJjZEUeEQgnyJP18");

        #endregion


        #region Properties

        public static RBAListPresenter Current
        {
            get { return _presenter ?? (_presenter = new RBAListPresenter()); }
        }


        public ItemViewModel CurrentViewModel { get; set; }

        #endregion


        #region DataAccess

        public void AddItem(Item item)
        {
            if (item.Id == 0)
            {
                MobileService.GetTable<Item>().InsertAsync(item).ContinueWith(t =>
                {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine(t.Exception);
                    }
                });
            }
            else
            {
                MobileService.GetTable<Item>().UpdateAsync(item);
            }
        }

        public void AddItemAsync(ItemViewModel item, Action success)
        {
            var itemImage = item.ItemImage;
            var tItem = item.Item;
            
           
            //if (itemImage != null && !string.IsNullOrEmpty(itemImage.ImageBase64))
            //{
            //    tItem.ImageGuid = itemImage.ImageGuid;

            //    var entryImageTable = MobileService.GetTable<ItemImage>();

            //    entryImageTable.InsertAsync(itemImage).ContinueWith(t2 =>
            //    {
            //        var exception = t2.Exception;
            //        if (exception != null)
            //        {
            //            Debug.WriteLine("Error Adding Image: " + exception.InnerException.StackTrace);
            //        }
            //    });
            //}
            //else
            //{
            //    tItem.ImageGuid = string.Empty;
            //}


            //Upload image with HttpClient to the blob service using the generated item.SAS

            var client = new WebClient();
           
            var continuation = new Action<Task>(t => {
                //Get a stream of the media just captured
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(new Uri(tItem.SAS));
                    var fileStream = System.Convert.FromBase64String(itemImage.ImageBase64);
                    request.Method = "PUT";                    
                    //request.Headers.Add("Content-Type", "image/jpeg");
                    request.Headers.Add("x-ms-blob-type", "BlockBlob");
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(fileStream, 0, fileStream.Length);
                    dataStream.Close();

                   // client.UploadDataAsync(new Uri(tItem.SAS), "PUT", fileStream);
                    WebResponse response = request.GetResponse();

                    Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.InnerException);    
                }
                        
                success();            
            });

            if (tItem.Id <= 0)
            {
                MobileService.GetTable<Item>().InsertAsync(tItem).ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
            }
            else
            {
                MobileService.GetTable<Item>().UpdateAsync(tItem).ContinueWith(continuation, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void GetItemAsync(int id, Action<Item> success)
        {
            MobileService.GetTable<Item>().LookupAsync(id).ContinueWith(t =>
            {
                //var ex = t.Exception;

                if (t.Status == TaskStatus.RanToCompletion)
                {
                    success(t.Result);
                }
            });
        }

        public void GetItemsAsync(Action<List<ItemViewModel>> successAction)
        {
            var itemViewModelList = new List<ItemViewModel>();
            MobileService.GetTable<Item>().Where(e => !e.Sold).ToListAsync().ContinueWith(itemListTask =>
            {
                //var ex = itemListTask.Exception;
                Console.WriteLine("Starting Get Item Continuation");
                if (itemListTask.Status != TaskStatus.RanToCompletion)
                {
                    return;
                }

                var itemList = itemListTask.Result;
                if (itemList == null)
                {
                    return;
                }

                Console.WriteLine("itemList Count: " + itemList.Count);
                var imageTaskArray = new Task[itemList.Count];
                for (var i = 0; i < itemList.Count; i++)
                {
                    var currentItem = itemList[i];
                    var itemViewModel = new ItemViewModel();
                    itemViewModel.Item = currentItem;                  

                    itemViewModelList.Add(itemViewModel);
                }             

                successAction(itemViewModelList);
                Console.WriteLine("Ending Get Item Continuation");

            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        public void Logout()
        {
            MobileService = new MobileServiceClient("https://choicetest.azure-mobile.net/", "YEvwSAlNbuwdouUHcJjZEUeEQgnyJP18");
        }

        #endregion

    }
}