using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using RBAList.Core;
using System.Collections.Generic;
using RBAList.Core.Models;

namespace RBAListDemo.Android
{
    [Activity(Label = "RBA List")]
    public class ItemListActivity : Activity
    {
        int count = 1;
        private ListView _listView;
        private ProgressBar _progress;
        private Button _btnAdd;
        private ItemAdapter _adapter;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ItemListView);

            // Get our button from the layout resource,
            // and attach an event to it
            _listView = FindViewById<ListView>(Resource.Id.listItems);
            _btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            _btnAdd.Click += _btnAdd_Click;
            _adapter = new ItemAdapter(this);
            _listView.ItemClick += _listView_ItemClick;
        }

        void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RBAListPresenter.Current.CurrentViewModel = _adapter[e.Position];
            StartActivity(typeof(ItemDetailActivity));
        }

        void _btnAdd_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ItemCreateActivity));
        }
        protected override void OnResume()
        {
            base.OnResume();

            _listView.Adapter = _adapter;
            
        }









        
    }
}

