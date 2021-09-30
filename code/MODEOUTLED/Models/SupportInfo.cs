using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class SupportInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _Link;		private string  _Nick;		private string  _Type;		private string  _Image;		private string  _Target;		private string  _Ord;		private string  _Tel;		private string  _IdGroupSupport;		private string  _Position;		private Boolean  _Active;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string Name{ get { return _Name; } set { _Name = value; } }		public string Link{ get { return _Link; } set { _Link = value; } }		public string Nick{ get { return _Nick; } set { _Nick = value; } }		public string Type{ get { return _Type; } set { _Type = value; } }		public string Image{ get { return _Image; } set { _Image = value; } }		public string Target{ get { return _Target; } set { _Target = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		public string Tel{ get { return _Tel; } set { _Tel = value; } }		public string IdGroupSupport{ get { return _IdGroupSupport; } set { _IdGroupSupport = value; } }		public string Position{ get { return _Position; } set { _Position = value; } }		public Boolean Active{ get { return _Active; } set { _Active = value; } }		#endregion			}}