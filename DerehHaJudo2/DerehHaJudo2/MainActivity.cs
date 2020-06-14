using Android;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Telephony;
using Android.Views;
using Android.Widget;
using ES.DMoral.ToastyLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace DerehHaJudo2
{
    [Activity(Label = "הצהרת בריאות", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            TryToGetPermissions();
            sp = this.GetSharedPreferences("details", FileCreationMode.Private);
            trainers.Add("בחר מאמן");
            trainers.Add("עידו");
            trainers.Add("נועם");
            trainers.Add("נצי");
            BuildScreen();
        }

        #region RuntimePermissions

        async Task TryToGetPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23)
            {
                await GetPermissionsAsync();
                return;
            }

        }
        const int RequestLocationId = 0;

        readonly string[] PermissionsGroupLocation =
            {
                            //TODO add more permissions
                            Manifest.Permission.SendSms,
                            Manifest.Permission.WriteSms,
             };
        async Task GetPermissionsAsync()
        {
            const string permission = Manifest.Permission.AccessFineLocation;

            if (CheckSelfPermission(permission) == (int)Android.Content.PM.Permission.Granted)
            {
                //TODO change the message to show the permissions name
                return;
            }
            if (ShouldShowRequestPermissionRationale(permission))
            {
                //set alert for executing the task
                Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
                alert.SetTitle("Permissions Needed");
                alert.SetMessage("The application need SMS permissions to continue");
                alert.SetPositiveButton("Request Permissions", (senderAlert, args) =>
                {
                    RequestPermissions(PermissionsGroupLocation, RequestLocationId);
                });
                alert.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
                });
                Dialog dialog = alert.Create();
                dialog.Show();
                return;
            }
            RequestPermissions(PermissionsGroupLocation, RequestLocationId);
        }
        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == (int)Android.Content.PM.Permission.Granted)
                        {

                        }
                        else
                        {
                            //Permission Denied :(
                            Toast.MakeText(this, "SMS permissions denied", ToastLength.Short).Show();

                        }
                    }
                    break;
            }
            //base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        //https://github.com/egarim/XamarinAndroidSnippets/blob/master/XamarinAndroidRuntimePermissions
        //https://youtu.be/Uzpy3qdYXmE
        #endregion

        List<string> trainers = new List<string>();
        LinearLayout.LayoutParams WrapContParams = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.WrapContent);
        LinearLayout.LayoutParams LP1 = new LinearLayout.LayoutParams(500, 300);
        LinearLayout.LayoutParams LP2 = new LinearLayout.LayoutParams(550, 150);
        LinearLayout InsideSVLayout, OAlayout, TitleLayout, NameLayout, IDLayout, AGUDALayout, CBLayout, SpinnerLayout, SendButtonLayout, ImageLayout, TopLayout;
        TextView TitleTV, NameTV, IDTV, AGUDATV;
        EditText NameET, IDET, AGUDAET;
        CheckBox CB1, CB2;
        Button SendButton;
        Spinner TrainersSpinner;
        ImageView img;
        Color mYellow = Color.ParseColor("#fffccf");
        Color MBlue1 = Color.ParseColor("#2c3c6d");
        Color MBlue2 = Color.ParseColor("#656584");
        Color MBlue3 = Color.ParseColor("#9f989a");
        ISharedPreferences sp;
        ScrollView sv;
        private void BuildScreen()
        {
            //
            sv = new ScrollView(this)
            {
                LayoutParameters = WrapContParams,
            };
            //
            InsideSVLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Vertical,
            };
            //
            OAlayout = FindViewById<LinearLayout>(Resource.Id.MainPageLayout);
            OAlayout.SetBackgroundColor(mYellow);
            //======================================================================
            //======================================================================
            TopLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            ImageLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            img = new ImageView(this);
            img.LayoutParameters = new ViewGroup.LayoutParams(360, 250);
            img.SetImageResource(Resource.Drawable.Logo);
            img.Click += this.Img_Click;
            //
            ImageLayout.AddView(img);
            //======================================================================
            //======================================================================
            TitleLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            TitleTV = new TextView(this)
            {
                Text = "הצהרת בריאות",
                TextSize = 42,
                LayoutParameters = LP1,
            };
            TitleTV.SetTextColor(MBlue1);
            //
            TitleLayout.AddView(TitleTV);
            //======================================================================
            //======================================================================
            NameLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            NameTV = new TextView(this)
            {
                Text = "שם הספורטאי: ",
                LayoutParameters = LP1,
                TextSize = 25,
            };
            NameTV.SetTextColor(MBlue2);
            //
            NameET = new EditText(this)
            {
                LayoutParameters = LP2,
                Hint = "שם + שם משפחה",
                Text = sp.GetString("Name", ""),
                TextSize = 20,
                TextDirection = TextDirection.Rtl,
            };
            NameET.SetTextColor(Color.Black);
            NameET.SetBackgroundColor(Color.White);
            NameET.SetBackgroundResource(Resource.Drawable.MyBackground);
            //
            NameLayout.LayoutDirection = LayoutDirection.Rtl;
            //
            NameLayout.AddView(NameTV);
            NameLayout.AddView(NameET);
            //======================================================================
            //======================================================================
            IDLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            IDTV = new TextView(this)
            {
                Text = "ת.ז הספורטאי: ",
                LayoutParameters = LP1,
                TextSize = 25,
            };
            IDTV.SetTextColor(MBlue2);
            //
            IDET = new EditText(this)
            {
                LayoutParameters = LP2,
                Hint = "ת.ז",
                Text = sp.GetString("ID", ""),
                TextSize = 20,

            };
            IDET.SetBackgroundColor(Color.White);
            IDET.SetTextColor(Color.Black);
            IDET.SetBackgroundResource(Resource.Drawable.MyBackground);
            IDLayout.LayoutDirection = LayoutDirection.Rtl;
            //
            IDLayout.AddView(IDTV);
            IDLayout.AddView(IDET);
            //======================================================================
            //======================================================================
            AGUDALayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Horizontal,
            };
            //
            AGUDATV = new TextView(this)
            {
                Text = "אגודת הספורטאי: ",
                LayoutParameters = LP1,
                TextSize = 25,
            };
            AGUDATV.SetTextColor(MBlue2);
            //
            AGUDAET = new EditText(this)
            {
                LayoutParameters = LP2,
                Hint = "אגודה",
                Text = sp.GetString("AGUDA", ""),
                TextSize = 20,
                TextDirection = Android.Views.TextDirection.Rtl,
            };
            AGUDAET.SetTextColor(Color.Black);
            AGUDAET.SetBackgroundColor(Color.White);
            AGUDAET.SetBackgroundResource(Resource.Drawable.MyBackground);
            AGUDALayout.LayoutDirection = LayoutDirection.Rtl;
            AGUDALayout.AddView(AGUDATV);
            AGUDALayout.AddView(AGUDAET);
            //======================================================================
            //======================================================================
            CBLayout = new LinearLayout(this)
            {
                LayoutParameters = WrapContParams,
                Orientation = Orientation.Vertical,
            };
            //
            CB1 = new CheckBox(this)
            {
                Text = "אני מצהיר/ה כי ערכתי היום בדיקה למדידת חום גוף, בה נמצא כי חום גופי אינו עולה על 38 מעלות צלזיוס",
                TextSize = 20,
                TextDirection = Android.Views.TextDirection.Rtl,

            };
            if (!CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                CB1.LayoutDirection = LayoutDirection.Rtl;
            }
            else
            {
                CB1.LayoutDirection = LayoutDirection.Ltr;
            }
            CB1.SetTextColor(MBlue3);
            //
            CB2 = new CheckBox(this)
            {
                Text = "אני מצהיר/ה כי איני משתעל/ת וכן כי אין לי קשיים בנשימה.",
                TextSize = 20,
                TextDirection = TextDirection.Rtl,
            };
            if (!CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
            {
                CB2.LayoutDirection = LayoutDirection.Rtl;
            }
            else
            {
                CB2.LayoutDirection = LayoutDirection.Ltr;
            }
            CB2.SetTextColor(MBlue3);
            CBLayout.AddView(CB1);
            CBLayout.AddView(CB2);
            //======================================================================
            //======================================================================
            SpinnerLayout = new LinearLayout(this)
            {
                LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent),
                Orientation = Orientation.Horizontal
            };
            SpinnerLayout.SetGravity(GravityFlags.Right);
            //
            var adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleSpinnerItem, trainers);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            TrainersSpinner = new Spinner(this)
            {
                Adapter = adapter,
                LayoutParameters = new Android.Views.ViewGroup.LayoutParams(300, 120),
            };
            TrainersSpinner.SetBackgroundColor(Color.White);
            TrainersSpinner.Adapter = adapter;
            TrainersSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(TrainersSpinner_ItemSelected);
            TrainersSpinner.SetBackgroundResource(Resource.Drawable.SpinnerBackground);
            SpinnerLayout.AddView(TrainersSpinner);
            //======================================================================
            //======================================================================
            SendButtonLayout = new LinearLayout(this)
            {
                LayoutParameters = new ViewGroup.LayoutParams(ViewGroup.LayoutParams.MatchParent, 250),
                Orientation = Orientation.Vertical,
            };
            SendButtonLayout.SetGravity(GravityFlags.CenterHorizontal);
            //
            SendButton = new Button(this)
            {
                Text = "שליחה",
                TextSize = 25,
                LayoutParameters = new ViewGroup.LayoutParams(350, 150),
            };
            SendButton.SetTextColor(MBlue1);
            SendButton.SetBackgroundResource(Resource.Drawable.SpinnerBackground);
            SendButton.Click += this.SendButton_Click;
            //
            SendButtonLayout.AddView(SendButton);
            //======================================================================
            //======================================================================
            TopLayout.AddView(TitleLayout);
            TopLayout.AddView(ImageLayout);
            InsideSVLayout.AddView(TopLayout);
            InsideSVLayout.AddView(NameLayout);
            InsideSVLayout.AddView(IDLayout);
            InsideSVLayout.AddView(AGUDALayout);
            InsideSVLayout.AddView(CBLayout);
            InsideSVLayout.AddView(SpinnerLayout);
            InsideSVLayout.AddView(SendButtonLayout);
            sv.AddView(InsideSVLayout);
            OAlayout.AddView(sv);
        }

        private void Img_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "נתראה באימון!", ToastLength.Short).Show();
        }
        private void TrainersSpinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spin = (Spinner)sender;
            currTrainer = spin.GetItemAtPosition(e.Position).ToString();
            switch (currTrainer)
            {
                case "עידו":
                    CurrNumber = "0542077344";
                    Toasty.Info(this, "עידו " + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                case "נועם":
                    CurrNumber = "0546544244";
                    Toasty.Info(this, " נועם" + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                case "נצי":
                    CurrNumber = "0547682373";
                    Toasty.Info(this, " נצי" + CurrNumber, 3, true).Show();
                    c = true;
                    break;
                default:
                    c = false;
                    break;
            }
        }
        string currTrainer, CurrNumber;
        bool c = true;
        private void SendButton_Click(object sender, EventArgs e)
        {
            if (Validinput() && c)
            {
                if (!CB1.Checked || !CB2.Checked)
                {
                    Toasty.Error(this, "אנא סמן ווי בשתי בתיבות", 5, false).Show();
                }
                else
                {
                    List<string> ts = new List<string>();
                    ts.Add("שם: " + NameET.Text);
                    ts.Add("\nת.ז: " + IDET.Text);
                    ts.Add("\nאגודה: " + AGUDAET.Text);
                    ts.Add("\nמצהיר כי ערכתי היום בדיקה למדידת חום גוף, בה נמצא כי חום גופי אינו עולה על 38 מעלות צלזיוס");
                    ts.Add("\nוכי איני משתעל/ת וכן כי אין לי קשיים בנשימה");
                    string toSend = "";
                    for (int i = 0; i<ts.Count; i++)
                    {
                        toSend += ts[i];
                    }
                    var content = toSend;
                    var destinationAdd = CurrNumber;
                    SmsManager sm = SmsManager.Default;
                    if (content.Length >= 150)
                    {
                        List<string> parts = new List<string>();
                        var enumerable = Enumerable.Range(0, content.Length / 20).Select(i => content.Substring(i * 20, 20));
                        parts = enumerable.ToList();
                        sm.SendMultipartTextMessage(destinationAdd, null, parts, null, null);
                    }
                    else
                    {
                        sm.SendTextMessage(destinationAdd /*מספר טלפון*/, null, content /*תכולה*/, null, null);
                    }
                    var editor = sp.Edit();
                    editor.PutString("Name", NameET.Text);
                    editor.PutString("ID", IDET.Text);
                    editor.PutString("AGUDA", AGUDAET.Text);
                    editor.Commit();
                    Toasty.Success(this, "הצהרה נשלחה בהצלחה", 5, true).Show();
                    var activity = (Activity)this;
                    activity.FinishAffinity();
                }
            }
            else
            {
                Toasty.Error(this, "שגיאה", 5, true).Show();
            }
        }
        public bool Validinput()
        {
           if (NameET.Text == "")
            {
                Toasty.Error(this, "שם ריק", 3, true).Show();
                return false;
            }
           if (IDET.Text == "")
            {
                Toasty.Error(this, "ת.ז ריק", 3, true).Show();
                return false;
            }
           if (AGUDAET.Text == "")
            {
                Toasty.Error(this, "אגודה ריקה", 3, true).Show();
                return false;
            }
            return (IsValidName(NameET.Text) && IsValidID(IDET.Text));

        }
        public bool IsValidName(string name)
        {
            bool Tr = true;
            Tr = name.Length >= 4;
            if (!Tr)
            {
                Toasty.Error(this, "שגיאה בשם", 5, true).Show();
                return false;
            }
            else
                return Tr;
        }
        public bool IsValidID(string id)
        {
            if (id.Length != 9)
            {
                Toasty.Error(this, "ת.ז שגוי", 5, true).Show();
                return false;
            }
            else
                return true;
        }
    }
}