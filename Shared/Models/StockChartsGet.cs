using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APBDBlazorApp.Shared.Models
{
    public class StockChartsGet
    {
        public int ResultsCount { get; set; }
        public List<StockCharts> StockCharts { get; set; }
    }
}