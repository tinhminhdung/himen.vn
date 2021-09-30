using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class CustomerInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _Email;		private string  _Password;		private string  _Tel;		private string  _Address;		private string  _SDate;		private string  _Status;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string Name{ get { return _Name; } set { _Name = value; } }		public string Email{ get { return _Email; } set { _Email = value; } }		public string Password{ get { return _Password; } set { _Password = value; } }		public string Tel{ get { return _Tel; } set { _Tel = value; } }		public string Address{ get { return _Address; } set { _Address = value; } }		public string SDate{ get { return _SDate; } set { _SDate = value; } }		public string Status{ get { return _Status; } set { _Status = value; } }		#endregion			}}