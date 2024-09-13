using QuanLyCuaHang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyCuaHang.DAO
{
    public class MenuDAO
    {

        private static MenuDAO instance;

        public static MenuDAO Instance
        {
            get { if (instance == null) instance = new MenuDAO(); return MenuDAO.instance; }
            private set { MenuDAO.instance = value; }
        }

        public MenuDAO() { }

        public List<Menu> GetListMenuByCustomer(int id) 
        {
            List<Menu> listMenu = new List<Menu>();
            string query = "SELECT p.name, bi.count, p.price, p.price*bi.count AS totalPrice FROM BillInfo AS bi, Bill AS b, Product AS p WHERE bi.idBill = b.id AND bi.idProduct = p.id AND b.status = 0 AND b.idCustomer =" + id;
        
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Menu menu = new Menu(item);
                listMenu.Add(menu);
            }


            return listMenu;
        }
    }
}
