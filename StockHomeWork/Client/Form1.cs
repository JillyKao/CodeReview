using Protocol;
using SocketLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    /// <summary>
    /// 需定義一個方法 具有一個 DataGridViewCellFormattingEventArgs參數 以供呼叫
    /// </summary>
    public class CustomConditionStyleAttribute : Attribute
    {
        public string ActionName { get; set; }
        public CustomConditionStyleAttribute(string actionName)
        {
            this.ActionName = actionName;
        }
    }
    public enum StockState
    {
        Up,//上漲
        Down,//下跌
        Flat,//持平
    }
    public class StockData
    {
        public int ID { get; set; }
        [Browsable(false)]
        public float ClosingPrice { get; set; }
        public string Name { get; set; }
        [DisplayName("成交")]
        [DisplayFormat(DataFormatString = "0.00", NullDisplayText = "--")]
        [CustomConditionStyle(nameof(CustomPriceStyle))]
        public float? Price { get; set; }

        [DisplayName("漲跌")]
        [DisplayFormat(DataFormatString = "0.00", NullDisplayText = "--")]
        [CustomConditionStyle(nameof(CustomStyle))]
        public float? Changes => Price - ClosingPrice;

        [DisplayName("漲跌幅")]
        [DisplayFormat(DataFormatString = "0.00%", NullDisplayText = "--")]
        [CustomConditionStyle(nameof(CustomStyle))]
        public float? ChangesRate => Changes / ClosingPrice;

        private StockState StockState
        {
            get
            {
                return Price switch
                {
                    var p when p > ClosingPrice => StockState.Up,
                    var p when p < ClosingPrice => StockState.Down,
                    _ => StockState.Flat,
                };
            }
        }
        public void CustomStyle(DataGridViewCellFormattingEventArgs args)
        {
            args.CellStyle.ForeColor =
               StockState switch
               {
                   StockState.Up => Color.Red,
                   StockState.Down => Color.Green,
                   StockState.Flat => Color.Gold,
               };
        }
        public void CustomPriceStyle(DataGridViewCellFormattingEventArgs args)
        {
            CustomStyle(args);
            args.FormattingApplied = true;
            args.Value =
               StockState switch
               {
                   StockState.Up => $"▲ {args.Value:f2}",
                   StockState.Down => $"▼ {args.Value:f2}",
                   StockState.Flat => $"{args.Value:f2}",
               };
        }
    }
    public partial class Form1 : Form
    {
        private Dictionary<int, StockData> StockDataDic { get; set; }
        private BindingList<StockData> StockDatasBindingList { get; set; }
        private DataGridView GridView { get; set; }
        private string IP = "127.0.0.1";
        private int Port = 2001;
        public Form1()
        {

            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            GridView = new DataGridView();
            GridView.Dock = DockStyle.Fill;
            this.Controls.Add(GridView);
            _ = Task.Run(async () =>
            {
                var client = new TcpSocketClient(IP, Port);
                async Task keepConnect()
                {
                    while (!(await client.StartClient()))//連線失敗 則每秒繼續重連一次
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    _ = client.StartReceiveAsync();
                }

                client.OnDisconnect += async (o) =>//斷線重連
                {
                    client.ResetSocket();
                    await keepConnect();
                };
                client.OnDataReceive += (datas) =>
                {
                    var code = (ProtocolDefine)datas[0];
                    var response = datas.Skip(1).Take(datas.Length - 1).ToArray();
                    switch (code)
                    {
                        case ProtocolDefine.ResponseStockData:
                            {
                                IniDataSource(response);
                            }
                            break;
                        case ProtocolDefine.UpdateStockData:
                            {
                                UpdateStockData(response);
                            }
                            break;
                        default:
                            {
                                Console.WriteLine("Un Define Code");
                            }
                            break;
                    }
                };
                await keepConnect();
               
                await client.SendAsync(BitConverter.GetBytes((int)ProtocolDefine.RequestStockData));//要求股票資料
            });
        }
        public class StockPriceData
        {
            public int ID { get; set; }
            public float? Price { get; set; }
        }
        private void UpdateStockData(byte[] response)
        {
            if (StockDataDic == null)
                return;
            var json = Encoding.UTF8.GetString(response);
            var stockDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StockPriceData>>(json);
            foreach(var stock in stockDatas)
            {
                if(StockDataDic.TryGetValue(stock.ID, out var data))
                {
                    data.Price = stock.Price;
                }
            }
            this.Invoke(new MethodInvoker(() => GridView.Refresh()));
        }
        private void IniDataSource(byte[] response)
        {
            try
            {
                var json = Encoding.UTF8.GetString(response);
                var stockDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<StockData>>(json);
                StockDataDic = stockDatas.ToDictionary(x => x.ID, x => x);
                StockDatasBindingList = new BindingList<StockData>(stockDatas);
                StockDatasBindingList.AllowEdit = false;
                StockDatasBindingList.AllowNew = false;
                StockDatasBindingList.AllowRemove = false;
                this.Invoke(new MethodInvoker(() =>
                {
                    GridView.DataSource = StockDatasBindingList;
                    var type = typeof(StockData);
                    var customConditionStyle = new Dictionary<int, Action<StockData, DataGridViewCellFormattingEventArgs>>();
                    for (int i = 0; i < GridView.ColumnCount; i++)
                    {
                        var column = GridView.Columns[i];
                        var property = type.GetProperty(column.DataPropertyName);
                        {
                            var att = property.GetCustomAttribute<DisplayFormatAttribute>();
                            column.DefaultCellStyle.Format = att?.DataFormatString ?? "";
                            column.DefaultCellStyle.NullValue = att?.NullDisplayText ?? "";
                        }
                        {
                            var att = property.GetCustomAttribute<CustomConditionStyleAttribute>();
                            if (att != null)
                            {
                                var method = type.GetMethod(att.ActionName);
                                if (method != null)
                                {
                                    customConditionStyle.Add(i, (stock, args) =>
                                    {
                                        method.Invoke(stock, new object[] { args });
                                    });
                                }
                            }
                        }
                    }
                    GridView.CellFormatting += (s, e) =>
                    {
                        if (customConditionStyle.TryGetValue(e.ColumnIndex, out var action))
                        {
                            action?.Invoke(StockDatasBindingList[e.RowIndex], e);
                        }
                    };
                }));
               
            }
            catch(Exception e)
            {

            }
          
        }

    }
}


