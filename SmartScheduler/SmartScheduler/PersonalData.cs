using System;

namespace SmartScheduler
{
	public class PersonalData
	{
		private string name;
		private string second_name;
		private string middle_name;
		private string group;

		public PersonalData (string _name, string _second_name, string _middle_name, string _group)
		{
			this.name = _name;
			this.second_name = _second_name;
			this.middle_name = _middle_name;
			this.group = _group;
		}

		public string GetName()
		{
			return this.name;
		}

		public string GetSecondName()
		{
			return this.second_name;
		}

		public string GetMiddleName()
		{
			return this.middle_name;
		}

		public string GetGroup()
		{
			return this.group;
		}

		public void SetName(string _name)
		{
			this.name = _name;
		}

		public void SetSecondName(string _second_name)
		{
			this.second_name = _second_name;
		}

		public void SetMiddleName(string _middle_name)
		{
			this.middle_name = _middle_name;
		}

		public void SetGroup(string _group)
		{
			this.group = _group;
		}
	}
}

