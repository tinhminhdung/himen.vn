using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class ProvinceInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _Level;		private string  _Price;		private string  _Ord;		private Boolean  _Active;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string Name{ get { return _Name; } set { _Name = value; } }		public string Level{ get { return _Level; } set { _Level = value; } }		public string Price{ get { return _Price; } set { _Price = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		public Boolean Active{ get { return _Active; } set { _Active = value; } }		#endregion			}}