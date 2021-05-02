using Northwind.Business.Abstract;
using Northwind.Business.Concrete;
using Northwind.Business.DependencyResolvers.Ninject;
using Northwind.DataAccess.Concrete.EntityFramework;
using Northwind.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Northwind.WebFormsUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _productService = InstanceFactory.GetInstance<IProductService>();
            _categoryService = InstanceFactory.GetInstance<ICategoryService>();
        }

        private IProductService _productService;
        private ICategoryService _categoryService;
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadProduct();
            LoadCategory();
        }

        private void LoadProduct()
        {
            dgwProduct.DataSource = _productService.GetAll();
        }

        private void LoadCategory()
        {
            cbxCategory.DataSource = _categoryService.GetAll();
            cbxCategory.DisplayMember = "CategoryName";
            cbxCategory.ValueMember = "CategoryID";

            cbxCategoryName.DataSource = _categoryService.GetAll();
            cbxCategoryName.DisplayMember = "CategoryName";
            cbxCategoryName.ValueMember = "CategoryID";
        }

        private void cmBoxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                dgwProduct.DataSource = _productService.GetListByCategory(Convert.ToInt32(cbxCategory.SelectedValue.ToString()));
            }
            catch (Exception)
            {

            }
        }

        private void txtBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBoxSearch.Text))
            {
                dgwProduct.DataSource = _productService.GetAllByFilter(txtBoxSearch.Text);
            }
            else
            {
                LoadProduct();
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (btnAdd.Text == "EKLE")
            {
                try
                {
                    _productService.Add(
                                      new Product
                                      {
                                          ProductName = txtProductName.Text,
                                          CategoryID = Convert.ToInt32(cbxCategoryName.SelectedValue),
                                          QuantityPerUnit = txtStock.Text,
                                          UnitPrice = Convert.ToDecimal(txtPrice.Text),
                                          UnitsInStock = Convert.ToInt16(txtUnitsInStock.Text)
                                      });
                    MessageBox.Show("Ürün kaydetme işlemi başarıyla gerçekleştirildi.", "KAYIT İŞLEMİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProduct();
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }

            }
            else if (btnAdd.Text == "GÜNCELLE")
            {
                try
                {
                    _productService.Update(new Product
                    {
                        ProductID = Convert.ToInt32(dgwProduct.CurrentRow.Cells["ProductID"].Value),
                        CategoryID = Convert.ToInt32(cbxCategoryName.SelectedValue),
                        ProductName = txtProductName.Text,
                        QuantityPerUnit = txtStock.Text,
                        UnitPrice = Convert.ToDecimal(txtPrice.Text),
                        UnitsInStock = Convert.ToInt16(txtUnitsInStock.Text)
                    });
                    MessageBox.Show("Ürün güncelleme işlemi başarıyla gerçekleştirildi.", "GÜNCELLEME İŞLEMİ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProduct();
                    ClearTextboxes(this.Controls);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message);
                }
            }

        }

        private void dgwProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtProductName.Text = dgwProduct.CurrentRow.Cells["ProductName"].Value.ToString();
            cbxCategoryName.SelectedValue = Convert.ToInt32(dgwProduct.CurrentRow.Cells["CategoryID"].Value);
            txtStock.Text = dgwProduct.CurrentRow.Cells["QuantityPerUnit"].Value.ToString();
            txtPrice.Text = dgwProduct.CurrentRow.Cells["UnitPrice"].Value.ToString();
            txtUnitsInStock.Text = dgwProduct.CurrentRow.Cells["UnitsInStock"].Value.ToString();
            btnAdd.Text = "GÜNCELLE";
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearTextboxes(this.Controls);
        }


        private void ClearTextboxes(Control.ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                ClearTextboxes(ctrl.Controls);
            }
            btnAdd.Text = "EKLE";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                _productService.Delete(new Product
                {
                    ProductID = Convert.ToInt32(dgwProduct.CurrentRow.Cells["ProductID"].Value)
                });
            }
            catch
            {
                MessageBox.Show("Veri silme işlemi başarısız. Silmek istediğiniz veri diğer tablolarla bağlantılı.");
            }

        }
    }
}
