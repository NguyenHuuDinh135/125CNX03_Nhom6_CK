using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using _125CNX03_Nhom6_CK.BLL;
using System.IO;
using System.Diagnostics;

namespace _125CNX03_Nhom6_CK.GUI.Forms.User
{
    public partial class OrderDetailForm : Form
    {
        private readonly IDonHangService _orderService;
        private readonly IChiTietDonHangService _orderItemService;
        private XElement _currentOrder;

        public OrderDetailForm(int orderId)
        {
            InitializeComponent();
            _orderService = new DonHangService();
            _orderItemService = new ChiTietDonHangService();

            LoadOrderDetails(orderId);
        }

        private void LoadOrderDetails(int orderId)
        {
            _currentOrder = _orderService.GetOrderById(orderId);
            if (_currentOrder != null)
            {
                InitializeUI(_currentOrder);
            }
            else
            {
                MessageBox.Show("Không tìm thấy đơn hàng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void InitializeUI(XElement order)
        {
            this.Text = $"Chi tiết đơn hàng #{order.Element("Id").Value}";
            this.Size = new Size(800, 650);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterParent;

            // Create header
            Panel headerPanel = new Panel();
            headerPanel.Size = new Size(this.Width - 40, 80);
            headerPanel.Location = new Point(20, 20);
            headerPanel.BackColor = Color.White;
            headerPanel.BorderStyle = BorderStyle.FixedSingle;

            Label headerTitle = new Label();
            headerTitle.Text = $"Chi tiết đơn hàng #{order.Element("Id").Value}";
            headerTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            headerTitle.Location = new Point(20, 20);
            headerTitle.Size = new Size(300, 30);
            headerPanel.Controls.Add(headerTitle);

            Label orderDateLabel = new Label();
            orderDateLabel.Text = $"Ngày đặt: {DateTime.Parse(order.Element("NgayDatHang").Value):dd/MM/yyyy HH:mm}";
            orderDateLabel.Font = new Font("Segoe UI", 10);
            orderDateLabel.Location = new Point(20, 50);
            orderDateLabel.Size = new Size(200, 20);
            headerPanel.Controls.Add(orderDateLabel);

            // Nút Xuất HTML
            Button btnExportHTML = new Button();
            btnExportHTML.Text = "📄 Xuất HTML";
            btnExportHTML.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnExportHTML.Size = new Size(140, 40);
            btnExportHTML.Location = new Point(600, 20);
            btnExportHTML.BackColor = Color.FromArgb(76, 175, 80);
            btnExportHTML.ForeColor = Color.White;
            btnExportHTML.FlatStyle = FlatStyle.Flat;
            btnExportHTML.FlatAppearance.BorderSize = 0;
            btnExportHTML.Cursor = Cursors.Hand;
            btnExportHTML.Click += BtnExportHTML_Click;
            headerPanel.Controls.Add(btnExportHTML);

            this.Controls.Add(headerPanel);

            // Create order info panel
            Panel infoPanel = new Panel();
            infoPanel.Size = new Size(this.Width - 40, 120);
            infoPanel.Location = new Point(20, 120);
            infoPanel.BackColor = Color.White;
            infoPanel.BorderStyle = BorderStyle.FixedSingle;

            Label nameLabel = new Label();
            nameLabel.Text = $"Khách hàng: {order.Element("NguoiNhan_Ten")?.Value ?? "N/A"}";
            nameLabel.Font = new Font("Segoe UI", 10);
            nameLabel.Location = new Point(20, 20);
            nameLabel.Size = new Size(300, 20);
            infoPanel.Controls.Add(nameLabel);

            Label phoneLabel = new Label();
            phoneLabel.Text = $"SĐT: {order.Element("NguoiNhan_SDT")?.Value ?? "N/A"}";
            phoneLabel.Font = new Font("Segoe UI", 10);
            phoneLabel.Location = new Point(20, 45);
            phoneLabel.Size = new Size(200, 20);
            infoPanel.Controls.Add(phoneLabel);

            Label addressLabel = new Label();
            addressLabel.Text = $"Địa chỉ: {order.Element("NguoiNhan_DiaChi")?.Value ?? "N/A"}";
            addressLabel.Font = new Font("Segoe UI", 10);
            addressLabel.Location = new Point(20, 70);
            addressLabel.Size = new Size(700, 20);
            addressLabel.AutoSize = false;
            infoPanel.Controls.Add(addressLabel);

            Label statusLabel = new Label();
            statusLabel.Text = $"Trạng thái: {GetOrderStatusText(int.Parse(order.Element("TrangThaiDonHang").Value))}";
            statusLabel.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            statusLabel.Location = new Point(20, 95);
            statusLabel.Size = new Size(200, 20);
            infoPanel.Controls.Add(statusLabel);

            this.Controls.Add(infoPanel);

            // Create items panel
            Panel itemsPanel = new Panel();
            itemsPanel.Size = new Size(this.Width - 40, 250);
            itemsPanel.Location = new Point(20, 260);
            itemsPanel.BackColor = Color.White;
            itemsPanel.BorderStyle = BorderStyle.FixedSingle;

            Label itemsTitle = new Label();
            itemsTitle.Text = "Danh sách sản phẩm";
            itemsTitle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            itemsTitle.Location = new Point(20, 20);
            itemsTitle.Size = new Size(200, 20);
            itemsPanel.Controls.Add(itemsTitle);

            DataGridView dataGridView = new DataGridView();
            dataGridView.Size = new Size(itemsPanel.Width - 40, itemsPanel.Height - 60);
            dataGridView.Location = new Point(20, 50);
            dataGridView.Font = new Font("Segoe UI", 10);
            dataGridView.BackgroundColor = Color.White;
            dataGridView.BorderStyle = BorderStyle.None;
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView.MultiSelect = false;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.RowTemplate.Height = 30;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.ReadOnly = true;

            var orderItems = _orderItemService.GetOrderItemsByOrderId(int.Parse(order.Element("Id").Value));
            dataGridView.DataSource = ConvertToOrderItemTable(orderItems);

            itemsPanel.Controls.Add(dataGridView);

            this.Controls.Add(itemsPanel);

            // Create total panel
            Panel totalPanel = new Panel();
            totalPanel.Size = new Size(this.Width - 40, 60);
            totalPanel.Location = new Point(20, 530);
            totalPanel.BackColor = Color.White;
            totalPanel.BorderStyle = BorderStyle.FixedSingle;

            Label totalLabel = new Label();
            totalLabel.Text = $"Tổng tiền: {decimal.Parse(order.Element("TongTien").Value):N0}đ";
            totalLabel.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            totalLabel.ForeColor = Color.Red;
            totalLabel.Location = new Point(20, 20);
            totalLabel.Size = new Size(300, 20);
            totalPanel.Controls.Add(totalLabel);

            this.Controls.Add(totalPanel);
        }

        private void BtnExportHTML_Click(object sender, EventArgs e)
        {
            try
            {
                var orderItems = _orderItemService.GetOrderItemsByOrderId(int.Parse(_currentOrder.Element("Id").Value));
                string htmlContent = GenerateOrderHTML(_currentOrder, orderItems);

                // Tạo file HTML trong thư mục Temp
                string fileName = $"DonHang_{_currentOrder.Element("Id").Value}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
                string tempPath = Path.Combine(Path.GetTempPath(), fileName);

                File.WriteAllText(tempPath, htmlContent, System.Text.Encoding.UTF8);

                // Mở file HTML trong trình duyệt
                Process.Start(new ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true
                });

                MessageBox.Show($"Đã xuất file HTML thành công!\n\nĐường dẫn: {tempPath}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất HTML: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GenerateOrderHTML(XElement order, System.Collections.Generic.List<XElement> orderItems)
        {
            var orderId = order.Element("Id").Value;
            var orderDate = DateTime.Parse(order.Element("NgayDatHang").Value);
            var customerName = order.Element("NguoiNhan_Ten")?.Value ?? "N/A";
            var customerPhone = order.Element("NguoiNhan_SDT")?.Value ?? "N/A";
            var customerAddress = order.Element("NguoiNhan_DiaChi")?.Value ?? "N/A";
            var status = GetOrderStatusText(int.Parse(order.Element("TrangThaiDonHang").Value));
            var totalAmount = decimal.Parse(order.Element("TongTien").Value);

            string itemsHtml = "";
            foreach (var item in orderItems)
            {
                var itemName = item.Element("ItemOrdered_TenSanPham")?.Value ?? "N/A";
                var itemPrice = decimal.Parse(item.Element("DonGia").Value);
                var itemQty = int.Parse(item.Element("SoLuong").Value);
                var itemTotal = itemPrice * itemQty;
                var itemImage = item.Element("ItemOrdered_DuongDanAnh")?.Value ?? "";

                itemsHtml += $@"
                <tr>
                    <td>
                        <div class='product-info'>
                            {(string.IsNullOrEmpty(itemImage) ? "" : $"<img src='{itemImage}' alt='{itemName}' class='product-image'>")}
                            <span>{itemName}</span>
                        </div>
                    </td>
                    <td class='text-right'>{itemPrice:N0}đ</td>
                    <td class='text-center'>{itemQty}</td>
                    <td class='text-right font-bold'>{itemTotal:N0}đ</td>
                </tr>";
            }

            return $@"<!DOCTYPE html>
<html lang='vi'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Đơn hàng #{orderId}</title>
    <style>
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            padding: 40px 20px;
            min-height: 100vh;
        }}
        
        .container {{
            max-width: 900px;
            margin: 0 auto;
            background: white;
            border-radius: 20px;
            box-shadow: 0 20px 60px rgba(0,0,0,0.3);
            overflow: hidden;
        }}
        
        .header {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 40px;
            text-align: center;
        }}
        
        .header h1 {{
            font-size: 36px;
            margin-bottom: 10px;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
        }}
        
        .header .order-id {{
            font-size: 48px;
            font-weight: bold;
            margin: 20px 0;
            letter-spacing: 2px;
        }}
        
        .header .date {{
            font-size: 16px;
            opacity: 0.9;
        }}
        
        .content {{
            padding: 40px;
        }}
        
        .section {{
            margin-bottom: 40px;
            background: #f8f9fa;
            padding: 30px;
            border-radius: 15px;
            border-left: 5px solid #667eea;
        }}
        
        .section-title {{
            font-size: 24px;
            color: #667eea;
            margin-bottom: 20px;
            font-weight: bold;
            display: flex;
            align-items: center;
            gap: 10px;
        }}
        
        .section-title::before {{
            content: '';
            width: 5px;
            height: 30px;
            background: #667eea;
            border-radius: 3px;
        }}
        
        .info-grid {{
            display: grid;
            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
            gap: 20px;
        }}
        
        .info-item {{
            display: flex;
            flex-direction: column;
            gap: 5px;
        }}
        
        .info-label {{
            font-size: 14px;
            color: #6c757d;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }}
        
        .info-value {{
            font-size: 16px;
            color: #212529;
            font-weight: 500;
        }}
        
        .status {{
            display: inline-block;
            padding: 10px 20px;
            border-radius: 25px;
            font-weight: bold;
            font-size: 16px;
            background: #fff3cd;
            color: #856404;
        }}
        
        table {{
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
            background: white;
            border-radius: 10px;
            overflow: hidden;
        }}
        
        thead {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }}
        
        th {{
            padding: 18px;
            text-align: left;
            font-weight: 600;
            font-size: 16px;
            letter-spacing: 0.5px;
        }}
        
        td {{
            padding: 18px;
            border-bottom: 1px solid #e9ecef;
        }}
        
        tr:last-child td {{
            border-bottom: none;
        }}
        
        tbody tr {{
            transition: background-color 0.3s ease;
        }}
        
        tbody tr:hover {{
            background-color: #f8f9fa;
        }}
        
        .product-info {{
            display: flex;
            align-items: center;
            gap: 15px;
        }}
        
        .product-image {{
            width: 60px;
            height: 60px;
            object-fit: cover;
            border-radius: 8px;
            border: 2px solid #e9ecef;
        }}
        
        .text-right {{
            text-align: right;
        }}
        
        .text-center {{
            text-align: center;
        }}
        
        .font-bold {{
            font-weight: bold;
            color: #212529;
        }}
        
        .total-section {{
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 30px 40px;
            margin-top: 30px;
            border-radius: 15px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }}
        
        .total-label {{
            font-size: 24px;
            font-weight: 600;
        }}
        
        .total-amount {{
            font-size: 36px;
            font-weight: bold;
            text-shadow: 2px 2px 4px rgba(0,0,0,0.2);
        }}
        
        .footer {{
            text-align: center;
            padding: 30px;
            background: #f8f9fa;
            color: #6c757d;
            font-size: 14px;
        }}
        
        @media print {{
            body {{
                background: white;
                padding: 0;
            }}
            
            .container {{
                box-shadow: none;
            }}
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🛍️ CHI TIẾT ĐƠN HÀNG</h1>
            <div class='order-id'>#{orderId}</div>
            <div class='date'>📅 Ngày đặt: {orderDate:dd/MM/yyyy HH:mm}</div>
        </div>
        
        <div class='content'>
            <div class='section'>
                <div class='section-title'>👤 Thông tin khách hàng</div>
                <div class='info-grid'>
                    <div class='info-item'>
                        <div class='info-label'>Họ tên</div>
                        <div class='info-value'>{customerName}</div>
                    </div>
                    <div class='info-item'>
                        <div class='info-label'>Số điện thoại</div>
                        <div class='info-value'>{customerPhone}</div>
                    </div>
                    <div class='info-item' style='grid-column: 1 / -1;'>
                        <div class='info-label'>Địa chỉ giao hàng</div>
                        <div class='info-value'>{customerAddress}</div>
                    </div>
                    <div class='info-item'>
                        <div class='info-label'>Trạng thái</div>
                        <div><span class='status'>{status}</span></div>
                    </div>
                </div>
            </div>
            
            <div class='section'>
                <div class='section-title'>📦 Danh sách sản phẩm</div>
                <table>
                    <thead>
                        <tr>
                            <th>Sản phẩm</th>
                            <th class='text-right'>Đơn giá</th>
                            <th class='text-center'>Số lượng</th>
                            <th class='text-right'>Thành tiền</th>
                        </tr>
                    </thead>
                    <tbody>
                        {itemsHtml}
                    </tbody>
                </table>
                
                <div class='total-section'>
                    <div class='total-label'>💰 TỔNG THANH TOÁN</div>
                    <div class='total-amount'>{totalAmount:N0}đ</div>
                </div>
            </div>
        </div>
        
        <div class='footer'>
            <p>Cảm ơn bạn đã mua hàng! 💙</p>
            <p>© 2025 - Hệ thống quản lý bán hàng</p>
        </div>
    </div>
</body>
</html>";
        }

        private System.Data.DataTable ConvertToOrderItemTable(System.Collections.Generic.List<XElement> elements)
        {
            var dt = new System.Data.DataTable();
            dt.Columns.Add("Tên sản phẩm", typeof(string));
            dt.Columns.Add("Đơn giá", typeof(decimal));
            dt.Columns.Add("Số lượng", typeof(int));
            dt.Columns.Add("Thành tiền", typeof(decimal));

            foreach (var element in elements)
            {
                var unitPrice = decimal.Parse(element.Element("DonGia").Value);
                var quantity = int.Parse(element.Element("SoLuong").Value);
                var totalPrice = unitPrice * quantity;

                dt.Rows.Add(
                    element.Element("ItemOrdered_TenSanPham").Value,
                    unitPrice,
                    quantity,
                    totalPrice
                );
            }

            return dt;
        }

        private string GetOrderStatusText(int status)
        {
            switch (status)
            {
                case 0: return "Chưa xử lý";
                case 1: return "Đang xử lý";
                case 2: return "Đang giao";
                case 3: return "Đã giao";
                case 4: return "Đã hủy";
                default: return "Không xác định";
            }
        }
    }
}