using System.Collections.ObjectModel;
using System.Windows;
using Caliburn.Micro;
using MahApps.Metro.Controls.Dialogs;
using MySql.Data.MySqlClient;
using WpfBasicApp02.Model;

namespace WpfBasicApp02.ViewModels
{
    class MainViewModel : Conductor<object>
    {
        public IDialogCoordinator _dialogCoordinator;    // 메시지박스, 다이얼로그 실행을 위한 변수
        public ObservableCollection<KeyValuePair<string, string>> Divisions { get; set; }

        public ObservableCollection<Book> Books { get; set; }

        // 선택된값에 대한 멤버변수, 멤버변수는 _를 붙이거나, 손문자로 변수명 시작
        private Book _selectedBook;

        // 선택된 값에 대한 속성
        public Book SelectedBook
        {

            get => _selectedBook;    // 람다식 //get { return _selectedBook; }과 동일
            set
            {
                _selectedBook = value;
                NotifyOfPropertyChange(() => SelectedBook);
            }
        }

        public MainViewModel()
        {
            LoadControlFromDB();
            LoadGridFromDB();
        }

        private void LoadControlFromDB()
        {
            // 1. 연결문자열(DB연결문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용쿼리
            string query = "SELECT division, names FROM divtbl";

            ObservableCollection<KeyValuePair<string, string>> divisions = new ObservableCollection<KeyValuePair<string, string>>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader(); // 데이터 가져올때                    

                    while (reader.Read())
                    {
                        var division = reader.GetString("division");
                        var names = reader.GetString("names");

                        divisions.Add(new KeyValuePair<string, string>(division, names));
                    }
                }
                catch (MySqlException ex)
                {
                    // 나중에...
                }
            } // conn.Close() 자동발생

            Divisions = divisions;
        }

        private void LoadGridFromDB()
        {
            // 1. 연결문자열(DB연결문자열은 필수)
            string connectionString = "Server=localhost;Database=bookrentalshop;Uid=root;Pwd=12345;Charset=utf8;";
            // 2. 사용쿼리, 기본쿼리로 먼저 작업 후 필요한 실제쿼리로 변경해도
            string query = @"SELECT b.Idx, b.Author, b.Division, b.Names, b.ReleaseDate, b.ISBN, b.Price,
                        d.Names AS dNames
                   FROM bookstbl AS b, divtbl AS d
                  WHERE b.Division = d.Division
                  ORDER by b.Idx";

            ObservableCollection<Book> books = new ObservableCollection<Book>();

            // 3. DB연결, 명령, 리더
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            Idx = reader.GetInt32("Idx"),
                            Division = reader.GetString("Division"),
                            DName = reader.GetString("dNames"),
                            Names = reader.GetString("Names"),
                            Author = reader.GetString("Author"),
                            ISBN = reader.GetString("ISBN"),
                            ReleaseDate = reader.GetDateTime("ReleaseDate"),
                            Price = reader.GetInt32("Price"),
                        });
                    }
                }
                catch (MySqlException ex)
                {
                    // 나중에...
                }
            } // conn.Close() 자동발생

            Books = books;
            NotifyOfPropertyChange(() => Books);
        }

        public async void DoAction()
        {
            await _dialogCoordinator.ShowMessageAsync(this, "데이터로드!", "로드");
        }
    }
}
