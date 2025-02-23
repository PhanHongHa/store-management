﻿using QuanLyCuaHang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DAO
{
    public class BillInfoDAO
    {

        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        public BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();

            DataTable data = DataProvider.Instance.ExecuteQuery("SELECT * FROM dbo.BillInfo WHERE idBill =" + id);

            foreach (DataRow item in data.Rows)
            {
                BillInfo billInfo = new BillInfo(item);
                listBillInfo.Add(billInfo);
            }
            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idProduct, int count)
        {
            DataProvider.Instance.ExecuteNonQuery("EXEC USP_InsertBillInfo @idBill , @idProduct , @count", new object[] { idBill, idProduct, count });
        }

        public void DeleteBillInfoByIdProduct(int id)
        {
            DataProvider.Instance.ExecuteQuery("DELETE dbo.BillInfo Where idProduct = " + id);
        }
    }
}
