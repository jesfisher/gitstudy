using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Net;

namespace ZQCode
{
    public class SQLHelper
    {
        public static string connString = string.Empty;

        #region 格式化的SQL语句
        /// <summary>
        /// 执行增、删、改
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int Update(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //写入错误日志
                WriteLog("执行Update(string sql)时出现错误：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 返回单一结果的查询
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static object GetSingleResult(string sql)
        {
            //MessageBox.Show(connString);
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //写入错误日志
                WriteLog("执行GetSingleResult(string sql)时出现错误：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static SqlDataReader GetReader(string sql)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //写入错误日志
                WriteLog("执行GetReader(string sql)时出现错误：" + ex.Message);
                conn.Close();
                throw ex;
            }
        }
        /// <summary>
        /// 返回单个表的dataset
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(string sql)
        {
            //MessageBox.Show(connString);
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //写入错误日志
                WriteLog("执行GetDataSet(string sql)时出现错误：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// 返回多个表的dataset，并且为每个表名赋值
        /// </summary>
        /// <param name="SqlDic"></param>
        /// <returns></returns>
        public static DataSet GetDataSet(Dictionary<string, string> SqlDic)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                foreach (string tableName in SqlDic.Keys)
                {
                    cmd.CommandText = SqlDic[tableName];
                    da.Fill(ds, tableName);
                }
                return ds;
            }
            catch (Exception ex)
            {
                //写入错误日志
                WriteLog("执行GetDataSet(Dictionary<string,string> SqlDic)时出现错误：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 基于ADO.NET事务，提交多条SQL的增删改
        /// </summary>
        /// <param name="sqlList">SQL语句集合</param>
        /// <returns>返回是否执行成功</returns>
        public static bool UpdataByTran(List<string> sqlList)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            try
            {
                conn.Open();
                cmd.Transaction = conn.BeginTransaction(); //开启事务
                foreach (string sql in sqlList)
                {
                    cmd.CommandText = sql;
                    cmd.ExecuteNonQuery();
                }
                cmd.Transaction.Commit();//提交并自动清除事务
                return true;
            }
            catch (Exception ex)
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction.Rollback();
                }
                //写入错误日志
                WriteLog("执行UpdataByTran(List<string> sqlList)时出现错误：" + ex.Message);
                throw ex;
            }
            finally
            {
                if (cmd.Transaction != null)
                {
                    cmd.Transaction = null;//清除事务
                }
                conn.Close();
            }
        }


        #endregion

        #region 执行带参数的SQL语句或存储过程

        /// <summary>
        /// 执行带参数的SQL语句或存储过程
        /// </summary>
        /// <param name="sqlORproname">SQL语句或存储过程名称</param>
        /// <param name="param">参数数组</param>
        /// <param name="isProcedure">是否是存储过程</param>
        /// <returns></returns>
        public static int Update(string sqlORproname, SqlParameter[] param, bool isProcedure)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sqlORproname, conn);
            if (isProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //将错误信息写入日志
                WriteLog("执行Update(string sqlORproname, SqlParameter[] param, bool isProcedure)方法时发生异常，错误信息：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static object GetSingleResult(string sqlORproname, SqlParameter[] param, bool isProcedure)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sqlORproname, conn);
            if (isProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //将错误信息写入日志
                WriteLog("执行GetSingleResult(string sqlORproname, SqlParameter[] param, bool isProcedure)方法时发生异常，错误信息：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public static SqlDataReader GetReader(string sqlORproname, SqlParameter[] param, bool isProcedure)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand(sqlORproname, conn);
            if (isProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            try
            {
                conn.Open();
                cmd.Parameters.AddRange(param);
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                //将错误信息写入日志
                WriteLog("执行GetReader(string sqlORproname, SqlParameter[] param, bool isProcedure)方法时发生异常，错误信息：" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 返回dataset，存储过程，带参数
        /// </summary>
        /// <param name="procedureName"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataSet GetDataSetByProcedure(string procedureName, SqlParameter[] param)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                conn.Open();
                if (param != null)
                {
                    cmd.Parameters.AddRange(param);
                }
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //写入错误日志
                WriteLog("执行GetDataSetByProcedure(string procedureName,SqlParameter[] param)时出现错误：" + ex.Message);
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region 批量插入数据

        /// <summary>
        /// 往数据库中批量插入数据
        /// </summary>
        /// <param name="dtSource">数据源表</param>
        /// <param name="targetTable">服务器上目标表</param>
        public static void BulkToDB(DataTable dtSource, string targetTable)
        {
            SqlConnection conn = new SqlConnection(connString);
            SqlBulkCopy bulkCopy = new SqlBulkCopy(connString, SqlBulkCopyOptions.FireTriggers);   //用其它源的数据有效批量加载sql server表中
            bulkCopy.DestinationTableName = targetTable;    //服务器上目标表的名称
            bulkCopy.BulkCopyTimeout = 300;                //设置超时时间
            bulkCopy.BatchSize = dtSource.Rows.Count;   //每一批次中的行数

            try
            {
                conn.Open();
                if (dtSource != null && dtSource.Rows.Count != 0)
                    bulkCopy.WriteToServer(dtSource);   //将提供的数据源中的所有行复制到目标表中
            }
            catch (Exception e)
            {
                throw new Exception("系统出现异常，详细信息为：" + e.Message.ToString());
            }
            finally
            {
                conn.Close();
                if (bulkCopy != null)
                    bulkCopy.Close();
            }
        }
        #endregion

        #region  批量更新数据
        /// <param name="dt">准备更新的DataTable新数据源</param>
        /// <param name="TableName">对应要更新的数据库表名</param>
        /// <param name="primaryKeyName">对应要更新的数据库表的主键名</param>
        /// <param name="columnsName">对应要更新的列的列名集合</param>
        /// <param name="limitWhere">需要在SQL的Where条件中限定的条件字符串，可为空。</param>
        /// <param name="onceUpdateNumber">每次往返处理的行数</param>
        /// <returns>返回更新的行数</returns>
        public static int Update(DataTable dt, string TableName, string primaryKeyName, string[] columnsName, string limitWhere = "1=1", int onceUpdateNumber = 10000)
        {
            if (string.IsNullOrEmpty(TableName)) return 0;
            if (string.IsNullOrEmpty(primaryKeyName)) return 0;
            if (columnsName == null || columnsName.Length <= 0) return 0;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            int result = 0;
            using (SqlConnection sqlconn = new SqlConnection(connString))
            {
                sqlconn.Open();
                //使用加强读写锁事务   
                SqlTransaction tran = sqlconn.BeginTransaction(IsolationLevel.ReadCommitted);
                try
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        //所有行设为修改状态   
                        dr.SetModified();
                    }
                    //为Adapter定位目标表   
                    SqlCommand cmd = new SqlCommand(string.Format("select * from {0} currentNode {1}", TableName, limitWhere), sqlconn, tran);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    SqlCommandBuilder sqlCmdBuilder = new SqlCommandBuilder(da);
                    da.AcceptChangesDuringUpdate = false;
                    StringBuilder columnsUpdateSql = new StringBuilder();
                    SqlParameter[] SqlPara = new SqlParameter[columnsName.Length];
                    //需要更新的列设置参数是,参数名为"@+列名"
                    for (int i = 0; i < columnsName.Length; i++)
                    {
                        //此处拼接要更新的列名及其参数值
                        columnsUpdateSql.Append(string.Format(" [{0}] = @{0} ,", columnsName[i]));
                        SqlPara[i] = new SqlParameter("@" + columnsName[i], columnsName[i]);
                    }
                    if (columnsUpdateSql.Length > 0)
                    {
                        //此处去掉拼接处最后一个","
                        columnsUpdateSql.Remove(columnsUpdateSql.Length - 1, 1);
                    }
                    //此处生成where条件语句
                    string limitSql = string.Format("[{0}] = '@{0}' ", primaryKeyName);
                    SqlCommand updateCmd = new SqlCommand(string.Format(" UPDATE [{0}] SET {1} WHERE {2} ", TableName, columnsUpdateSql, limitSql));
                    //不修改源DataTable   
                    updateCmd.UpdatedRowSource = UpdateRowSource.None;
                    da.UpdateCommand = updateCmd;
                    da.UpdateCommand.Parameters.AddRange(SqlPara);
                    da.UpdateCommand.Parameters.Add(new SqlParameter("@" + primaryKeyName, primaryKeyName));
                    //每次往返处理的行数
                    da.UpdateBatchSize = onceUpdateNumber;
                    result = da.Update(ds, "default");
                    ds.AcceptChanges();
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                    throw new Exception("系统出现异常，详细信息为：" + e.Message.ToString());
                }
                finally
                {
                    sqlconn.Dispose();
                    sqlconn.Close();
                }
            }
            return result;
        }
        #endregion

        #region 获取数据库服务器时间 ，写入错误日志
        /// <summary>
        /// 获取数据服务器的时间
        /// </summary>
        /// <returns></returns>
        public static DateTime GetDBServerTime()
        {
            return Convert.ToDateTime(GetSingleResult("select getdate()"));
        }
        /// <summary>
        /// 写入错误日志
        /// </summary>
        /// <param name="msg"></param>
        private static void WriteLog(string msg)
        {
            FileStream fs = new FileStream("ProjectDemo.log", FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("【" + DateTime.Now.ToString() + "】" + "错误信息:" + msg);
            sw.Close();
            fs.Close();
        }
        #endregion

        #region 获取本机IP地址
        public static string GetIpAddress()
        {
            string hostName = Dns.GetHostName();   //获取本机名
            IPHostEntry localhost = Dns.GetHostByName(hostName);    //方法已过期，可以获取IPv4的地址
            //IPHostEntry localhost = Dns.GetHostEntry(hostName);   //获取IPv6地址
            IPAddress localaddr = localhost.AddressList[0];
            return localaddr.ToString();
        }
        #endregion
    }
}