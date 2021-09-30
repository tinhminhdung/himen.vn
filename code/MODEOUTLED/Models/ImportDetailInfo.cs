using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class ImportDetailInfo	{		#region[Declare variables]		private string  _Id;		private string  _IdImport;		private string  _IdPro;		private string  _IdSize;		private string  _Number;		private string  _Price;		private string  _Total;		private string  _Ord;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string IdImport{ get { return _IdImport; } set { _IdImport = value; } }		public string IdPro{ get { return _IdPro; } set { _IdPro = value; } }		public string IdSize{ get { return _IdSize; } set { _IdSize = value; } }		public string Number{ get { return _Number; } set { _Number = value; } }		public string Price{ get { return _Price; } set { _Price = value; } }		public string Total{ get { return _Total; } set { _Total = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		#endregion			}}