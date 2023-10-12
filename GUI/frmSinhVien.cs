using BUS;
using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmSinhVien : Form
    {

        private readonly SinhvienService sinhvienService = new SinhvienService();
        private readonly LopService lopService = new LopService();
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void BindGrid(List<Sinhvien> sinhviens)
        {
            dgvSinhVien.Rows.Clear();
            foreach(var item in sinhviens)
            {
                dgvSinhVien.Rows.Add(item.MaSV,item.HotenSV,item.NgaySinh,item.Lop.TenLop);
            }
        }

        private void FillLopCmb(List<Lop> lops)
        {
            cboLop.Items.Clear();
            cboLop.DataSource = lops;
            cboLop.ValueMember = "MaLop";
            cboLop.DisplayMember = "TenLop";
        }

        public void setGridViewStyle(DataGridView dataGridView)
        {
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView.BackgroundColor = Color.White;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            try
            {
                setGridViewStyle(dgvSinhVien);
                var listSinhvien = sinhvienService.GetAll();
                var listLop = lopService.GetAll();
                BindGrid(listSinhvien);
                FillLopCmb(listLop);
                cboLop.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvSinhVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvSinhVien.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvSinhVien.SelectedRows[0];
                txtMaSV.Text = row.Cells[0].Value.ToString();
                txtHoTenSV.Text = row.Cells[1].Value.ToString();
                dtNgaySinh.Text = row.Cells[2].Value.ToString();
                cboLop.Text = row.Cells[3].Value.ToString();
            }
        }

        private void btThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát không?","Thông báo",MessageBoxButtons.YesNo,MessageBoxIcon.Warning);
            if(result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void checkValid()
        {
            if (txtMaSV.Text == "" || txtHoTenSV.Text == "")
                throw new Exception("Vui lòng nhập đầy đủ thông tin");
            if (txtMaSV.Text.Length != 6)
                throw new Exception("Mã sinh viên phải có 6 kí tự");
            
            
        }

        private void ReloadData()
        {
            var listSinhvien = sinhvienService.GetAll();
            BindGrid(listSinhvien);
            txtHoTenSV.Text = "";
            txtMaSV.Text = "";
            cboLop.SelectedIndex = 1;
        }

        private void btThem_Click(object sender, EventArgs e)
        {
            try
            {
                checkValid();
                Sinhvien sv = sinhvienService.FindById(txtMaSV.Text);
                    if (sv != null) throw new Exception("Mã sinh viên đã tồn tại.");
                Sinhvien sinhvien = new Sinhvien() {MaSV = txtMaSV.Text,HotenSV = txtHoTenSV.Text,NgaySinh=dtNgaySinh.Value,MaLop=cboLop.SelectedValue.ToString() };
                
                sinhvienService.InsertUpdate(sinhvien);
                MessageBox.Show("Thêm thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadData();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void btXoa_Click(object sender, EventArgs e)
        {
            try
            {
                Sinhvien sinhvien = sinhvienService.FindById(txtMaSV.Text);
                if (sinhvien == null)
                    throw new Exception("Không tìm thấy mã sinh viên cần xóa.");

                DialogResult result = MessageBox.Show("Bạn có chắn chắn muốn xóa?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if(result == DialogResult.Yes)
                {
                    sinhvienService.DeleteById(txtMaSV.Text);
                    MessageBox.Show("Xóa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ReloadData();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btSua_Click(object sender, EventArgs e)
        {
            try
            {
                checkValid();
                Sinhvien sv = sinhvienService.FindById(txtMaSV.Text);
                    if (sv == null) throw new Exception("Không tìm thấy mã cần sửa.");
                Sinhvien sinhvien = new Sinhvien() { MaSV = txtMaSV.Text, HotenSV = txtHoTenSV.Text, NgaySinh = dtNgaySinh.Value, MaLop = cboLop.SelectedValue.ToString() };
                sinhvienService.InsertUpdate(sinhvien);
                MessageBox.Show("Sửa thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ReloadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string RemoveDiacritics(string text)
        {
            string formD = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char ch in formD)
            {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //List<Sinhvien> sinhviens = new List<Sinhvien>();
            string findName = txtFind.Text;
            findName = RemoveDiacritics(findName);
            for (int i = 0; i < dgvSinhVien.Rows.Count; i++)
            {
                string name = dgvSinhVien.Rows[i].Cells[1].Value.ToString();
                

                name = RemoveDiacritics(name);
                

                bool contains = name.IndexOf(findName, StringComparison.OrdinalIgnoreCase) >= 0;
                if (contains)
                {
                    dgvSinhVien.Rows[i].Visible = true;
                }
                else
                {
                    dgvSinhVien.Rows[i].Visible = false;
                }
            }
        }

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            if (txtFind.Text.Length == 0)
                ReloadData();
        }
    }
}
