using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DTO
{
     public class Menu
    {
        private string productName;
        private int count;
        
        private float price;
        private float totalPrice;

        public float TotalPrice { get => totalPrice; set => totalPrice = value; }
        public float Price { get => price; set => price = value; }
        public int Count { get => count; set => count = value; }
        public string ProductName { get => productName; set => productName = value; }
  
        public Menu(string productName, string size, int count, float price, float totalPrice=0)
        {
            this.productName = productName;
            
            this.Count = count;
                
            this.Price = price;
            this.TotalPrice = totalPrice;
        }

        public Menu(DataRow row)
        {
            this.ProductName = row["name"].ToString();
            
            this.Count = (int)row["count"];

            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.TotalPrice = (float)Convert.ToDouble(row["totalPrice"].ToString());
        }
    }
}
