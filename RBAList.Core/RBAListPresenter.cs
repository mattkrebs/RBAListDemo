using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using RBAList.Core.Models;

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


            if (itemImage != null && !string.IsNullOrEmpty(itemImage.ImageBase64))
            {
                tItem.ImageGuid = itemImage.ImageGuid;

                var entryImageTable = MobileService.GetTable<ItemImage>();

                entryImageTable.InsertAsync(itemImage).ContinueWith(t2 =>
                {
                    var exception = t2.Exception;
                    if (exception != null)
                    {
                        Debug.WriteLine("Error Adding Image: " + exception.InnerException.StackTrace);
                    }
                });
            }
            else
            {
                tItem.ImageGuid = string.Empty;
            }

            var continuation = new Action<Task>(t => success());

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

                    var imageListTask = MobileService.GetTable<ItemImage>().Where(x => x.ImageGuid == currentItem.ImageGuid).ToListAsync();
                    imageTaskArray[i] = imageListTask;
                    var indexString = i.ToString(CultureInfo.InvariantCulture);

                    Console.WriteLine("Set ItemImage Continuation: " + indexString);
                    imageListTask.ContinueWith(imageListTaskContinue =>
                    {
                        Console.WriteLine("Begin Get ItemImage Continuation");
                        if (imageListTaskContinue.Status == TaskStatus.RanToCompletion && imageListTaskContinue.Result != null && imageListTaskContinue.Result.Count > 0)
                        {
                            var image = (imageListTaskContinue.Result)[0];
                            if (image != null)
                            {
                                itemViewModel.ItemImage = image;
                            }
                        }
                        Console.WriteLine("End Get ItemImage Continuation: " + indexString);
                    });

                    itemViewModelList.Add(itemViewModel);
                }

                Task.WaitAll(imageTaskArray);
                Console.WriteLine("All ItemImages Tasks Complete");

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