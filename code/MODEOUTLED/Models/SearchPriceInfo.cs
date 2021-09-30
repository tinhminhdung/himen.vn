using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class SearchPriceInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _PriceFrom;		private string  _PriceTo;		private string  _Ord;		private Boolean  _Active;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string Name{ get { return _Name; } set { _Name = value; } }		public string PriceFrom{ get { return _PriceFrom; } set { _PriceFrom = value; } }		public string PriceTo{ get { return _PriceTo; } set { _PriceTo = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		public Boolean Active{ get { return _Active; } set { _Active = value; } }		#endregion			}}