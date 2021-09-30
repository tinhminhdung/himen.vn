using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using onsoft.Models;
namespace onsoft.Controllers
{
    public class BaocaoController : Controller
    {
        //
        // GET: /Baocao/
        wwwEntities data = new wwwEntities();
        #region [GroupProduct]
        public ActionResult FromdaytoDay()
        {
          
            return View();
        }
        [HttpPost]      
        public ActionResult FromdaytoDay( FormCollection fc)
        {
            string mang = "";
            string datefrom = fc["txtDate"];
            string dateto = fc["txtDateto"];
            ViewBag.Date = datefrom;
            ViewBag.Dateto = dateto;
            if (datefrom == "")
            {
                var list = data.sp_GroupProduct_Baocao(DateTime.Parse("1800/1/1"), DateTime.Parse(dateto.ToString())).Take(10).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    mang += "['" + list[i].GroupName + "'," + list[i].Soluongmua + "],";
                }
                string chuoi = "";
                chuoi += "<script type=\" text />javascript\" src=\"http://www.google.com/jsapi\"></script>";
                chuoi += "<script>";
                chuoi += "    google.load('visualization', '1', { 'packages': ['columnchart'] });";
                chuoi += "    google.setOnLoadCallback(createChart);";
                //callback function
                chuoi += "    function createChart() {";
                //create data table object
                chuoi += "        var dataTable = new google.visualization.DataTable();";
                //define columns
                chuoi += "        dataTable.addColumn('string', 'VietCoding Visits');";
                chuoi += "        dataTable.addColumn('number', 'Số lượng mua');";
                //define rows of data
                chuoi += "        dataTable.addRows([" + mang + "]);";
                //instantiate our chart object
                chuoi += "        var chart = new google.visualization.ColumnChart(document.getElementById('chart'));";
                //define options for visualization
                chuoi += "        var options = { width: "+(100*list.Count)+", height: 540, is3D: true, title: 'Thống kê group product' };";
                //draw our chart
                chuoi += "        chart.draw(dataTable, options);";
                chuoi += "    }";
                chuoi += "    createChart();";
                chuoi += "</script>";
                ViewBag.script = chuoi;
                return View(list);
            }
            else {
                var list = data.sp_GroupProduct_Baocao(DateTime.Parse(datefrom.ToString()), DateTime.Parse(dateto.ToString())).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    mang += "['" + list[i].GroupName + "'," + list[i].Soluongmua + "],";
                }
                string chuoi = "";
                chuoi += "<script type=\" text />javascript\" src=\"http://www.google.com/jsapi\"></script>";
                chuoi += "<script>";
                chuoi += "    google.load('visualization', '1', { 'packages': ['columnchart'] });";
                chuoi += "    google.setOnLoadCallback(createChart);";
                //callback function
                chuoi += "    function createChart() {";
                //create data table object
                chuoi += "        var dataTable = new google.visualization.DataTable();";
                //define columns
                chuoi += "        dataTable.addColumn('string', 'VietCoding Visits');";
                chuoi += "        dataTable.addColumn('number', 'Số lượng mua');";
                //define rows of data
                chuoi += "        dataTable.addRows([" + mang + "]);";
                //instantiate our chart object
                chuoi += "        var chart = new google.visualization.ColumnChart(document.getElementById('chart'));";
                //define options for visualization
                chuoi += "        var options = { width: "+(140*list.Count)+", height: 540, is3D: true, title: 'Thống kê group product' };";
                //draw our chart
                chuoi += "        chart.draw(dataTable, options);";
                chuoi += "    }";
                chuoi += "    createChart();";
                chuoi += "</script>";
                ViewBag.script = chuoi;

                return View(list);
            }

            
        }

        #endregion


        #region [Product]
        public ActionResult Fordaytoday_Product()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Fordaytoday_Product(FormCollection fc)
        {
               string mang ="";
            string datefrom = fc["txtDate"];
            string dateto = fc["txtDateto"];
            ViewBag.Date = datefrom;
            ViewBag.Dateto = dateto;
            if (datefrom == "")
            {
                var list = data.sp_Product_Baocao(DateTime.Parse("1800/1/1"), DateTime.Parse(dateto.ToString())).ToList();
            
                


                return View(list);
            }
            else
            {
                var list = data.sp_Product_Baocao(DateTime.Parse(datefrom.ToString()), DateTime.Parse(dateto.ToString())).ToList();
               
                return View(list);
            }
           
        }
        public ActionResult PartialView(int? id)
        {
            var list = data.v_ProductForMonth.ToList();
            string mang = "";
            if (id == null)
            { list = data.v_ProductForMonth.Where(m => m.Id == 0).ToList(); }
            else
            {
                list = data.v_ProductForMonth.Where(m => m.Id == id).ToList();
                for (int i = 1; i < 13; i++)
                {
                    switch (i)
                    {
                        case 1: mang += "['" +"Jan"+ "'," + list[0].Jan + "],"; break;
                        case 2: mang += "['" + "Feb" + "'," + list[0].Feb + "],"; break;
                        case 3: mang += "['" + "Mar" + "'," + list[0].Mar + "],"; break;
                        case 4: mang += "['" + "Apr" + "'," + list[0].Apr + "],"; break;
                        case 5: mang += "['" + "May" + "'," + list[0].May + "],"; break;
                        case 6: mang += "['" + "Jun" + "'," + list[0].Jun + "],"; break;
                        case 7: mang += "['" + "Jul" + "'," + list[0].Jul + "],"; break;
                        case 8: mang += "['" + "Aug" + "'," + list[0].Aug + "],"; break;
                        case 9: mang += "['" + "Sep" + "'," + list[0].Sep + "],"; break;
                        case 10: mang += "['" + "Oct" + "'," + list[0].Oct + "],"; break;
                        case 11: mang += "['" + "Nov" + "'," + list[0].Nov + "],"; break;
                        case 12: mang += "['" + "Dec" + "'," + list[0].Dec + "],"; break;

                    }

                }
            }
            string chuoi = "";
            chuoi += "<script type=\" text />javascript\" src=\"http://www.google.com/jsapi\"></script>";
            chuoi += "<script>";
            chuoi += "    google.load('visualization', '1', { 'packages': ['columnchart'] });";
            chuoi += "    google.setOnLoadCallback(createChart);";
            //callback function
            chuoi += "    function createChart() {";
            //create data table object
            chuoi += "        var dataTable = new google.visualization.DataTable();";
            //define columns
            chuoi += "        dataTable.addColumn('string', 'VietCoding Visits');";
            chuoi += "        dataTable.addColumn('number', 'tháng');";
            //define rows of data
            chuoi += "        dataTable.addRows([" + mang + "]);";
            //instantiate our chart object
            chuoi += "        var chart = new google.visualization.ColumnChart(document.getElementById('chart'));";
            //define options for visualization
            chuoi += "        var options = { width: "+(100*12)+", height: 540, is3D: true, title: 'Thống kê product' };";
            //draw our chart
            chuoi += "        chart.draw(dataTable, options);";
            chuoi += "    }";
            chuoi += "    createChart();";
            chuoi += "</script>";
            ViewBag.script = chuoi;

            return PartialView(list);
        }
        #endregion






    }
}
