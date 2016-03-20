
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

namespace SmartScheduler
{
	[Activity (Label = "SettingsActivity")]			
	public class SettingsActivity : Activity
	{
		Button btn_ok;
		EditText text_name;
		EditText text_second_name;
		EditText text_middle_name;
		EditText text_group;
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Settings);
			// Create your application here

			btn_ok = FindViewById<Button> (Resource.Id.buttonOk);
			text_name = FindViewById<EditText> (Resource.Id.editTextName);
			text_second_name = FindViewById<EditText> (Resource.Id.editTextSecondName);
			text_middle_name = FindViewById<EditText> (Resource.Id.editTextMiddleName);
			text_group = FindViewById<EditText> (Resource.Id.editTextGroup);

			btn_ok.Click += delegate {

				var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
				var filePath = System.IO.Path.Combine (documentsPath, "private.txt");

				System.IO.File.WriteAllText (filePath, text_name.Text + "\n" + text_second_name.Text + "\n" + text_middle_name + "\n" + text_group);

				this.Finish();
			};
		}
	}
}

