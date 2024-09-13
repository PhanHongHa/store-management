using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DTO
{     
    public class Customer
    {
        private int id;
        private string name;
        private string address;
        private string phoneNumber;
        private string status;


        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string PhoneNumber { get => phoneNumber; set => phoneNumber = value; }
        public string Status { get => status; set => status = value; }

        public Customer(int id, string name, string address, string phoneNumber, string status)
        {
            this.id = id;
            this.name = name;
            this.address = address;
            this.phoneNumber = phoneNumber;
            this.status = status;
          
        }

        public Customer(DataRow row) 
        { 
            this.id = (int)row["id"];
            this.name = row["name"].ToString();
            this.address = row["address"].ToString();
            this.phoneNumber = row["phoneNumber"].ToString();
            this.status = row["status"].ToString();
        }
        
    }
}
