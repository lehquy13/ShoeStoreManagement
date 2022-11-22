using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ChartJSCore.Models;
using ChartJSCore.Helpers;
using ShoeStoreManagement.CRUD.Interfaces;
using Microsoft.AspNetCore.Identity;
using ShoeStoreManagement.Areas.Identity.Data;
using ShoeStoreManagement.CRUD.Implementations;
using ShoeStoreManagement.Core.Models;

namespace ShoeStoreManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
	{
        private int startYear = 2010;
        private readonly ILogger<UserController> _logger;
        private string _selectedMonth = "";
        private string _selectedYear = "";
        private string _selectedTime = "";
        private string _selectedType = "";
        private readonly IOrderCRUD _orderCRUD;

        private List<Order>? orders;

        public ReportController(ILogger<UserController> logger, IOrderCRUD orderCRUD)
        {
            _logger = logger;
            _orderCRUD = orderCRUD;
        }

        public IActionResult Index(string selectedMonth = "", string selectedYear = "", string selectedTime = "Yearly", string selectedType = "Order")
		{
            Init(selectedMonth, selectedYear, selectedTime, selectedType);

            ViewBag.Report = true;
            Chart verticalBarChart = GenerateVerticalBarChart();

            List<bool> disableSelections = new List<bool>();
            if (selectedTime.Equals("Yearly"))
            {
                disableSelections.Add(true);
                disableSelections.Add(true);
            }
            else if (selectedTime.Equals("Monthly"))
            {
                disableSelections.Add(true);
                disableSelections.Add(false);
            }
            else if (selectedTime.Equals("Daily"))
            {
                disableSelections.Add(false);
                disableSelections.Add(false);
            }

            ViewData["disableSelections"] = disableSelections;
            ViewData["VerticalBarChart"] = verticalBarChart;
            ViewData["Selections"] = new List<string> { selectedMonth, selectedYear, selectedTime, selectedType };
            return View();

        }

        private void Init(string selectedMonth, string selectedYear, string selectedTime, string selectedType)
        {
            _selectedMonth = selectedMonth;
            _selectedYear = selectedYear;
            _selectedTime = selectedTime;
            _selectedType = selectedType;
        }

        private Chart GenerateVerticalBarChart()
        {
            Chart chart = new Chart();
            chart.Type = Enums.ChartType.Bar;

            ChartJSCore.Models.Data data = new ChartJSCore.Models.Data();

            int currentYear = DateTime.Now.Year;

            data.Labels = new List<string>();

            orders = _orderCRUD.GetAllOrderAsync().Result;
            List<double?> dataValues = new List<double?>();
            List<ChartColor> colors = new List<ChartColor>();
            List<ChartColor> borderColors = new List<ChartColor>();

            int index = 0;

            if (_selectedTime.Equals("Yearly"))
            {
                for (int i = startYear; i <= currentYear; i++)
                {
                    data.Labels.Add(i.ToString());
                    dataValues.Add(0);
                    colors.Add(ChartColor.FromRgba(255, 99, 132, 0.2));
                    borderColors.Add(ChartColor.FromRgb(255, 99, 132));

                    foreach (Order order in orders)
                    {
                        if (order.OrderDate.Year == i)
                        {
                            if (_selectedType.Equals("Order"))
                                dataValues[index]++;
                            else
                                dataValues[index] += order.OrderTotalPayment;
                        }
                    }

                    index++;
                }
            }
            else if (_selectedTime.Equals("Monthly") && !string.IsNullOrEmpty(_selectedYear))
            {
                for (int i = 1; i <= 12; i++)
                {
                    data.Labels.Add(i.ToString());
                    dataValues.Add(0);
                    colors.Add(ChartColor.FromRgba(255, 99, 132, 0.2));
                    borderColors.Add(ChartColor.FromRgb(255, 99, 132));

                    foreach (Order order in orders)
                    {
                        if (order.OrderDate.Year == int.Parse(_selectedYear) && order.OrderDate.Month == i)
                        {
                            if (_selectedType.Equals("Order"))
                                dataValues[index]++;
                            else
                                dataValues[index] += order.OrderTotalPayment;
                        }
                    }

                    index++;
                }
            }
            else if (_selectedTime.Equals("Daily") && !string.IsNullOrEmpty(_selectedYear) && !string.IsNullOrEmpty(_selectedMonth))
            {
                for (int i = 1; i <= 31; i++)
                {
                    data.Labels.Add(i.ToString());
                    dataValues.Add(0);
                    colors.Add(ChartColor.FromRgba(255, 99, 132, 0.2));
                    borderColors.Add(ChartColor.FromRgb(255, 99, 132));

                    foreach (Order order in orders)
                    {
                        if (order.OrderDate.Year == int.Parse(_selectedYear) && order.OrderDate.Day == i && order.OrderDate.Month.ToString().Equals(_selectedMonth))
                        {
                            if (_selectedType.Equals("Order"))
                                dataValues[index]++;
                            else
                                dataValues[index] += order.OrderTotalPayment;
                        }
                    }

                    index++;
                }
            }

            var dataset = new BarDataset
            {
                Label = "# of Votes",
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
