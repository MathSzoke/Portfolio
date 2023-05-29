using Backend.Data.ErrorData;
using Npgsql;
using System.Data;
using System.Text;

namespace Backend.Data;
public class DatabaseExecs
{
    protected static NpgsqlConnection _connection = null!;
    protected static string _connectionString = "Host=localhost;User ID=postgres;Password=C0ntr0lB@s308122001;Port=5432;Database=PortfolioSite;Pooling=true;";
    protected static readonly object _lock = new();

    protected static NpgsqlConnection Connection
    {
        get
        {
            if (_connection == null || _connection.State == ConnectionState.Closed)
            {
                lock (_lock)
                {
                    if (_connection == null || _connection.State == ConnectionState.Closed)
                    {
                        _connection?.Close();
                        _connection = new NpgsqlConnection(_connectionString);
                        _connection.Open();
                    }
                }
            }
            return _connection;
        }
    }

    /// <summary>
    /// Executa uma query dinâmica no banco de dados PostgreSQL,
    /// permite executar qualquer procedure com os parâmetros desejáveis.<br></br><br></br>
    /// Segue a sintaxe para o parâmetro "parameters": @nomeProcedure, valorParametro.
    /// </summary>
    /// <param name="procedureName">Nome da procedure</param>
    /// <param name="parameters">(1 ou mais) parâmetros a serem passadas para a execução da procedure.</param>
    public static bool ExecProcedure(string procedureName, List<NpgsqlParameter> parameters)
    {
        try
        {
            using NpgsqlCommand command = new(procedureName, Connection);

            command.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters.ToArray());
            }

            command.ExecuteNonQuery();

            return true;
        }
        catch (NpgsqlException ex)
        {
            ErrorLog.LogError(ex);
            return false;
        }
    }

    /// <summary>
    /// Executa uma query dinâmica no banco de dados PostgreSQL,
    /// permite executar qualquis quer functions com os parâmetros desejáveis.<br></br><br></br>
    /// Segue a sintaxe para o parâmetro "parameters": ":nomeParametro", valorParametro. (O TIPO DE LISTA TEM QUE SER "List<NpgsqlParameter>")
    /// </summary>
    /// <param name="functionName">Nome da function</param>
    /// <param name="parameters">(1 ou mais) parâmetros a serem passadas para a execução da function.</param>
    public static DataTable? ExecFunction(string functionName, List<string> parameters)
    {
        try
        {
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();

            StringBuilder sql = new();
            sql.Append("SELECT " + functionName + "(");

            if (parameters != null)
            {
                int i = 0;
                foreach (string parameter in parameters)
                {
                    string columnNames = string.Format("\'{0}\'", parameter);
                    i++;
                    sql.Append(columnNames);
                    if (i < parameters.Count)
                    {
                        sql.Append(", ");
                    }
                }
            }
            sql.Append(')');

            string selectSql = sql.ToString();
            using NpgsqlCommand command = new(selectSql, _connection);
            command.CommandType = CommandType.Text;

            using NpgsqlDataAdapter adapter = new(command);
            DataTable data = new();
            adapter.Fill(data);
            _connection.Close();
            return data;
        }
        catch (NpgsqlException ex)
        {
            _connection.Close();
            ErrorLog.LogError(ex);
            return null;
        }
    }

    /// <summary>
    /// Executa uma query dinâmica no banco de dados,
    /// permite inserir dados nas colunas desejadas.<br></br><br></br>
    /// Segue a respectiva sintaxe do Dictionary: 
    /// { "columnName", "valueToInsert" }
    /// </summary>
    /// <param name="tableName">Nome da tabela onde sera inserido o dado</param>
    /// <param name="data">Nome da coluna que o dado irá ocupar e em seguida o valor do dado que será inserido na coluna.</param>
    public static void InsertData(string tableName, Dictionary<string, object> data)
    {
        try
        {
            // Abre a conexão com o banco de dados
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();

            string table = string.Format("\"{0}\"", tableName);
            // Build the SQL command string
            StringBuilder sb = new();
            sb.Append("INSERT INTO public." + table + " (");

            // Add the column names to the SQL command string
            int i = 0;
            foreach (string key in data.Keys)
            {
                i++;

                string columns = string.Format("\"{0}\"", key);
                sb.Append(columns);

                if (i < data.Keys.Count)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(") VALUES (");

            // Add the parameter placeholders to the SQL command string
            i = 0;
            foreach (string key in data.Keys)
            {
                object? value = data[key];

                if (value is string)
                {
                    value = "'" + value + "'";
                }
                else if (value is int)
                {
                    value = value.ToString();
                }
                else if (value is DateTime)
                {
                    value = "'" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }
                else value ??= value = "null";

                i++;
                sb.Append(value);
                if (i < data.Keys.Count)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(')');

            // Create the SQL command
            string insertSql = sb.ToString();
            using NpgsqlCommand command = new(insertSql, _connection);

            // Execute the SQL command
            command.ExecuteNonQuery();
            _connection.Close();
        }
        catch (NpgsqlException ex)
        {
            _connection.Close();
            ErrorLog.LogError(ex);
        }
    }

    /// <summary>
    /// Executa uma query dinâmica no banco de dados SQL Server,
    /// permite selecionar as colunas desejadas e a clausula WHERE.<br></br><br></br>
    /// Segue a sintaxe correta à ser usado no parâmetro "whereClause": "nomeColunaWhere = valorParaComparar"<br></br><br></br>
    /// O resultado deverá ser assim: <code>DatabaseExecs.ExecAnySelect("tableName", "columns", "columnNameWhere = valueToCompare")</code>
    /// </summary>
    /// <param name="tableName"><code>Nome da tabela onde sera feito a consulta</code></param>
    /// <param name="columns">Colunas desejadas</param>
    /// <param name="whereClause">Clausula WHERE da query</param>
    /// <returns>Retorna um DataTable com o resultado, que no caso é o resultado da query.</returns>
    public static DataTable? ExecAnySelect(string tableName, List<string> columns, string whereClause)
    {
        try
        {
            // Abre a conexão com o banco de dados
            _connection = new NpgsqlConnection(_connectionString);
            _connection.Open();

            // Build the SQL command string
            StringBuilder sb = new();
            sb.Append("SELECT ");

            // Add the column names to the SQL command string
            int i = 0;
            foreach (string column in columns)
            {
                string columnNames = string.Format("\"{0}\"", column);
                i++;
                sb.Append(columnNames);
                if (i < columns.Count)
                {
                    sb.Append(", ");
                }
            }

            string table = string.Format("\"{0}\"", tableName);
            sb.Append(" FROM public." + table);

            // Add the where clause to the SQL command string, if it exists
            if (!string.IsNullOrEmpty(whereClause))
            {
                sb.Append(" WHERE " + whereClause);
            }

            // Create the SQL command
            string selectSql = sb.ToString();
            using NpgsqlCommand command = new(selectSql, _connection);
            command.CommandType = CommandType.Text;

            using NpgsqlDataAdapter adapter = new(command);
            DataTable data = new();
            adapter.Fill(data);

            _connection.Close();
            return data;
        }
        catch (NpgsqlException ex)
        {
            _connection.Close();
            ErrorLog.LogError(ex);
            return null;
        }
    }
}
