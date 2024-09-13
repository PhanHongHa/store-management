using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DTO
{
    public class Product
    {

        private int id;
        private string name;
        private int idCategory;
        private float price;

        public float Price { get => price; set => price = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        

        public Product(int id, string name, int idProductCategory, float price) 
        {
            this.Id = id;
            this.Name = name;
           
            this.Price = price;
            this.IdCategory = idProductCategory;

        }

        public Product(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name" ].ToString();
         
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.IdCategory =(int)row["idCategory"];

        }


    }
}
