using QuanLyCuaHang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DAO
{
    public class CustomerDAO
    {
        public static int TableWidth = 440;
        public static int TableHeight = 85;

        private static CustomerDAO instance;

        public static CustomerDAO Instance {
            get { if (instance == null) instance = new CustomerDAO(); return CustomerDAO.instance; }
            private set { CustomerDAO.instance = value; }
        }

        private CustomerDAO() { }

        public List<Customer> LoadCustomerList()
        {
            List<Customer> customerList = new List<Customer>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetCustomerList");

            foreach (DataRow item in data.Rows)
            {
                Customer customer = new Customer(item);
                customerList.Add(customer);
            }

            return customerList;
        }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });

        }

        public bool InsertCustomer(string name, string address, string phoneNumber)
        {
            string query = string.Format("INSERT dbo.Customers (name, address, phoneNumber) VALUES ( N'{0}', N'{1}', N'{2}' )", name, address, phoneNumber);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public bool UpdateCustomer(string name, int id, string address, string phoneNumber)
        {
            string query = string.Format("UPDATE dbo.Customers SET name = N'{0}', address = N'{2}' , phoneNumber = N'{3}' WHERE id = {1}", name, id, address, phoneNumber);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteCustomer(int id)
        {

            string query = string.Format("DELETE dbo.Customers WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public List<Customer> SearchTableByStatus(string status)
        {

            List<Customer> listTable = new List<Customer>();
            string query = string.Format("SELECT * FROM dbo.TableFood  WHERE dbo.fuConvertToUnsign1(status) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%' ", status);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Customer table = new Customer(row);
                listTable.Add(table);
            }

            return listTable;
        }


        public List<Customer> SearchTableByPhoneNumber(string phoneNumber)
        {

            List<Customer> listCustomer = new List<Customer>();
            string query = string.Format("SELECT * FROM dbo.Customers WHERE phoneNumber = N'{0}' ", phoneNumber);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Customer customer = new Customer(row);
                listCustomer.Add(customer);
            }

            return listCustomer;
        }
    }
}
