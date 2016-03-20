using System;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Util;
using Android.Locations;
using SmartScheduler;
using Android.Content.PM;
using Android.Content;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace SmartScheduler
{
	[Activity (Theme = "@android:style/Theme.Holo.Light", Label = "Smart Scheduler", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		bool isFirst = true;
		PersonalData data;
		double addedDays = 0;
		int addedMonths = 0;
		List<ApiInteractionMember> stream_schedule = new List<ApiInteractionMember> ();
		List<ApiInteractionMember> added_events = new List<ApiInteractionMember> ();
		List<string> today_schedule_for_list = new List<string>();
		List<ApiInteractionMember> today_schedule = new List<ApiInteractionMember>();

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
			SetContentView(Resource.Layout.Main);

			ActionBar.Tab tabSettings = ActionBar.NewTab();
			ActionBar.Tab tabCalendar = ActionBar.NewTab();
			ActionBar.Tab tabSchedule = ActionBar.NewTab();
			ActionBar.Tab tabEdit = ActionBar.NewTab();

			tabSettings.SetText("Settings");
			//tab.SetIcon(Resource.Drawable.settingssmall);
			tabSettings.TabSelected += (sender, args) => {

				SetContentView(Resource.Layout.Settings);

				Button btn_ok;
				EditText text_name;
				EditText text_second_name;
				EditText text_middle_name;
				EditText text_group;

				btn_ok = FindViewById<Button> (Resource.Id.buttonOk);
				text_name = FindViewById<EditText> (Resource.Id.editTextName);
				text_second_name = FindViewById<EditText> (Resource.Id.editTextSecondName);
				text_middle_name = FindViewById<EditText> (Resource.Id.editTextMiddleName);
				text_group = FindViewById<EditText> (Resource.Id.editTextGroup);

				btn_ok.Click += delegate {

					var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
					var filePath = System.IO.Path.Combine (documentsPath, "private.txt");

					System.IO.File.WriteAllText (filePath, text_name.Text + "\n" + text_second_name.Text + "\n" + text_middle_name.Text + "\n" + text_group.Text);

					data = new PersonalData(text_name.Text, text_second_name.Text, text_middle_name.Text, text_group.Text);

					stream_schedule = new List<ApiInteractionMember> ();

					tabSchedule.Select();
				};
			};
			
			ActionBar.AddTab(tabSettings);

			tabCalendar.SetText("Calendar");
			//tab.SetIcon(Resource.Drawable.calendarsmall);
			tabCalendar.TabSelected += (sender, args) => {

				SetContentView(Resource.Layout.Calendar);

				CalendarView calendar = FindViewById<CalendarView> (Resource.Id.calendarView1);

				calendar.DateChange += (object sender_calendar, CalendarView.DateChangeEventArgs e_calendar) =>
				{
					var day_of_month = e_calendar.DayOfMonth;
					var current_day = DateTime.Now.Day;

					var month = e_calendar.Month;
					var current_month = DateTime.Now.Month;

					addedDays = day_of_month - current_day;
					addedMonths = month - current_month;

					tabSchedule.Select();
				};
			};
			
			ActionBar.AddTab(tabCalendar);

			tabSchedule.SetText("My Schedule");
			//tab.SetIcon(Resource.Drawable.thirdsmall);
			tabSchedule.TabSelected += (sender, args) => {

				data = getPersonalData();

				SetContentView(Resource.Layout.Schedule);

				ListView listView;

				listView = FindViewById<ListView> (Resource.Id.listView1);

				DateTime current_date = DateTime.Now.AddDays(addedDays);
				string date = current_date.Year.ToString() + ".";

				if (current_date.Month.ToString().Length == 1)
				{
					date += "0" + current_date.Month.ToString() + ".";
				}
				else
				{
					date += current_date.Month.ToString() + ".";
				}

				if (current_date.Day.ToString().Length == 1)
				{
					date += "0" + current_date.Day.ToString("");
				}
				else
				{
					date += current_date.Day.ToString();
				}

				if (stream_schedule.Count == 0)
				{

					ApiInteraction api = new ApiInteraction(this.Assets);

					var full_schedule = api.GetListOfApiMembers();

					foreach (var item in full_schedule)
					{
						if (item.subGroup != null && item.subGroup.Contains(data.GetGroup()))
						{
							stream_schedule.Add(item);
						}
						else if (item.stream != null && item.stream.Contains(data.GetGroup()))
						{
							stream_schedule.Add(item);
						}
					}
				}

				today_schedule_for_list = new List<string>();
				today_schedule = new List<ApiInteractionMember>();

				foreach(var item in stream_schedule)
				{
					if ( item.date == date)
					{
						today_schedule_for_list.Add(item.beginLesson + "-" + item.endLesson + " " + item.discipline + " " + item.auditorium);
						today_schedule.Add(item);
					}
				}

				foreach(var item in added_events)
				{
					if( item.date == date )
					{
						today_schedule_for_list.Add(item.beginLesson + " " + item.discipline);
						today_schedule.Add(item);
					}
				}

				today_schedule = today_schedule.OrderBy(o => o.beginLesson).ToList();
				today_schedule_for_list.Sort();


				string[] items = today_schedule_for_list.ToArray();

				listView.Adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);

				listView.ItemClick += (object senderlist, ListView.ItemClickEventArgs e_list) =>
				{
					string message = "";

					if (today_schedule[e_list.Position].auditorium != null)
					{
						message += "Auditorium: " + today_schedule[e_list.Position].auditorium;
					}
					if (today_schedule[e_list.Position].beginLesson != null)
					{
						message += "\n\nBegin Lesson: " + today_schedule[e_list.Position].beginLesson;
					}
					if (today_schedule[e_list.Position].endLesson != null)
					{
						message += "\n\nEnd Lesson: " + today_schedule[e_list.Position].endLesson;
					}
					if (today_schedule[e_list.Position].discipline != null)
					{
						message += "\n\nDiscipline: " + today_schedule[e_list.Position].discipline;
					}
					if (today_schedule[e_list.Position].lecturer != null)
					{
						message += "\n\nLecturer: " + today_schedule[e_list.Position].lecturer;
					}
					if (today_schedule[e_list.Position].kindOfWork != null)
					{
						message += "\n\nKind Of Work: " + today_schedule[e_list.Position].kindOfWork;
					}
					if (today_schedule[e_list.Position].building != null)
					{
						message += "\n\nBuilding: " + today_schedule[e_list.Position].building;
					}
					if (today_schedule[e_list.Position].dayOfWeekString != null)
					{
						message += "\n\nDay Of Week: " + today_schedule[e_list.Position].dayOfWeekString;
					}
					if (today_schedule[e_list.Position].group != null)
					{
						message += "\n\nGroup: " + today_schedule[e_list.Position].group;
					}
					if (today_schedule[e_list.Position].subGroup != null)
					{
						message += "\n\nSubgroup: " + today_schedule[e_list.Position].subGroup;
					}
					if (today_schedule[e_list.Position].stream != null)
					{
						message += "\n\nStream: " + today_schedule[e_list.Position].stream;
					}

					AlertDialog.Builder builder = new AlertDialog.Builder(this);
					builder.SetTitle("Schedule");
					builder.SetMessage(message);
					builder.SetPositiveButton ("Ok", (sender1, args1) => {
						builder.Dispose();
					});

					builder.SetNegativeButton ("Delete this item", (sender1, args1) => {
						int index = e_list.Position;
						//today_schedule.RemoveAt(index);
						var item_need_to_delete = today_schedule[index];

						bool isFound = false;
						int i = 0;
						while (!isFound && i < added_events.Count)
						{
							if ((added_events[i].discipline == item_need_to_delete.discipline) && (added_events[i].beginLesson == item_need_to_delete.beginLesson) && (added_events[i].date == item_need_to_delete.date))
							{
								added_events.RemoveAt(i);

								isFound = true;
							}
							else
							{
								i++;
							}
						}

						i = 0;
						isFound = false;
						while (!isFound && i < stream_schedule.Count)
						{
							if ((stream_schedule[i].discipline == item_need_to_delete.discipline) && (stream_schedule[i].beginLesson == item_need_to_delete.beginLesson) && (stream_schedule[i].endLesson == item_need_to_delete.endLesson)
								&& (stream_schedule[i].date == item_need_to_delete.date))
							{
								stream_schedule.RemoveAt(i);

								isFound = true;
							}
							else
							{
								i++;
							}
						}

						tabSettings.Select();
						tabSchedule.Select();
					});

					builder.Show();
				};

				TextView textCurrentDay = FindViewById<TextView> (Resource.Id.textViewDate);
				ImageButton btn_prev;
				ImageButton btn_second;

				textCurrentDay.Text = DateTime.Now.AddDays(addedDays).ToString("yyyy-M-d dddd");

				btn_prev = FindViewById<ImageButton> (Resource.Id.imageButtonPrev);
				btn_second = FindViewById<ImageButton> (Resource.Id.imageButtonSecond);

				btn_prev.Click += delegate {
					addedDays -= 1;
					tabSettings.Select();
					tabSchedule.Select();
				};

				btn_second.Click += delegate {
					addedDays += 1;
					tabSettings.Select();
					tabSchedule.Select();
				};
			};

			ActionBar.AddTab(tabSchedule);

			tabEdit.SetText("Edit Schedule");
			//tab.SetIcon(Resource.Drawable.editsmall);
			tabEdit.TabSelected += (sender, args) => {

				SetContentView(Resource.Layout.Edit);

				Button btn_ok_add = FindViewById<Button> (Resource.Id.buttonOkAdd);
				EditText edit_text_add = FindViewById<EditText> (Resource.Id.editTextAdd);
				TimePicker time_picker = FindViewById<TimePicker> (Resource.Id.timePicker1);
				time_picker.SetIs24HourView(Java.Lang.Boolean.True);

				btn_ok_add.Click += delegate {

					var hour = time_picker.CurrentHour;
					var minutes = time_picker.CurrentMinute;

					var text = edit_text_add.Text;

					string time = "";

					if (hour.ToString().Length == 1)
					{
						time += "0" + hour.ToString();
					}
					else
					{
						time += hour.ToString();
					}

					time += ":";

					if (minutes.ToString().Length == 1)
					{
						time += "0" + minutes.ToString();
					}
					else
					{
						time += minutes.ToString();
					}

					DateTime current_date = DateTime.Now.AddDays(addedDays);
					string date = current_date.Year.ToString() + ".";

					if (current_date.Month.ToString().Length == 1)
					{
						date += "0" + current_date.Month.ToString() + ".";
					}
					else
					{
						date += current_date.Month.ToString() + ".";
					}

					if (current_date.Day.ToString().Length == 1)
					{
						date += "0" + current_date.Day.ToString("");
					}
					else
					{
						date += current_date.Day.ToString();
					}
					added_events.Add(new ApiInteractionMember("null", 0, time, "null", date, "null", 0, "null", text, "null", "null", 0, "null", "null", 0, "null", 0, "null", 0));

					tabSchedule.Select();
				};

			};

			ActionBar.AddTab(tabEdit);

			// This event fires when the ServiceConnection lets the client (our App class) know that
			// the Service is connected. We use this event to start updating the UI with location
			// updates from the Service
			App.Current.LocationServiceConnected += (object sender, ServiceConnectedEventArgs e) => {
				//Log.Debug (logTag, "ServiceConnected Event Raised");
				// notifies us of location changes from the system
				App.Current.LocationService.LocationChanged += HandleLocationChanged;
				//notifies us of user changes to the location provider (ie the user disables or enables GPS)
				App.Current.LocationService.ProviderDisabled += HandleProviderDisabled;
				App.Current.LocationService.ProviderEnabled += HandleProviderEnabled;
				// notifies us of the changing status of a provider (ie GPS no longer available)
				App.Current.LocationService.StatusChanged += HandleStatusChanged;
			};

			// Start the location service:
			App.StartLocationService();
		}

		///<summary>
		/// Updates UI with location data
		/// </summary>
		public void HandleLocationChanged(object sender, LocationChangedEventArgs e)
		{
			Android.Locations.Location location = e.Location;
			//Log.Debug (logTag, "Foreground updating");

			// these events are on a background thread, need to update on the UI thread
			RunOnUiThread (() => {
				/*latText.Text = String.Format ("Latitude: {0}", location.Latitude);
				longText.Text = String.Format ("Longitude: {0}", location.Longitude);
				altText.Text = String.Format ("Altitude: {0}", location.Altitude);
				speedText.Text = String.Format ("Speed: {0}", location.Speed);
				accText.Text = String.Format ("Accuracy: {0}", location.Accuracy);
				bearText.Text = String.Format ("Bearing: {0}", location.Bearing);*/

				double hse_latitude = 55.778405;
				double hse_longitude = 37.733783;


				if ((location.Latitude <= hse_latitude + 0.04 && location.Latitude >= hse_latitude -0.04)
					&& (location.Longitude <= hse_longitude + 0.04 && location.Longitude >= hse_longitude -0.04))
				{
					string time = "";
					DateTime current_datetime = DateTime.Now.AddDays(addedDays);
					var hour = current_datetime.Hour;
					var minutes = current_datetime.Minute;

					if (hour.ToString().Length == 1)
					{
						time += "0" + hour.ToString();
					}
					else
					{
						time += hour.ToString();
					}

					time += ":";

					if (minutes.ToString().Length == 1)
					{
						time += "0" + minutes.ToString();
					}
					else
					{
						time += minutes.ToString();
					}

					string message = "";

					int i = 0;
					bool isFound = false;
					while (!isFound && i < today_schedule.Count)
					{
						if (String.Compare(today_schedule[i].beginLesson, "06:00") > 0)
						{
							isFound = true;

							message = "Auditorium : " + today_schedule[i].auditorium + "      Time: " + today_schedule[i].beginLesson;
						}
						else
						{
							i++;
						}
					}


					// Instantiate the builder and set notification elements:
					Notification.Builder builder = new Notification.Builder (this)
						.SetContentTitle (time)
						.SetContentText (message)
						.SetSmallIcon (Resource.Drawable.logo);

					// Build the notification:
					Notification notification = builder.Build();

					// Get the notification manager:
					NotificationManager notificationManager =
						GetSystemService (Context.NotificationService) as NotificationManager;

					// Publish the notification:
					const int notificationId = 0;
					notificationManager.Notify (notificationId, notification);
				}
			});
		}

		public void HandleProviderDisabled(object sender, ProviderDisabledEventArgs e)
		{
			//Log.Debug (logTag, "Location provider disabled event raised");
		}

		public void HandleProviderEnabled(object sender, ProviderEnabledEventArgs e)
		{
			//Log.Debug (logTag, "Location provider enabled event raised");
		}

		public void HandleStatusChanged(object sender, StatusChangedEventArgs e)
		{
			//Log.Debug (logTag, "Location status changed, event raised");
		}

		private PersonalData getPersonalData()
		{
			PersonalData data;

			try
			{
				var documentsPath = System.Environment.GetFolderPath (System.Environment.SpecialFolder.Personal);
				var filePath = System.IO.Path.Combine (documentsPath, "private.txt");
				var result = System.IO.File.ReadAllLines(filePath);

				data = new PersonalData(result[0], result[1], result[2], result[3]);
			}
			catch
			{
				//result = { "First Name", "Second Name", "Middle Name", "Group" };

				data = new PersonalData("First Name", "Second Name", "Middle Name", "Group");
			}

			return data;
		}
	}
}


