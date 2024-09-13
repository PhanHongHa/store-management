
using QuanLyCuaHang.DAO;
using QuanLyCuaHang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace QuanLyCuaHang
{
    public partial class fAdmin : Form
    {
        BindingSource productList = new BindingSource();


        BindingSource categoryList = new BindingSource();

        BindingSource customerList = new BindingSource();

        BindingSource accountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();

        }



        #region methods

        void Load()
        {
            dtgvProduct.DataSource = productList;
            dtgvCategory.DataSource = categoryList;
            dtgvCustomer.DataSource = customerList;
            dtgvAccount.DataSource = accountList;

            LoadDateTimePickerBill();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);

            LoadListProduct();
            LoadCategoryIntoCombobox(cbCategory);
            AddProductBinding();

            LoadListCategory();
            AddCategoryBinding();

            LoadListCustomer();
            AddCustomerBinding();

            LoadAccount();
            AddAccountBinding();
        }
        void LoadDateTimePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }
        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }


        void LoadListProduct()
        {
            productList.DataSource = ProductDAO.Instance.GetListProduct();
        }


        void AddProductBinding()
        {

            txbIdProduct.DataBindings.Add(new Binding("Text", dtgvProduct.DataSource, "ID", true, DataSourceUpdateMode.Never));

            txbProductName.DataBindings.Add(new Binding("Text", dtgvProduct.DataSource, "Name", true, DataSourceUpdateMode.Never));
            nmProductPrice.DataBindings.Add(new Binding("Value", dtgvProduct.DataSource, "Price", true, DataSourceUpdateMode.Never));
        }

        List<Product> SearchProductByName(string name)
        {

            List<Product> listProduct = ProductDAO.Instance.SearchProductByName(name);

            return listProduct;

        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }



        private void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
            dtgvCategory.Columns["Name"].HeaderText = "Tên Loại";
        }

        void AddCategoryBinding()
        {

            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbCategoryId.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Id", true, DataSourceUpdateMode.Never));

        }

        List<Category> SearchCategoryByName(string name)
        {

            List<Category> listCategory = CategoryDAO.Instance.SearchCategoryByName(name);

            return listCategory;

        }

        private void LoadListCustomer()
        {
            customerList.DataSource = CustomerDAO.Instance.LoadCustomerList();
            dtgvCustomer.Columns["Name"].HeaderText = "Tên bàn";
            dtgvCustomer.Columns["Status"].HeaderText = "Trạng thái";
        }

        void AddCustomerBinding()
        {

            txbTableName.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbTableId.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbTableStatus.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Status", true, DataSourceUpdateMode.Never));

            txbTableAddress.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "Address", true, DataSourceUpdateMode.Never));
            txbPhoneNumber.DataBindings.Add(new Binding("Text", dtgvCustomer.DataSource, "PhoneNumber", true, DataSourceUpdateMode.Never));
        }

        private object SearchAccountByDisplayName(string displayName)
        {
            List<Account> listAccount = AccountDAO.Instance.SearchAccountByDisplayName(displayName);

            return listAccount;
        }

        private void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
            dtgvAccount.Columns["UserName"].HeaderText = "Tên tài khoản";
            dtgvAccount.Columns["DisplayName"].HeaderText = "Tên hiển thị";
            dtgvAccount.Columns["Type"].HeaderText = "Loại tài khoản";
        }

        void AddAccountBinding()
        {

            txbAccountName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }


        private void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại tài khoản thành công");

            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
        }


        private void PrintBill()
        {
            
        }

        #endregion

        #region events
        private void btnViewBill_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }



        private void btnViewProduct_Click(object sender, EventArgs e)
        {
            LoadListProduct();
        }


        private void txbProductID_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                if (dtgvProduct.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvProduct.SelectedCells[0].OwningRow.Cells["CategoryID"].Value;

                    Category cateogory = CategoryDAO.Instance.GetCategoryById(id);

                    cbCategory.SelectedItem = cateogory;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbCategory.Items)
                    {
                        if (item.Id == cateogory.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbCategory.SelectedIndex = index;
                }
            }
            catch
            { }
           
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txbProductName.Text;
            int categoryID = (cbCategory.SelectedItem as Category).Id;
            float price = (float)nmProductPrice.Value;

            if (ProductDAO.Instance.InsertProduct(name, categoryID, price))
            {
                MessageBox.Show("Thêm sản phầm thành công");
                LoadListProduct();
                if (insertProduct != null)
                    insertProduct(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm thức ăn");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string name = txbProductName.Text;
            int categoryID = (cbCategory.SelectedItem as Category).Id;
            float price = (float)nmProductPrice.Value;
            int id = Convert.ToInt32(txbIdProduct.Text);

            if (ProductDAO.Instance.UpdateProduct( name, categoryID, price,id))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListProduct();
                if (updateProduct != null)
                    updateProduct(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa thức ăn");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbIdProduct.Text);

            if (ProductDAO.Instance.DeleteProduct(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListProduct();
                if (deleteProduct != null)
                    deleteProduct(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa sản phầm");
            }
        }

        private void ToExcel(DataGridView dtgvBill, string fileName)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;

                workbook = excel.Workbooks.Add(Type.Missing);
                excel.Columns.ColumnWidth = 25;

                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                worksheet.Name = "Doanh thu";

                // export header
                for (int i = 0; i < dtgvBill.ColumnCount; i++)
                {
                    worksheet.Cells[1, i + 1] = dtgvBill.Columns[i].HeaderText;
                }

                // export content
                for (int i = 0; i < dtgvBill.RowCount; i++)
                {
                    for (int j = 0; j < dtgvBill.ColumnCount; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dtgvBill.Rows[i].Cells[j].Value.ToString();
                    }
                }

                // save workbook
                workbook.SaveAs(fileName);
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Xuất dữ liệu thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        #endregion

        private event EventHandler insertProduct;
        public event EventHandler InsertProduct
        {
            add { insertProduct += value; }
            remove { insertProduct -= value; }
        }

        private event EventHandler deleteProduct;
        public event EventHandler DeleteProduct
        {
            add { deleteProduct += value; }
            remove { deleteProduct -= value; }
        }

        private event EventHandler updateProduct;
        public event EventHandler UpdateProduct
        {
            add { updateProduct += value; }
            remove { updateProduct -= value; }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            productList.DataSource = SearchProductByName(txbSearchFood.Text);
        }


        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnViewTable_Click(object sender, EventArgs e)
        {
            LoadListCustomer();
        }


        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;



            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm loại món thành công");
                LoadListCategory();


            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryId.Text);
            try
            {
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa loại món thành công");
                    LoadListCategory();


                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa loại món");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + " n/ Không xóa được loại ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {

            string name = txbCategoryName.Text;


            int id = Convert.ToInt32(txbCategoryId.Text);

            if (CategoryDAO.Instance.UpdateCategory(name, id))
            {
                MessageBox.Show("Sửa loại món thành công");
                LoadListCategory();


            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa loại món");
            }

        }

        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            categoryList.DataSource = SearchCategoryByName(txbCategorySearch.Text);
        }



        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string address = txbTableAddress.Text;
            string phoneNumber = txbPhoneNumber.Text;



            if (CustomerDAO.Instance.InsertCustomer(name, address, phoneNumber))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListCustomer();


            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableId.Text);
            try
            {
                if (CustomerDAO.Instance.DeleteCustomer(id))
                {
                    MessageBox.Show("Xóa bàn thành công");
                    LoadListCustomer();


                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa loại món");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + " n/ Không xóa được bàn ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUpdateCustomer_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            string address = txbTableAddress.Text;
            string phoneNumber = txbPhoneNumber.Text;


            int id = Convert.ToInt32(txbTableId.Text);

            if (CustomerDAO.Instance.UpdateCustomer(name, id, address, phoneNumber))
            {
                MessageBox.Show("Sửa bàn thành công");
                LoadListCustomer();


            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn");
            }
        }

        private void btnSearchTable_Click(object sender, EventArgs e)
        {
            productList.DataSource = SearchProductByName(txbSearchTable.Text);
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }



        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;

            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm tài khoản");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;


            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
                LoadAccount();


            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa tài khoản");
            }
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;



            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công");
                LoadAccount();


            }
            else
            {
                MessageBox.Show("Tên tài khoản không được sửa!");
            }
        }

        private void btnSearchAccount_Click(object sender, EventArgs e)
        {
            accountList.DataSource = SearchAccountByDisplayName(txbSearchAccount.Text);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;
            ResetPassword(userName);
        }

        private void btnViewCustomer_Click(object sender, EventArgs e)
        {
            LoadListCustomer();
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "Excel 2007 or Higher (.xlsx) | *.xlsx ";
            if(sf.ShowDialog() == DialogResult.OK)
            {
                ToExcel(dtgvBill, sf.FileName);
            }
        }

        private void dtgvBill_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                PrintBill();
            }
           
        }

       
    }
}
