using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LockDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入你的名字");
            var myname = Console.ReadLine();

            string connstr = ConfigurationManager.ConnectionStrings["connstr"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine("开始查询");
                        using (var selectCmd = conn.CreateCommand())
                        {
                            selectCmd.Transaction = tx;
                            selectCmd.CommandText = "select * from T_Girls with(xlock,ROWLOCK) where id=1";
                            using (var reader = selectCmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    Console.WriteLine("没有id为1的女孩");
                                    return;
                                }
                                string bf = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("BF")))
                                {
                                    bf = reader.GetString(reader.GetOrdinal("BF"));
                                }
                                if (!string.IsNullOrEmpty(bf))//已经有男朋友
                                {
                                    if (bf == myname)
                                    {
                                        Console.WriteLine("早已经是我的人了");
                                    }
                                    else
                                    {
                                        Console.WriteLine("早已经被" + bf + "抢走了");
                                    }
                                    Console.ReadKey();
                                    return;
                                }
                                //如果bf==null，则继续向下抢
                            }
                            Console.WriteLine("查询完成，开始update");
                            using (var updateCmd = conn.CreateCommand())
                            {
                                updateCmd.Transaction = tx;
                                updateCmd.CommandText = "Update T_Girls set BF='aaa' where id=1";
                                updateCmd.ExecuteNonQuery();
                            }
                            Console.WriteLine("结束Update");
                            Console.WriteLine("按任意键结束事务");
                            Console.ReadKey();
                        }
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                        tx.Rollback();
                    }
                }
            }
        }
    }
}
