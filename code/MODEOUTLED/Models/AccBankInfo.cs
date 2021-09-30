using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class AccBankInfo	{		#region[Declare variables]		private string  _Id;		private string  _IdBank;		private string  _Account;		private string  _Number;		private string  _Description;		private string  _Ord;		private Boolean  _Active;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }		public string IdBank{ get { return _IdBank; } set { _IdBank = value; } }		public string Account{ get { return _Account; } set { _Account = value; } }		public string Number{ get { return _Number; } set { _Number = value; } }		public string Description{ get { return _Description; } set { _Description = value; } }		public string Ord{ get { return _Ord; } set { _Ord = value; } }		public Boolean Active{ get { return _Active; } set { _Active = value; } }		#endregion			}}