using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class BannerForm : Form
    {
        private readonly IBannerService _bannerService;

        public BannerForm()
        {
            InitializeComponent();
            _bannerService = new BannerService();
            LoadData();
        }

        private void LoadData()
        {
            dataGridViewBanners.DataSource = null;
            var banners = _bannerService.GetAllBanners();
            dataGridViewBanners.DataSource = ConvertToDataTable(banners);
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tên banner", typeof(string));
            dt.Columns.Add("Đường dẫn ảnh", typeof(string));
            dt.Columns.Add("Liên kết", typeof(string));
            dt.Columns.Add("Thứ tự", typeof(int));
            dt.Columns.Add("Hiển thị", typeof(bool));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    element.Element("TenBanner")?.Value ?? "",
                    element.Element("HinhAnh")?.Value ?? "",
                    element.Element("LienKet")?.Value ?? "",
                    int.Parse(element.Element("ThuTu")?.Value ?? "0"),
                    bool.Parse(element.Element("HienThi")?.Value ?? "false")
                );
            }

            return dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBannerName.Text) || string.IsNullOrEmpty(txtImageUrl.Text))
            {
                MessageBox.Show("Vui lòng nhập tên banner và đường dẫn ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var newBanner = new XElement("Banner",
                new XElement("TenBanner", txtBannerName.Text),
                new XElement("HinhAnh", txtImageUrl.Text),
                new XElement("LienKet", txtLink.Text),
                new XElement("ThuTu", int.Parse(txtOrder.Text)),
                new XElement("HienThi", chkDisplay.Checked)
            );

            _bannerService.AddBanner(newBanner);
            LoadData();
            ClearForm();
            MessageBox.Show("Thêm banner thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridViewBanners.SelectedRows.Count > 0)
            {
                if (string.IsNullOrEmpty(txtBannerName.Text) || string.IsNullOrEmpty(txtImageUrl.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên banner và đường dẫn ảnh!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dataGridViewBanners.SelectedRows[0];
                var bannerId = (int)selectedRow.Cells["Id"].Value;

                var banner = _bannerService.GetBannerById(bannerId);
                if (banner != null)
                {
                    banner.Element("TenBanner").Value = txtBannerName.Text;
                    banner.Element("HinhAnh").Value = txtImageUrl.Text;
                    banner.Element("LienKet").Value = txtLink.Text;
                    banner.Element("ThuTu").Value = int.Parse(txtOrder.Text).ToString();
                    banner.Element("HienThi").Value = chkDisplay.Checked.ToString();

                    _bannerService.UpdateBanner(banner);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Cập nhật banner thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn banner để cập nhật!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewBanners.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewBanners.SelectedRows[0];
                var bannerId = (int)selectedRow.Cells["Id"].Value;

                var result = MessageBox.Show("Bạn có chắc chắn muốn xóa banner này?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    _bannerService.DeleteBanner(bannerId);
                    LoadData();
                    ClearForm();
                    MessageBox.Show("Xóa banner thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn banner để xóa!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridViewBanners_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBanners.SelectedRows.Count > 0)
            {
                var selectedRow = dataGridViewBanners.SelectedRows[0];
                var bannerId = (int)selectedRow.Cells["Id"].Value;

                var banner = _bannerService.GetBannerById(bannerId);
                if (banner != null)
                {
                    txtBannerName.Text = banner.Element("TenBanner").Value;
                    txtImageUrl.Text = banner.Element("HinhAnh").Value;
                    txtLink.Text = banner.Element("LienKet").Value;
                    txtOrder.Text = banner.Element("ThuTu").Value;
                    chkDisplay.Checked = bool.Parse(banner.Element("HienThi").Value);
                }
            }
        }

        private void ClearForm()
        {
            txtBannerName.Clear();
            txtImageUrl.Clear();
            txtLink.Clear();
            txtOrder.Text = "0";
            chkDisplay.Checked = true;
        }
    }
}