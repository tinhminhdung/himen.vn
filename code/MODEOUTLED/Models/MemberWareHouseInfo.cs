using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class MemberWareHouseInfo	{		#region[Declare variables]		private string  _Id;		private string  _IdMember;		private string  _IdWareHouse;		private string  _SDate;		private string  _EDate;		private string  _Ord;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string IdMember{ get { return _IdMember; } set { _IdMember = value; } }		public string IdWareHouse{ get { return _IdWareHouse; } set { _IdWareHouse = value; } }		public string SDate{ get { return _SDate; } set { _SDate = value; } }		public string EDate{ get { return _EDate; } set { _EDate = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		#endregion			}}