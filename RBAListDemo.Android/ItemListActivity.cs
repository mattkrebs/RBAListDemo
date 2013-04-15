using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using RBAList.Core;

namespace RBAListDemo.Android
{
    [Activity(Label = "RBA List", Icon = "@drawable/icon")]
    public class ItemListActivity : Activity
    {
        #region Variables

        private ItemAdapter _adapter;
        private Button _btnAdd;
        private ListView _listView;

        #endregion


        #region Methods

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            //RequestWindowFeature(WindowFeatures.ActionBar);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.ItemListView);
            ActionBar.SetDisplayShowHomeEnabled(false);
            // Get our button from the layout resource,
            // and attach an event to it
            _listView = FindViewById<ListView>(Resource.Id.listItems);
            _btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            //_btnAdd.Click += _btnAdd_Click;
            _adapter = new ItemAdapter(this);
            _listView.ItemClick += _listView_ItemClick;
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_listView.Adapter == null)
                _listView.Adapter = _adapter;
        }

        //private void _btnAdd_Click(object sender, EventArgs e)
        //{
        //    StartActivity(typeof (ItemCreateActivity));
        //}

        private void _listView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            RBAListPresenter.Current.CurrentViewModel = _adapter[e.Position];
            StartActivity(typeof (ItemDetailActivity));
        }

        #endregion


        #region MENU

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ListsView_Menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.addList:
                    StartActivity(typeof (ItemCreateActivity));
                    return true;
                case Resource.Id.logout:
                    SettingsPresenter.Current.Logout(Redirect);
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void Redirect()
        {
            var i = new Intent(this, typeof (SplashScreen));
            i.AddFlags(ActivityFlags.ClearTop);
            StartActivity(i);
        }

        #endregion
    }
}