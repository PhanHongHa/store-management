using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DTO
{
    public class BillInfo
    {
        private int id;
        private int idCustomer;
        private int idBill;
        private int count;

        public int Id { get => id; set => id = value; }
        public int IdCustomer { get => idCustomer; set => idCustomer = value; }
        public int IdBill { get => idBill; set => idBill = value; }
        public int Count { get => count; set => count = value; }

        public BillInfo(int id, int idCustomer, int idBill, int count) 
        {
            this.Id = id;
            this.IdCustomer = idCustomer  ;
            this.IdBill = idBill;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.Id = (int)row["id"];
            this.IdBill = (int)row["idBill"];
            this.IdCustomer = (int)row["idCustomer"];
            this.Count = (int)row["count"];
        }
    }
}
