using QuanLyCuaHang.DAO;
using QuanLyCuaHang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Menu = QuanLyCuaHang.DTO.Menu;

namespace QuanLyCuaHang
{
    public partial class fManagerStore : Form
    {

        BindingSource customerList = new BindingSource();


        private Account loginAccount;

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }
        }

       
        public fManagerStore(Account account)
        {
         

            InitializeComponent();

            this.LoginAccount = account;

            LoadCategory();

            dtgvCustomer.DataSource = customerList;
            LoadListCustomer();
            AddCustomerBinding();
        }

        #region Method
        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            infoToolStripMenuItem.Text += " (" + LoginAccount.DisplayName + ")";
        }

        //void LoadCustomer()
        //{
        //    customerList.DataSource = CustomerDAO.Instance.LoadCustomerList();

            

        //    flpCustomer.Controls.Clear();

        //    foreach (Customer item in customerList)
        //    {
        //        Button btn = new Button() { Width = CustomerDAO.TableWidth, Height = CustomerDAO.TableHeight };
        //        btn.Text = item.Name + Environment.NewLine + item.PhoneNumber + Environment.NewLine + item.Address + Environment.NewLine + item.Status;
        //        btn.Click += btn_Click;
        //        btn.Tag = item;

        //        switch (item.Status)
        //        {
        //            case "Chưa thanh toán":
        //                btn.BackColor = Color.LightPink;
        //                break;
        //            default:
        //                btn.BackColor = Color.Azure;
        //                break;
        //        }

        //        flpCustomer.Controls.Add(btn);
        //    }
        //}


        void LoadListCustomer()
        {
            customerList.DataSource = CustomerDAO.Instance.LoadCustomerList();
            dtgvCustomer.Columns["Name"].HeaderText = "Tên bàn";
            dtgvCustomer.Columns["Status"].HeaderText = "Trạng thái";
            dtgvCustomer.Columns["PhoneNumber"].HeaderText = "Số điện thoại";
            dtgvCustomer.Columns["Address"].HeaderText = "Địa chỉ";


            
        }

        void AddCustomerBinding()
        {
            txbName.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbId.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbStatus.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Status", true, DataSourceUpdateMode.Never));
            txbAddress.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Address", true, DataSourceUpdateMode.Never));
            txbPhoneNumber.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "PhoneNumber", true, DataSourceUpdateMode.Never));

          

        }

        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory();
            cbCategory.DataSource = listCategory;
            cbCategory.DisplayMember = "Name";
        }

        void LoadProductListByCategoryID(int id)
        {
            List<Product> listProduct = ProductDAO.Instance.GetProductByIdCategory(id);
            cbProductName.DataSource = listProduct;
            cbProductName.DisplayMember = "Name";
        }

        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByCustomer(id);
            float totalPrice = 0;

            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.ProductName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());
                totalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }

            CultureInfo culture = new CultureInfo("vi-VN");

            //Thread.CurrentThread.CurrentCulture = culture;

            txbTotalPrice.Text = totalPrice.ToString("c", culture);

           
        }

        List<Customer> SearchCustomerByPhoneNumber(string phoneNumber)
        {

            List<Customer> listCustomer = CustomerDAO.Instance.SearchTableByPhoneNumber(phoneNumber);

            return listCustomer;

        }


        private void PrintBill()
        {
            prdBill.Document = pdcBill;
            prdBill.ShowDialog();
        }

        private void pdcBill_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            var nameStore = "KHOI'S Beauty Center";
            var sloganStore = "Spa - nail - makeup - massage - set brow";
            var fdStore = "By TâmMinh";
            var addressStore = "Long Hòa, DT, BD";
            var phoneStore = "0967 180 997";

            var idBill = "1122";

            var w = pdcBill.DefaultPageSettings.PaperSize.Width;


            string nameCustomer = txbName.Text;
            string phoneCustomer = txbPhoneNumber.Text;
            string addressCustomer = txbAddress.Text;

            string totalPrice = txbTotalPrice.Text;

            string nameProduct = (lsvBill.Tag as Menu).ProductName.ToString();

            #region header
            e.Graphics.DrawString(
                    nameStore.Normalize(), 
                    new Font("Courier New", 16, FontStyle.Bold),
                    Brushes.Black, new Point(110, 20)
                    );

            e.Graphics.DrawString(
                   String.Format("HD{0}", idBill),
                   new Font("Courier New", 12, FontStyle.Bold),
                   Brushes.Black, new Point(w / 2  + 200, 20)
                   );

           

            e.Graphics.DrawString(
                  String.Format("{0}", sloganStore),
                  new Font("Courier New", 10, FontStyle.Bold),
                  Brushes.Black, new Point(75, 40)
                  );

            e.Graphics.DrawString(
                  String.Format("{0}", fdStore),
                  new Font("Courier New", 13, FontStyle.Bold),
                  Brushes.Black, new Point(200, 60)
                  );

            e.Graphics.DrawString(
                  String.Format("{0} - {1}", addressStore, phoneStore),
                  new Font("Courier New", 12, FontStyle.Bold),
                  Brushes.Black, new Point(110, 85)
                  );

            e.Graphics.DrawString(
                   String.Format("{0}", DateTime.Now.ToString("dd/MM/yyyy HH:mm")), 
                   new Font("Courier New", 12, FontStyle.Bold),
                   Brushes.Black, new Point(w / 2 + 200, 45)
                   );

            // Đường vẽ ngang
            Pen blackPen = new Pen(Color.Black, 1);

            var y = 110;

            Point p1 = new Point(10, y);
            Point p2 = new Point(w-10, y);

            e.Graphics.DrawLine( blackPen, p1, p2 );

            //Thông tin khách hàng
            e.Graphics.DrawString(
                 String.Format("Tên khách hàng: |{0}", nameCustomer),
                 new Font("Courier New", 12, FontStyle.Bold),
                 Brushes.Black, new Point(75, 125)
                 );

            e.Graphics.DrawString(
                  String.Format("Số điện thoại:  |{0}", phoneCustomer),
                  new Font("Courier New", 12, FontStyle.Bold),
                  Brushes.Black, new Point(75, 150)
                  );

            e.Graphics.DrawString(
                  String.Format("Địa chỉ:        |{0}", addressCustomer),
                  new Font("Courier New", 12, FontStyle.Bold),
                  Brushes.Black, new Point(75, 175)
                  );


            // Đường vẽ ngang
           
            var y2 = 205;

            Point p3 = new Point(10, y2);
            Point p4 = new Point(w - 10, y2);

            e.Graphics.DrawLine(blackPen, p3, p4);

            #endregion

            #region Body

            e.Graphics.DrawString(
                 String.Format("Tên sản phẩm: {0}", nameProduct ),
                 new Font("Courier New", 16, FontStyle.Bold),
                 Brushes.Black, new Point(75, 230)
                 );

            #endregion

            #region Footer

            // Đường vẽ ngang

            var y3 = 800;

            Point p5 = new Point(10, y3);
            Point p6 = new Point(w - 10, y3);

            e.Graphics.DrawLine(blackPen, p5, p6);


            e.Graphics.DrawString(
                 String.Format("Tổng tiền: {0}", totalPrice),
                 new Font("Courier New", 16, FontStyle.Bold),
                 Brushes.Black, new Point(w / 2 + 50, 825)
                 );
            #endregion

        }

        #endregion


        #region Event

        //void btn_Click(object sender, EventArgs e)
        //{
        //    int customerID = ((sender as Button).Tag as Customer).Id;
        //    lsvBill.Tag = (sender as Button).Tag;
        //    ShowBill(customerID);
        //}
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin fAdmin = new fAdmin();
            fAdmin.loginAccount = LoginAccount;
            fAdmin.InsertProduct += f_InsertProduct;
            fAdmin.DeleteProduct += f_DeleteProduct;
            fAdmin.UpdateProduct += f_UpdateProduct;
            fAdmin.ShowDialog();
        }

        private void infomationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fInfomation f = new fInfomation(LoginAccount);

            f.UpdateAccount += f_UpdateAccount;
          
            f.ShowDialog();
        }


        void f_UpdateProduct(object sender, EventArgs e)
        {
            LoadProductListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Customer).Id);
        }

        void f_DeleteProduct(object sender, EventArgs e)
        {
            LoadProductListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Customer).Id);
            LoadListCustomer();
        }

        void f_InsertProduct(object sender, EventArgs e)
        {
            LoadProductListByCategoryID((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Customer).Id);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.Id;

            LoadProductListByCategoryID(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            

            int id = Convert.ToInt32(txbId.Text);

            if (id == null)
            {
                MessageBox.Show("Hãy chọn khách hàng");
                return;
            }

            int idBill = BillDAO.Instance.GetUnCheckBillIDByCustomerID(id);
            int productID = (cbProductName.SelectedItem as Product).Id;
            int count = (int)nmCount.Value;

            if (idBill == -1)
            {
                BillDAO.Instance.InsertBill(id);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), productID, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, productID, count);
            }

            ShowBill(id);

            //LoadListCustomer();
        }


        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbId.Text);

            string name = txbName.Text;

            int idBill = BillDAO.Instance.GetUnCheckBillIDByCustomerID(id);

            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);

            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có muốn in hóa đơn " + name, "Thông báo", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    PrintBill();

                    BillDAO.Instance.CheckOut(idBill, discount, (float)totalPrice);
                    ShowBill(id);

                    LoadListCustomer();

                    
                }
                else 
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)totalPrice);
                    ShowBill(id);

                    LoadListCustomer();

                    MessageBox.Show("Thanh toán hóa đơn của " + name + " thành công");
                }
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {

            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            txbTotalPrice.Text = finalTotalPrice.ToString("c");
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string name = txbName.Text;
            string address = txbAddress.Text;
            string phoneNumber = txbPhoneNumber.Text;



            if (CustomerDAO.Instance.InsertCustomer(name, address, phoneNumber))
            {
                MessageBox.Show("Thêm khách hàng thành công");
                LoadListCustomer();


            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm khách hàng");
            }

        }



        void f_UpdateAccount(object sender, AccountEvent e)
        {
            infoToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void btnSearchCustomer_Click(object sender, EventArgs e)
        {

            customerList.DataSource = SearchCustomerByPhoneNumber(txbPhoneNumber.Text);

        }

       

       

        private void dtgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(txbId.Text);
                ShowBill(id);
                LoadListCustomer();
            }
        }

       
    }

    #endregion



}
