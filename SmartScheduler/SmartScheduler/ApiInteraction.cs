using System;
using System.IO;
using System.Collections.Generic;

using Android.Content.Res;
using System.Text.RegularExpressions;

using Newtonsoft.Json;

namespace SmartScheduler
{
	public class ApiInteraction
	{
		string json_text;

		public ApiInteraction (AssetManager _assets)
		{
			AssetManager assets;

			assets = _assets;

			using (StreamReader sr = new StreamReader(assets.Open("bbi144.txt")))
			{
				this.json_text = sr.ReadToEnd ();
			}
		}

		public List<ApiInteractionMember> GetListOfApiMembers()
		{
			List<ApiInteractionMember> result = JsonConvert.DeserializeObject<List<ApiInteractionMember>> (this.json_text);
				
			return result;
		}
	}

	public class ApiInteractionMember
	{
		public string auditorium { get; set; }
		public int auditoriumOid { get; set; }
		public string beginLesson { get; set; }
		public string building { get; set; }
		public string date { get; set; }
		public string dateOfNest { get; set; }
		public int dayOfWeek { get; set; }
		public string dayOfWeekString { get; set; }
		public string discipline { get; set; }
		public string endLesson { get; set; }
		public string group { get; set; }
		public int groupOid { get; set; }
		public string kindOfWork { get; set; }
		public string lecturer { get; set; }
		public int lecturerOid { get; set; }
		public string stream { get; set; }
		public int streamOid { get; set; }
		public string subGroup { get; set; }
		public int subGroupOid { get; set; }

		public ApiInteractionMember (string _auditorium, int _auditoriumOid, string _beginLesson,
			string _building, string _date, string _dateOfNest, int _dayOfWeek,
			string _dayOfWeekString, string _discipline, string _endLesson,
			string _group, int _groupOid, string _kindOfWork, string _lecturer,
			int _lecturerOid, string _stream, int _streamOid, string _subGroup, int _subGroupOid)
		{
			this.auditorium = _auditorium; this.auditoriumOid = _auditoriumOid; this.beginLesson = _beginLesson;
			this.building = _building; this.date = _date; this.dateOfNest = _dateOfNest; this.dayOfWeek = _dayOfWeek;
			this.dayOfWeekString = _dayOfWeekString; this.discipline = _discipline; this.endLesson = _endLesson;
			this.group = _group; this.groupOid = _groupOid; this.kindOfWork = _kindOfWork; this.lecturer = _lecturer;
			this.lecturerOid = _lecturerOid; this.stream = _stream; this.streamOid = _streamOid;
			this.subGroup = _subGroup; this.subGroupOid = _subGroupOid;
		}
	}
}

