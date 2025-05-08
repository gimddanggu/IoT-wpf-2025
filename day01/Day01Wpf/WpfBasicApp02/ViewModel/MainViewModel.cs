using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace WpfBasicApp02.ViewModel
{
    class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel() {
            LoadControlFromDB();
            LoadGridFromDB();
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


                }
                catch (MySqlException ex)
                {
                    // 나중에

                }
            }
        }

        private void LoadControlFromDB()
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
                    // 나중에

                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}


