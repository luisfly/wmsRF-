using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;
using System.Data;
using System.Collections;
using System.Data.Common;

namespace TJ.WMS.RF.Service
{
    /// <summary>
    /// SqlServer数据库操作辅助类
    /// </summary>
    public class SqlDbHelper
    {
        private string connectionString = string.Empty;
        private int commandTimeout = 180;
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }
        /// <summary>
        /// SQL语句执行超时时间
        /// </summary>
        public int CommandTimeout
        {
            get { return commandTimeout; }
            set { commandTimeout = value; }
        }
        /// <summary>
        /// 全局共享事务
        /// </summary>
        protected SqlTransaction Transaction
        {
            get
            {
                try
                {
                    return CallContext.GetData("Global_SqlTransaction") as SqlTransaction;
                }
                catch
                {
                    return null;
                }
            }
        }
        /// <summary>
        /// 全局共享连接
        /// </summary>
        protected SqlConnection Connection
        {
            get
            {
                try
                {
                    SqlConnection conn = CallContext.GetData("Global_SqlConnection") as SqlConnection;
                    if (conn != null && conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }

                    return conn;
                }
                catch
                {
                    return null;
                }
            }
        }
        #region Internal

        /// <summary>
        /// 创建连接
        /// </summary>
        /// <param name="connString"></param>
        /// <returns></returns>
        protected SqlConnection CreateConnection(string connString)
        {
            if (string.IsNullOrEmpty(connString))
            {
                if (string.IsNullOrEmpty(this.ConnectionString))
                    throw new ArgumentNullException("ConnectionString");
                connString = this.ConnectionString;
            }
            connString = connString + ";User ID=TJMIS;Password=TongJian@2014";//20141227
            SqlConnection conn = new SqlConnection(connString);
            return conn;
        }
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        protected SqlConnection OpenNew(string connString, bool isShare)
        {
            SqlConnection conn = null;

            try
            {
                conn = CreateConnection(connString);
                conn.Open();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //设置全局共享连接
            if (isShare)
            {
                CallContext.FreeNamedDataSlot("Global_SqlConnection");
                CallContext.SetData("Global_SqlConnection", conn);
            }

            return conn;
        }

        #endregion
        #region 打开/关闭数据库连接

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <returns></returns>
        public bool Close()
        {
            try
            {
                SqlConnection conn = CallContext.GetData("Global_SqlConnection") as SqlConnection; ;
                if (conn != null && conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                CallContext.FreeNamedDataSlot("Global_SqlTransaction");
                CallContext.FreeNamedDataSlot("Global_SqlConnection");
                return true;
            }
            catch (Exception ex)
            {
                CallContext.FreeNamedDataSlot("Global_SqlTransaction");
                CallContext.FreeNamedDataSlot("Global_SqlConnection");
                throw ex;
            }
        }
        private bool Close(IDbConnection conn)
        {
            try
            {
                if (conn != null && conn.State != ConnectionState.Closed)
                    conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }

        #endregion
        #region 事务处理

        public void BeginTrans()
        {
            try
            {
                SqlConnection conn = Connection;
                if (conn == null)
                    conn = OpenNew(this.connectionString, true);

                SqlTransaction trans = conn.BeginTransaction();
                CallContext.SetData("Global_SqlTransaction", trans);
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public void BeginTrans(IsolationLevel iso)
        {
            try
            {
                SqlConnection conn = Connection;
                if (conn == null)
                    conn = OpenNew(this.connectionString, true);

                SqlTransaction trans = conn.BeginTransaction(iso);
                CallContext.SetData("Global_SqlTransaction", trans);
            }
            catch (Exception ex)
            {
                Close();
                throw ex;
            }
        }
        public void Commit()
        {
            Commit(false);
        }
        public void Commit(bool keepConnection)
        {
            SqlTransaction trans = Transaction;
            if (trans != null)
            {
                try
                {
                    trans.Commit();
                    CallContext.FreeNamedDataSlot("Global_SqlTransaction");
                }
                catch
                {
                    trans.Rollback();
                    CallContext.FreeNamedDataSlot("Global_SqlTransaction");
                }
            }
            if (!keepConnection)
            {
                Close();
            }
        }
        public void RollBack()
        {
            try
            {
                SqlTransaction trans = Transaction;

                CallContext.FreeNamedDataSlot("Global_SqlTransaction");

                if (trans != null)
                    trans.Rollback();
            }
            catch
            {
                CallContext.FreeNamedDataSlot("Global_SqlTransaction");
                CallContext.FreeNamedDataSlot("Global_SqlConnection");
            }
            Close();
        }

        #endregion
        #region 执行SP/SQL

        public int ExcuteSQL(string commandText)
        {
            return ExcuteSQL(commandText, (DbParameter[])null);
        }
        public int ExcuteSQL(string commandText, params DbParameter[] parameters)
        {
            return ExcuteCmd(commandText, CommandType.Text, parameters);
        }
        public int ExcuteSP(string spName)
        {
            return ExcuteSP(spName, (DbParameter[])null);
        }
        public int ExcuteSP(string spName, params object[] paraValues)
        {
            if (paraValues != null)
            {
                SqlParameter[] commandParameters = SqlDbParameterCache.GetSpParameterSet(ConnectionString, spName, false);
                DbHelperUtil.AssignParameterValues(commandParameters, paraValues); ;
                return ExcuteSP(spName, commandParameters);
            }
            else
            {
                return ExcuteSP(spName, (DbParameter[])null);
            }
        }
        public int ExcuteSP(string spName, DataRow dataRow)
        {
            if (dataRow != null)
            {
                SqlParameter[] commandParameters = SqlDbParameterCache.GetSpParameterSet(ConnectionString, spName, false);
                DbHelperUtil.AssignParameterValues(commandParameters, dataRow); ;
                return ExcuteSP(spName, commandParameters);
            }
            else
            {
                return ExcuteSP(spName, (DbParameter[])null);
            }
        }
        public int ExcuteSP(string spName, params DbParameter[] parameters)
        {
            return ExcuteCmd(spName, CommandType.StoredProcedure, parameters);
        }

        public int ExcuteCmd(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            SqlTransaction trans = Transaction;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            bool needDispose = true;    //针对没有事务的情况

            try
            {
                cmd = new SqlCommand();
                cmd.CommandTimeout = CommandTimeout;

                if (trans == null)
                {
                    conn = this.Connection;

                    if (conn == null)
                    {
                        conn = OpenNew(this.connectionString, false);
                    }
                    else
                    {
                        needDispose = false;//采用共享的数据库连接，使用完毕后不需关闭
                    }

                    DbHelperUtil.PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, parameters);
                }
                else
                {
                    DbHelperUtil.PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, parameters);
                }

                int returnVal = cmd.ExecuteNonQuery();
                return returnVal;
            }
            catch (Exception ex)
            {
                RollBack();
                if (ex is SqlException && ((SqlException)ex).Number == 99999)
                {
                    throw new RFException(ex.Message);
                }
                throw ex;
            }
            finally
            {
                if (needDispose)
                    Close(conn);
                if (cmd != null)
                    cmd.Parameters.Clear();
            }
        }
      
        #endregion
        #region 获取单值

        /// <summary>
        /// 获取某一业务实体指定属性的值
        /// </summary>
        /// <typeparam name="T">业务实体类型</typeparam>
        /// <param name="propertyName">属性名称</param>
        /// <param name="conditionExp">条件表达式，如：#GID#='001'</param>
        /// <returns>返回object类型值，如果根据条件有多行多列的结果集，只取第一行第一列</returns>
        public object GetValue(string commandText, CommandType commandType)
        {
            return GetValue(commandText, commandType, (DbParameter[])null);
        }
        public object GetValue(string commandText, CommandType commandType, params object[] paraValues)
        {
            if (commandType == CommandType.StoredProcedure)
            {
                SqlParameter[] commandParameters = SqlDbParameterCache.GetSpParameterSet(ConnectionString, commandText, false);
                DbHelperUtil.AssignParameterValues(commandParameters, paraValues); ;
                return GetValue(commandText, commandType, commandParameters);
            }
            else
            {
                return GetValue(commandText, commandType, (DbParameter[])null);
            }
        }
        public object GetValue(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            SqlTransaction trans = Transaction;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            bool needDispose = true;    //针对没有事务的情况

            try
            {
                cmd = new SqlCommand();
                cmd.CommandTimeout = CommandTimeout;
                if (trans == null)
                {
                    conn = this.Connection;

                    if (conn == null)
                    {
                        conn = OpenNew(this.connectionString, false);
                    }
                    else
                    {
                        needDispose = false;//采用共享的数据库连接，使用完毕后不需关闭
                    }
                    DbHelperUtil.PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, parameters);
                }
                else
                {
                    DbHelperUtil.PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, parameters);
                }

                object obj = cmd.ExecuteScalar();//结果集中的第一行第一列
                return obj;
            }
            catch (Exception ex)
            {
                RollBack();
                if (ex is SqlException && ((SqlException)ex).Number >=50000)
                {
                    throw new RFException(ex.Message);
                }
                throw ex;
            }
            finally
            {
                if (needDispose)
                    Close(conn);
                if (cmd != null)
                    cmd.Parameters.Clear();
            }
        }
        public object GetValue(string spname, params object[] paraValues)
        {
            if (paraValues != null)
            {
                SqlParameter[] commandParameters = SqlDbParameterCache.GetSpParameterSet(ConnectionString, spname, false);
                DbHelperUtil.AssignParameterValues(commandParameters, paraValues); ;
                return GetValue(spname, CommandType.StoredProcedure, commandParameters);
            }
            else
            {
                return GetValue(spname, CommandType.StoredProcedure, (DbParameter)null);
            }
        }
        #endregion
        #region 获取DataSet

        public DataSet GetDataSet(string spName)
        {
            return GetDataSet(spName, (DbParameter[])null);
        }
        public DataSet GetDataSet(string spName, DataRow dataRow)
        {
            if (dataRow != null)
            {
                SqlParameter[] commandParameters = SqlDbParameterCache.GetSpParameterSet(ConnectionString, spName, false);
                DbHelperUtil.AssignParameterValues(commandParameters, dataRow); ;
                return GetDataSet(spName, commandParameters);
            }
            else
            {
                return GetDataSet(spName, (DbParameter[])null);
            }
        }
        public DataSet GetDataSet(string spName, params object[] paraValues)
        {
            if (paraValues != null)
            {
                SqlParameter[] commandParameters = SqlDbParameterCache.GetSpParameterSet(ConnectionString, spName, false);
                DbHelperUtil.AssignParameterValues(commandParameters, paraValues); ;
                return GetDataSet(spName, commandParameters);
            }
            else
            {
                return GetDataSet(spName, (DbParameter[])null);
            }
        }
        public DataSet GetDataSet(string spName, params DbParameter[] parameters)
        {
            return GetDataSet_Cmd(spName, CommandType.StoredProcedure, parameters);
        }

        public DataSet GetDataSet_SQL(string commandText)
        {
            return GetDataSet_SQL(commandText, (DbParameter[])null);
        }
        public DataSet GetDataSet_SQL(string commandText, params DbParameter[] parameters)
        {
            return GetDataSet_Cmd(commandText, CommandType.Text, parameters);
        }

        public DataSet GetDataSet_Cmd(string commandText, CommandType commandType, params DbParameter[] parameters)
        {
            SqlTransaction trans = Transaction;
            SqlConnection conn = null;
            SqlCommand cmd = null;
            bool needDispose = true;    //针对没有事务的情况

            try
            {
                cmd = new SqlCommand();
                cmd.CommandTimeout = CommandTimeout;
                if (trans == null)
                {
                    conn = this.Connection;

                    if (conn == null)
                    {
                        conn = OpenNew(this.connectionString, false);
                    }
                    else
                    {
                        needDispose = false;//采用共享的数据库连接，使用完毕后不需关闭
                    }
                    DbHelperUtil.PrepareCommand(cmd, conn, (SqlTransaction)null, commandType, commandText, parameters);
                }
                else
                {
                    DbHelperUtil.PrepareCommand(cmd, trans.Connection, trans, commandType, commandText, parameters);
                }

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                RollBack();
                if (ex is SqlException && ((SqlException)ex).Number == 99999)
                {
                    throw new RFException(ex.Message);
                }
                throw ex;
            }
            finally
            {
                if (needDispose)
                    Close(conn);
                if (cmd != null)
                    cmd.Parameters.Clear();
            }
        }

        #endregion
    }

    /// <summary>
    /// SQL Server 参数缓存
    /// </summary>
    public class SqlDbParameterCache
    {
        /// <summary>
        /// 存储过程的参数缓存集合
        /// </summary>
        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        #region Internal

        /// <summary>
        /// 对SqlParameter数组进行克隆(深度拷贝)
        /// </summary>
        /// <param name="originalParameters">源参数列表</param>
        /// <returns>返回进行克隆后得到的参数列表</returns>
        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] clonedParameters = new SqlParameter[originalParameters.Length];
            for (int i = 0, j = originalParameters.Length; i < j; i++)
            {
                clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
            }
            return clonedParameters;
        }
        /// <summary>
        /// 为指定的存储过程获取参数列表
        /// </summary>
        /// <param name="connection">有效的SqlConnection对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">指定在是否获取返回参数</param>
        /// <returns>返回获取到的SqlParameter列表</returns>
        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0)
                throw new ArgumentNullException("spName");
            string hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter ? ":include ReturnValue Parameter" : "");
            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
            {
                SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);
                paramCache[hashKey] = spParameters;
                cachedParameters = spParameters;
            }

            return CloneParameters(cachedParameters); // 这里返回的是从缓存集合里获取到的参数列表的一个Clone版本
        }
        /// <summary>
        /// 查找存储过程的参数信息，并进行缓存
        /// </summary>
        /// <param name="connection">SqlConnection连接对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">指定获取的参数列表中是否包含返回参数信息</param>
        /// <returns>存储过程的参数信息列表</returns>
        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            if (spName == null || spName.Length == 0)
                throw new ArgumentNullException("spName");
            SqlCommand cmd = new SqlCommand(spName, connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            if (!includeReturnValueParameter)
            { //返回参数位于第0个位置
                cmd.Parameters.RemoveAt(0);
            }
            SqlParameter[] discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);//将Command对象的参数信息拷贝到返回列表中                
            foreach (SqlParameter p in discoveredParameters)
            {// 将所有的参数的默认值初始值赋为DBNull
                p.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        #endregion

        /// <summary>
        /// 将参数信息添加到缓冲集合中。采用connectionString和commandText的组合作为Key键
        /// </summary>
        /// <param name="connectionString">有效的SqlConnection对象</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <param name="commandParameters">用于缓存的SqlParamters参数列表</param>
        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            string hashKey = connectionString + ":" + commandText;
            paramCache[hashKey] = commandParameters;
        }
        /// <summary>
        /// 从参数缓存集合中获取参数列表信息
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="commandText">存储过程名称或SQL语句</param>
        /// <returns>返回获取到参数列表</returns>
        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            string hashKey = connectionString + ":" + commandText;
            SqlParameter[] cachedParameters = paramCache[hashKey] as SqlParameter[];
            if (cachedParameters == null)
                return null;
            else
                return CloneParameters(cachedParameters);
        }
        /// <summary>
        /// 为指定的存储过程获取参数列表。该方法默认不返回存储过程的返回参数信息
        /// </summary>
        /// <remarks>
        /// 该方法将先从参数缓存集合中查找指定的存储过程的参数，若参数缓存中没有指定的存储过程的参数信息，
        /// 则从数据库中查找指定的存储过程的参数，并将获取到的参数存入参数缓存集合中          
        /// </remarks>
        /// <param name="connection">有效的SqlConnection对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">指定在是否获取返回参数</param>
        /// <returns>返回获取到的SqlParameter列表</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            return GetSpParameterSet(connectionString, spName, false);
        }
        /// <summary>
        /// 为指定的存储过程获取参数列表
        /// </summary>
        /// <remarks>
        /// 该方法将先从参数缓存集合中查找指定的存储过程的参数，若参数缓存中没有指定的存储过程的参数信息，
        /// 则从数据库中查找指定的存储过程的参数，并将获取到的参数存入参数缓存集合中
        /// </remarks>
        /// <param name="connection">有效的SqlConnection对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">指定在是否获取返回参数</param>
        /// <returns>返回获取到的SqlParameter列表</returns>
        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            if (connectionString == null || connectionString.Length == 0)
                throw new ArgumentNullException("connectionString");
            if (spName == null || spName.Length == 0)
                throw new ArgumentNullException("spName");

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                return GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
        }
        /// <summary>
        /// 为指定的存储过程获取参数列表。该方法默认不返回存储过程的返回参数信息
        /// </summary>
        /// <remarks>
        /// 该方法将先从参数缓存集合中查找指定的存储过程的参数，若参数缓存中没有指定的存储过程的参数信息，
        /// 则从数据库中查找指定的存储过程的参数，并将获取到的参数存入参数缓存集合中          
        /// </remarks>
        /// <param name="connection">有效的SqlConnection对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">指定在是否获取返回参数</param>
        /// <returns>返回获取到的SqlParameter列表</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            return GetSpParameterSet(connection, spName, false);
        }
        /// <summary>
        /// 为指定的存储过程获取参数列表
        /// </summary>
        /// <remarks>
        /// 该方法将先从参数缓存集合中查找指定的存储过程的参数，若参数缓存中没有指定的存储过程的参数信息，
        /// 则从数据库中查找指定的存储过程的参数，并将获取到的参数存入参数缓存集合中
        /// </remarks>
        /// <param name="connection">有效的SqlConnection对象</param>
        /// <param name="spName">存储过程名称</param>
        /// <param name="includeReturnValueParameter">指定在是否获取返回参数</param>
        /// <returns>返回获取到的SqlParameter列表</returns>
        internal static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null)
                throw new ArgumentNullException("connection");
            using (SqlConnection clonedConnection = (SqlConnection)((ICloneable)connection).Clone())
            {
                return GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
        }
    }

    /// <summary>
    /// DbHelper公共处理类
    /// </summary>
    public class DbHelperUtil
    {
        #region Command
        /// <summary>
        /// 构建DbCommand
        /// </summary>
        /// <param name="command">DbCommand</param>
        /// <param name="connection">DbConnection</param>
        /// <param name="transaction">DbTransaction</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="commandText">命令脚本</param>
        /// <param name="commandParameters">命令参数</param>
        internal static void PrepareCommand(DbCommand command, DbConnection connection, DbTransaction transaction,
    CommandType commandType, string commandText, DbParameter[] commandParameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0)
                throw new ArgumentNullException("commandText");
            if (connection.State != ConnectionState.Open)
                connection.Open();

            command.Connection = connection;
            command.CommandType = commandType;
            command.CommandText = commandText;

            if (transaction != null)
            {
                if (transaction.Connection == null)
                    throw new ArgumentException("transaction已提交或回滚, 请提供一个打开的transaction.", "transaction");
                command.Transaction = transaction;
            }

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
        }
        
        /// <summary>
        /// 为指定的DbCommand对象指定参数列表
        /// </summary>
        /// <param name="command">需要添加参数的DbCommand对象</param>
        /// <param name="commandParameters">用于添加的参数列表。该列表包括参数值</param>
        internal static void AttachParameters(DbCommand command, DbParameter[] commandParameters)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (DbParameter p in commandParameters)
                {
                    if (p != null&& !command.Parameters.Contains(p))
                    {
                        
                        if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input)
                            && (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }
        /// <summary>
        /// 将存储在DataRow中的数据作为参数值赋值到指定的参数列表。
        /// </summary>
        /// <remarks>该方法将用参数列表中的参数名称与DataRow的Column的名称进行匹配赋值</remarks>
        /// <param name="commandParameters">需要赋值的参数列表</param>
        /// <param name="dataRow">存储参数值的DataRow对象</param>
        internal static void AssignParameterValues(DbParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
                return;

            int i = 1;
            foreach (DbParameter p in commandParameters)
            {
                if (p.ParameterName == null || p.ParameterName.Length <= 1)
                    throw new Exception(string.Format("第{0}个参数名称不可用, 该参数的值为:'{1}'.", i, p.ParameterName));
                ////约定：存储过程的参数名称命名方式为：@+字段名,如：@firstname
                string colName = p.ParameterName.Substring(1);//去掉符号“@”
                if (dataRow.Table.Columns.Contains(colName))
                {
                    p.Value = dataRow[colName];
                    if (p.DbType == DbType.DateTime && p.Value != DBNull.Value && Convert.ToDateTime(p.Value) == new DateTime(1900, 1, 1))
                        p.Value = DBNull.Value;
                }
                i++;
            }
        }
        /// <summary>
        /// 将指定的值列表数据作为参数值赋值到指定的参数列表
        /// </summary>
        /// <param name="commandParameters">需要赋值的参数列表</param>
        /// <param name="parameterValues">将作为参数值的值列表</param>
        internal static void AssignParameterValues(DbParameter[] commandParameters, object[] paramValues)
        {
            if ((commandParameters == null) || (paramValues == null))
                return;

            int j = commandParameters.Length;

            for (int i = 0; i < j; i++)
            {
                if (i >= paramValues.Length)
                {
                    commandParameters[i].Value = DBNull.Value;
                }//当参数值的个数小于参数个数时，补上DBNull.Value
                else if (paramValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)paramValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (paramValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = paramValues[i];
                }
            }
        }
        #endregion
    }

    /// <summary>
    /// IDbHelper对象工厂
    /// </summary>
    public class DbHelperFactory
    {

        /// <summary>
        /// 获取IDbHelper实例
        /// </summary>
        /// <param name="assemblyname"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public static SqlDbHelper GetDbHelper()
        {
            return GetDbHelper(null);
        }

        /// <summary>
        /// 获取IDbHelper实例
        /// </summary>
        /// <param name="helperType"></param>
        /// <param name="connString"></param>
        /// <returns></returns>
        public static SqlDbHelper GetDbHelper(string connString)
        {
            SqlDbHelper helper = new SqlDbHelper();
            SetConnectionString(helper, connString);

            return helper;
        }

        /// <summary>
        /// 设置DbHelper数据库连接字符串
        /// </summary>
        /// <param name="helper"></param>
        public static void SetConnectionString(SqlDbHelper helper, string connString)
        {
            if (helper == null)
                return;

            DbConnectionStringBuilder sb = new SqlConnectionStringBuilder(connString);
            helper.ConnectionString = sb.ConnectionString;
        }
    }
}
