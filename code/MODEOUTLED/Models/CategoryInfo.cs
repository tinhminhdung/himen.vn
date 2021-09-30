using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MODEOUTLED.Models
{
    public class CategoryInfo	{		#region[Declare variables]		private string  _Id;		private string  _Name;		private string  _Ord;		private string  _Title;		private string  _Description;		private string  _Keyword;		private string  _Tag;		private string  _Level;		private string  _Index;		private string  _Priority;		private Boolean  _Active;		#endregion		#region[Public Properties]		public string Id{ get { return _Id; } set { _Id = value; } }        [Required(ErrorMessage="Phải nhập tên loại sản phẩm")]		public string Name{ get { return _Name; } set { _Name = value; } }
        [Required(ErrorMessage = "Required!")]		public string Ord{ get { return _Ord; } set { _Ord = value; } }
        [Required(ErrorMessage = "Required!")]		public string Title{ get { return _Title; } set { _Title = value; } }
        [Required(ErrorMessage = "Required!")]		public string Description{ get { return _Description; } set { _Description = value; } }		public string Keyword{ get { return _Keyword; } set { _Keyword = value; } }		public string Tag{ get { return _Tag; } set { _Tag = value; } }		public string Level{ get { return _Level; } set { _Level = value; } }		public string Index{ get { return _Index; } set { _Index = value; } }		public string Priority{ get { return _Priority; } set { _Priority = value; } }		public Boolean Active{ get { return _Active; } set { _Active = value; } }		#endregion			}}