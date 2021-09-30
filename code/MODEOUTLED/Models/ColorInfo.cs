using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class ColorInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _Image;		private string  _Description;		private string  _Ord;		private Boolean  _Active;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string Name{ get { return _Name; } set { _Name = value; } }		public string Image{ get { return _Image; } set { _Image = value; } }		public string Description{ get { return _Description; } set { _Description = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		public Boolean Active{ get { return _Active; } set { _Active = value; } }		#endregion			}}