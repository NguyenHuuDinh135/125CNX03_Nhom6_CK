using System;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;

namespace _125CNX03_Nhom6_CK.GUI.Forms
{
    public partial class BlogForm : Form
    {
        private readonly IBaiVietService _articleService;

        public BlogForm()
        {
            InitializeComponent();
            _articleService = new BaiVietService();
            LoadArticles();
        }

        private void LoadArticles()
        {
            dataGridViewArticles.DataSource = null;
            var articles = _articleService.GetActiveArticles();
            dataGridViewArticles.DataSource = ConvertToDataTable(articles);
        }

        private System.Data.DataTable ConvertToDataTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Tiêu đề", typeof(string));
            dt.Columns.Add("Tóm tắt", typeof(string));
            dt.Columns.Add("Ngày đăng", typeof(DateTime));

            foreach (var element in elements)
            {
                dt.Rows.Add(
                    int.Parse(element.Element("Id")?.Value ?? "0"),
                    element.Element("TieuDe")?.Value ?? "",
                    element.Element("TomTat")?.Value ?? "",
                    DateTime.Parse(element.Element("NgayDang")?.Value ?? DateTime.Now.ToString())
                );
            }

            return dt;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}