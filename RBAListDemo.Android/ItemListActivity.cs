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
    [Activity(Label = "RBAListDemo.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class ItemListActivity : Activity
    {
        int count = 1;
        private ListView _listView;
        private Button _btnAdd;

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
           
            
        }

        void _btnAdd_Click(object sender, EventArgs e)
        {
            StartActivity(typeof(ItemCreateActivity));
        }
        protected override void OnResume()
        {
            base.OnResume();

            _listView.Adapter = new ItemAdapter(this); ;
        }

      

         
            


        
    }
}

