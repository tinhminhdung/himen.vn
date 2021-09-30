using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class ShopInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _Address;		private string  _Tel;		private string  _Description;		private string  _SDate;		private string  _EDate;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string Name{ get { return _Name; } set { _Name = value; } }		public string Address{ get { return _Address; } set { _Address = value; } }		public string Tel{ get { return _Tel; } set { _Tel = value; } }		public string Description{ get { return _Description; } set { _Description = value; } }		public string SDate{ get { return _SDate; } set { _SDate = value; } }		public string EDate{ get { return _EDate; } set { _EDate = value; } }		#endregion			}}