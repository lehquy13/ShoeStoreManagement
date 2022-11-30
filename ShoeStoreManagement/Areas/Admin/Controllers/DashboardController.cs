using ChartJSCore.Helpers;
using ChartJSCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.Core.Models;
using ShoeStoreManagement.CRUD.Interfaces;
using ShoeStoreManagement.Data;
using System.Data;
using System.Linq;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
	{
        private List<string> roleList = new List<string>() { "Customer", "Admin" };
        private readonly ILogger<UserController> _logger;
        private readonly IOrderCRUD _orderCRUD;
        private readonly IProductCRUD _productCRUD;
        private readonly IApplicationUserCRUD _applicationUserCRUD;
        private readonly UserManager<ApplicationUser> _usermanager; 
        
        private List<Order>? orders;

        public DashboardController(ILogger<UserController> logger, IOrderCRUD orderCRUD, IProductCRUD productCRUD, IApplicationUserCRUD applicationUserCRUD, UserManager<ApplicationUser> userManager) 
        {
            _logger = logger;
            _orderCRUD = orderCRUD;
            _productCRUD = productCRUD;
            _applicationUserCRUD = applicationUserCRUD;
            _usermanager = userManager;
        }

        
		public IActionResult Index()
		{
            ViewBag.Home = true;
            Chart verticalBarChart = GenerateVerticalBarChart();
            Chart verticalBarChart2 = GenerateVerticalBarChart2();

            // Handles the number of shoes
            string shoesNumber = _productCRUD.GetAllAsync().Result.Count.ToString();
            int customerNumber = 0;
            int staffNumber = 0;

            // Handles the numbers of cus and staff
            List<ApplicationUser> users = _applicationUserCRUD.GetAllAsync().Result.ToList();
            foreach (ApplicationUser user in users)
            {
                if (_usermanager.GetRolesAsync(user).Result.ToList()[0].Equals(roleList[0]))
                    customerNumber++;
                else if (_usermanager.GetRolesAsync(user).Result.ToList()[0].Equals(roleList[1]))
                    staffNumber++;
            }

            float totalSum = 0;
            // Handles total profit
            List<Order> orders = _orderCRUD.GetAllOrderAsync().Result.ToList();
            foreach(Order order in orders)
            {
                totalSum += order.OrderTotalPayment;
            }

            ViewData["VerticalBarChart"] = verticalBarChart;
            ViewData["VerticalBarChart2"] = verticalBarChart2;
            ViewData["dataList"] = new List<string>() { shoesNumber, customerNumber.ToString(), staffNumber.ToString(), totalSum.ToString() };
            return View();
		}
        private Chart GenerateVerticalBarChart()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            data.Labels = new List<string>();

            orders = _orderCRUD.GetAllOrderAsync().Result;
            List<double?> dataValues = new List<double?>();
            List<ChartColor> colors = new List<ChartColor>();
            List<ChartColor> borderColors = new List<ChartColor>();

            int index = 0;

            for (int i = 1; i <= 31; i++)
            {
                data.Labels.Add(i.ToString());
                dataValues.Add(0);
                colors.Add(ChartColor.FromRgba(255, 99, 132, 0.2));
                borderColors.Add(ChartColor.FromRgb(255, 99, 132));

                foreach (Order order in orders)
                {
                    if (order.OrderDate.Year == currentYear && order.OrderDate.Month == currentMonth && order.OrderDate.Day == i)
                        dataValues[index]++;
                }

                index++;
            }
            

            var dataset = new BarDataset
            {
                Label = "# numbers of order",
                Data = dataValues,
                BackgroundColor = colors,
                BorderColor = borderColors,
                BorderWidth = new List<int> { 1 },
                BarPercentage = 0.5,
                BarThickness = 6,
                MaxBarThickness = 8,
                MinBarLength = 2
            };

            data.Datasets = new List<Dataset> { dataset };

            chart.Data = data;

            var options = new Options
            {
                Scales = new Dictionary<string, Scale>()
            };

            var scales = new Dictionary<string, Scale>
            {
                {
                    "x", new Scale
                    {
                        Grid = new Grid()
                        {
                            Offset = true
                        }
                    }
                },
                {
                    "y", new CartesianLinearScale
                    {
                        BeginAtZero = true
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }

        private Chart GenerateVerticalBarChart2()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            data.Labels = new List<string>();

            orders = _orderCRUD.GetAllOrderAsync().Result;
            List<double?> dataValues = new List<double?>();
            List<ChartColor> colors = new List<ChartColor>();
            List<ChartColor> borderColors = new List<ChartColor>();

            int index = 0;

            for (int i = 1; i <= 31; i++)
            {
                data.Labels.Add(i.ToString());
                dataValues.Add(0);
                colors.Add(ChartColor.FromRgba(255, 99, 132, 0.2));
                borderColors.Add(ChartColor.FromRgb(255, 99, 132));

                foreach (Order order in orders)
                {
                    if (order.OrderDate.Year == currentYear && order.OrderDate.Month == currentMonth && order.OrderDate.Day == i)
                        dataValues[index] += order.OrderTotalPayment;
                }

                index++;
            }


            var dataset = new BarDataset
            {
                Label = "# profit",
                Data = dataValues,
                BackgroundColor = colors,
                BorderColor = borderColors,
                BorderWidth = new List<int> { 1 },
                BarPercentage = 0.5,
                BarThickness = 6,
                MaxBarThickness = 8,
                MinBarLength = 2
            };

            data.Datasets = new List<Dataset> { dataset };

            chart.Data = data;

            var options = new Options
            {
                Scales = new Dictionary<string, Scale>()
            };

            var scales = new Dictionary<string, Scale>
            {
                {
                    "x", new Scale
                    {
                        Grid = new Grid()
                        {
                            Offset = true
                        }
                    }
                },
                {
                    "y", new CartesianLinearScale
                    {
                        BeginAtZero = true
                    }
                }
            };

            options.Scales = scales;

            chart.Options = options;

            chart.Options.Layout = new Layout
            {
                Padding = new Padding
                {
                    PaddingObject = new PaddingObject
                    {
                        Left = 10,
                        Right = 12
                    }
                }
            };

            return chart;
        }
    }
}
