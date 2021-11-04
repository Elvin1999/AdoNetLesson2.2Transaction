using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            #region DataViewer


            //using (var connection=new SqlConnection())
            //{
            //    connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
            //    DataSet set = new DataSet();
            //    var da = new SqlDataAdapter("select Id,Name,Pages,YearPress from Books", connection);

            //    da.Fill(set, "Books");

            //    DataViewManager dvm = new DataViewManager(set);
            //    dvm.DataViewSettings["Books"].RowFilter = "Pages>40";
            //    dvm.DataViewSettings["Books"].RowFilter = "YearPress>2000";
            //    dvm.DataViewSettings["Books"].Sort = "YearPress DESC";
            //    DataView dv = dvm.CreateDataView(set.Tables["Books"]);

            //    mygrid.ItemsSource = dv;

            //}

            #endregion
            #region Transaction

            #region UpdateStoredProcedure
                //            create procedure sp_UpdateBook
                //@MyId int,
                //@Page int
                //as
                //begin

                //SELECT* FROM Books where Id = @MyId
                //if (EXISTS(SELECT * FROM Books where Id = @MyId))
                //                begin
                //                print 'i updated'
                //update Books
                //set Pages = @Page
                //where Id = @MyId
                //end

                //    else
                //                begin
                //                print 'i can not update'
                //rollback transaction
                //end
                //end
            #endregion


            SqlTransaction sqlTransaction = null;
            using (SqlConnection conn=new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                conn.Open();
                sqlTransaction = conn.BeginTransaction();

                SqlCommand comm1 = new SqlCommand("insert into Press(Id,Name) values(@Id,@Name)", conn);
                comm1.Transaction = sqlTransaction;

                SqlParameter param1 = new SqlParameter();
                param1.Value = 5565;
                param1.ParameterName = "@Id";
                param1.SqlDbType = SqlDbType.Int;

                SqlParameter param2 = new SqlParameter();
                param2.Value = "John";
                param2.ParameterName = "@Name";
                param2.SqlDbType = SqlDbType.NVarChar;


                comm1.Parameters.Add(param1);
                comm1.Parameters.Add(param2);


                SqlCommand comm2 = new SqlCommand("sp_UpdateBook",conn);
                comm2.Transaction = sqlTransaction;
                comm2.CommandType = CommandType.StoredProcedure;

                var p1 = new SqlParameter();
                p1.Value = 4587;
                p1.SqlDbType = SqlDbType.Int;
                p1.ParameterName = "@MyId";

                var p2 = new SqlParameter();
                p2.Value = 1111;
                p2.SqlDbType = SqlDbType.Int;
                p2.ParameterName = "@Page";

                comm2.Parameters.Add(p1);
                comm2.Parameters.Add(p2);


                try
                {
                    comm1.ExecuteNonQuery();
                    comm2.ExecuteNonQuery();
                    sqlTransaction.Commit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    sqlTransaction.Rollback();
                }
                finally
                {
                    MessageBox.Show("OKAY");
           
                }

            }





            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["MyConnString"].ConnectionString;
                DataSet set = new DataSet();
                var da = new SqlDataAdapter("select Id,Name,Pages,YearPress from Books", connection);

                da.Fill(set, "Books");

                DataViewManager dvm = new DataViewManager(set);
                dvm.DataViewSettings["Books"].Sort = "YearPress DESC";
                DataView dv = dvm.CreateDataView(set.Tables["Books"]);

                mygrid.ItemsSource = dv;

            }


            #endregion
        }
    }
}
