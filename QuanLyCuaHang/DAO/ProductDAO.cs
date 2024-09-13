using QuanLyCuaHang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QuanLyCuaHang.DAO
{
    public class ProductDAO
    {
        private static ProductDAO instance;

        public static ProductDAO Instance
        {
            get { if (instance == null) instance = new ProductDAO(); return ProductDAO.instance; }
            private set { ProductDAO.instance = value; }
        }

        private ProductDAO() { }

        public List<Product> GetProductByIdCategory(int id)
        {
            List<Product> listProduct = new List<Product>();
            string query = "SELECT * FROM Product WHERE idCategory =" + id;

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Product product = new Product(row);
                listProduct.Add(product);
            }

            return listProduct;
        }

        public List<Product> GetListProduct()
        {
            List<Product> list = new List<Product>();
            string query = "SELECT * FROM Product ";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Product product = new Product(row);
                list.Add(product);
            }

            return list;
        }


        public bool InsertProduct(string name, int idCategory, float price)
        {
            string query = string.Format("INSERT dbo.Product ( name, Idcategory , price ) VALUES ( N'{0}', {1}, {2} )", name , idCategory , price);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public bool UpdateProduct( string name, int idCategory, float price, int id)
        {
            string query = string.Format("UPDATE dbo.Product SET name = N'{0}' , idCategory = {1} , price = {2} WHERE id = {3}", name, idCategory, price, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteProduct(int id)
        {

            BillInfoDAO.Instance.DeleteBillInfoByIdProduct(id);
            string query = string.Format("DELETE dbo.Product WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public List<Product> SearchProductByName(string name)
        {

            List<Product> listProduct = new List<Product>();
            string query = string.Format("SELECT * FROM dbo.Product WHERE dbo.fuConvertToUnsign1(name) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%' ", name);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Product product = new Product(row);
                listProduct.Add(product);
            }

            return listProduct;
        }
    }
}
