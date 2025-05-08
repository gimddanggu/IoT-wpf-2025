using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;

namespace WpfBasicApp01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 메인윈도우 로드 후 이벤트처리 헨들러
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // DB연결
            // 데이터그리드에 데이터를 읽어오기 
            LoadControlFromDB();
            LoadGridFromDB();
        }

        private async void LoadControlFromDB()
        {
            // 1. 연결문자열(DB연결문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용쿼리
            string query = "SELECT Division, Names FROM divtbl";

            // Dictionary나 KeyValuePair 둘 다 상관없음
            List<KeyValuePair<string, string>> divisions = new List<KeyValuePair<string, string>>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            { // using 문을 쓰면 close 자동으로 해줌
                try
                {
                    conn.Open();    
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 데이터 가져올 때

                    while (reader.Read())
                    {
                        var division = reader.GetString("Division");
                        var names = reader.GetString("Names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                    
                }
                catch (MySqlException ex)
                {
                    await this.ShowMessageAsync($"에러! {ex.Message}", "에러");
                }
            }
            CboDivisions.ItemsSource = divisions; // 데이터 연동
            CboDivisions.SelectedValuePath = "Key";  // 선택된 값은 Value로 바인딩
            CboDivisions.DisplayMemberPath = "Value";   // 보여지는 텍스트는 Key로 바인딩
        }

        private void LoadGridFromDB()
        {
            // 1. 연결문자열(DB연결문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용쿼리,기본쿼리로 먼저 작업 후 필요한 실제쿼리로 변경해도 ㄱㅊ
            string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price,
	                            d.Names as dNames
                                FROM bookstbl b, divtbl d
                                WHERE b.Division = d.Division
                                ORDER BY b.Idx;";

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            { // using 문을 쓰면 close 자동으로 해줌
                try
                {
                    conn.Open();
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    GrdBooks.ItemsSource = dt.DefaultView;

                }
                catch (MySqlException ex)
                {
                    // 나중에

                }
            }
        }
        /// <summary>
        /// 데이터그리드 더블 클릭 이벤트 핸들러
        /// 선택한 그리드의 레코드값 오른쪽 상세에 출력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void GrdBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (GrdBooks.SelectedItems.Count == 1)
            {
                // 그리드 데이터를 하나만 선택했을 때
                var item = GrdBooks.SelectedItems[0] as DataRowView;    // 데이터 그리드의 데이터는 IList 형식, 그 중 한 건은 DataRowView 객체로 형 변환 가능

                NudIdx.Value = Convert.ToDouble(item.Row["Idx"]);
                CboDivisions.SelectedValue = Convert.ToString(item.Row["Division"]);
                TxtNames.Text = Convert.ToString(item.Row["Names"]);
                TxtAuthor.Text = Convert.ToString(item.Row["Author"]);
                TxtIsbn.Text = Convert.ToString(item.Row["ISBN"]);
                TxtPrice.Text = Convert.ToString(item.Row["Price"]);
                DpcReleaseDate.Text = Convert.ToString(item.Row["ReleaseDate"]);
                //MessageBox.Show(item.Row["Names"].ToString());
            }
            await this.ShowMessageAsync($"처리완료!", "메시지");
        }
    }
}