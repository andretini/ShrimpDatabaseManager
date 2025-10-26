using System;
using System.Data;

namespace ShrimpDatabaseManager.Adapters
{
    /// <summary>
    /// Uma implementação "fake" da interface IDbConnection, usada apenas para testes.
    /// Não realiza nenhuma conexão real com banco de dados.
    /// </summary>
    public class DummyDbConnection : IDbConnection
    {
        public string ConnectionString { get; set; } = string.Empty;
        public int ConnectionTimeout => 0;
        public string Database => "InMemoryDB";
        public ConnectionState State { get; private set; } = ConnectionState.Closed;

        public IDbTransaction BeginTransaction()
        {
            return new DummyDbTransaction(this);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return new DummyDbTransaction(this);
        }

        public void ChangeDatabase(string databaseName) { }

        public void Close() => State = ConnectionState.Closed;

        public IDbCommand CreateCommand()
        {
            return new DummyDbCommand(this);
        }

        public void Open() => State = ConnectionState.Open;

        public void Dispose() => Close();
    }

    /// <summary>
    /// Implementação dummy de IDbCommand.
    /// </summary>
    public class DummyDbCommand : IDbCommand
    {
        public string CommandText { get; set; } = string.Empty;
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; } = CommandType.Text;
        public IDbConnection Connection { get; set; }
        public IDataParameterCollection Parameters { get; } = new DummyDataParameterCollection();
        public IDbTransaction Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }

        public DummyDbCommand(IDbConnection connection)
        {
            Connection = connection;
        }

        public void Cancel() { }
        public IDbDataParameter CreateParameter() => new DummyDbParameter();
        public void Dispose() { }
        public int ExecuteNonQuery() => 0;
        public IDataReader ExecuteReader() => new DummyDataReader();
        public IDataReader ExecuteReader(CommandBehavior behavior) => new DummyDataReader();
        public object ExecuteScalar() => null;
        public void Prepare() { }
    }

    /// <summary>
    /// Implementação dummy de IDbTransaction.
    /// </summary>
    public class DummyDbTransaction : IDbTransaction
    {
        public IDbConnection Connection { get; }

        public DummyDbTransaction(IDbConnection connection)
        {
            Connection = connection;
        }

        public IsolationLevel IsolationLevel => IsolationLevel.Unspecified;

        public void Commit() { }
        public void Dispose() { }
        public void Rollback() { }
    }

    /// <summary>
    /// Implementação dummy de IDataReader.
    /// </summary>
    public class DummyDataReader : IDataReader
    {
        public int Depth => 0;
        public bool IsClosed => false;
        public int RecordsAffected => 0;
        public int FieldCount => 0;

        public void Close() { }
        public void Dispose() { }
        public bool GetBoolean(int i) => false;
        public byte GetByte(int i) => 0;
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length) => 0;
        public char GetChar(int i) => '\0';
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length) => 0;
        public IDataReader GetData(int i) => this;
        public string GetDataTypeName(int i) => string.Empty;
        public DateTime GetDateTime(int i) => DateTime.MinValue;
        public decimal GetDecimal(int i) => 0;
        public double GetDouble(int i) => 0;
        public Type GetFieldType(int i) => typeof(object);
        public float GetFloat(int i) => 0;
        public Guid GetGuid(int i) => Guid.Empty;
        public short GetInt16(int i) => 0;
        public int GetInt32(int i) => 0;
        public long GetInt64(int i) => 0;
        public string GetName(int i) => string.Empty;
        public int GetOrdinal(string name) => 0;
        public DataTable GetSchemaTable() => new();
        public string GetString(int i) => string.Empty;
        public object GetValue(int i) => null;
        public int GetValues(object[] values) => 0;
        public bool IsDBNull(int i) => true;
        public bool NextResult() => false;
        public bool Read() => false;
        public object this[int i] => null;
        public object this[string name] => null;
    }

    /// <summary>
    /// Implementação dummy de IDataParameterCollection.
    /// </summary>
    public class DummyDataParameterCollection : System.Collections.ArrayList, IDataParameterCollection
    {
        public bool Contains(string parameterName) => false;
        public int IndexOf(string parameterName) => -1;
        public void RemoveAt(string parameterName) { }
        public object this[string parameterName] { get => null; set { } }
    }

    public class DummyDbParameter : IDbDataParameter
    {
        public byte Precision { get; set; }
        public byte Scale { get; set; }
        public int Size { get; set; }
        public DbType DbType { get; set; }
        public ParameterDirection Direction { get; set; }
        public bool IsNullable => true;
        public string ParameterName { get; set; }
        public string SourceColumn { get; set; }
        public DataRowVersion SourceVersion { get; set; }
        public object Value { get; set; }
    }
}
