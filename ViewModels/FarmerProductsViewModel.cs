using AgriEnergyConnect1.Models;
using System.Collections.Generic;

namespace AgriEnergyConnect1.ViewModels
{
    public class FarmerProductsViewModel
    {
        public Farmer Farmer { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}