using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;
using System.Data;

namespace NoivaModasMostra
{
    class Conexao
    {
        private string user;
        private string server;
        private string pass;
        private string dataBase;
        private string conectString;
        private bool sucessedOnLastLoad;
        public bool SucessedOnLastLoad
        {
            set { sucessedOnLastLoad = value; }
            get { return sucessedOnLastLoad;}
        }
        
        MySqlConnection conexao;

        public void _setUser(string novoUser)
        {
            this.user = novoUser;
            //_setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
        }
        public void _setServer(string novoServ)
        {
            this.server = novoServ;
            //_setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
        }
        public void _setPassword(string novoPass)
        {
            this.pass = novoPass;
            //_setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
        }
        public void _setDatabase(string novoDb)
        {
            this.dataBase = novoDb;
            
            //_setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
        }
        private void _setConStr(string novoConStr)
        {
            MySqlConnection con = new MySqlConnection("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";");
            con.Open();
            MySqlCommand command = new MySqlCommand("CREATE DATABASE IF NOT EXISTS " + this.getDatabase(), con);
            command.ExecuteNonQuery();
            if (con.State == ConnectionState.Open)
                this.conectString = novoConStr;
            con.Close();
        }
        private string _getConStr()
        {
            return this.conectString;
        }
        public string getUser()
        {
            return this.user;
        }

        public MySqlConnection getConnection()
        {
            return this.conexao;
        }

        public string getPass()
        {
            return this.pass;
        }

        public string getServer()
        {
            return this.server;
        }

        public string getDatabase()
        {
            return this.dataBase;
        }

        public Conexao()
        {
            try
            {
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                }
                else
                {
                    System.Windows.Forms.MessageBox.Show("Execute o aplicativo como administrador!");
                    
                }

                _setServer("");
                _setUser("root");
                _setPassword("33722363");
                _setDatabase("mostra2016");

                _setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
                conexao = new MySqlConnection(_getConStr());
                if (conexao.State== ConnectionState.Open)
                {
                    SucessedOnLastLoad = true;
                    CheckTables(conexao);
                }
                else
                    SucessedOnLastLoad = false;

                conexao.Close();
            }
            catch
            {
                SucessedOnLastLoad = false;
            }
        }
        

        public Conexao(string Host,string User,string Password,string Database)
        {
            try
            {
                _setServer(Host);
                _setUser(User);
                _setPassword(Password);
                _setDatabase(Database);
                _setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
                //testaCon.Open(); 
                conexao = new MySqlConnection(_getConStr());
                conexao.Open();
                //string erro = checkStrings();
                if (conexao.State== ConnectionState.Open)
                {
                    SucessedOnLastLoad = true;
                    CheckTables(conexao);
                    conexao.Close();
                }
                else
                {
                    SucessedOnLastLoad = false;
                    //System.Windows.Forms.MessageBox.Show(erro);
                }
            }
            catch (Exception ex)
            {
                SucessedOnLastLoad = false;
                if (conexao!= null)
                    conexao.Close();
                System.Windows.Forms.MessageBox.Show("Catch: "+ ex.Message);
                //System.Windows.Forms.MessageBox.Show("Erro no processo de conexão!:\n" + ex.Message + "\n\nContate o programador!");
                //System.Windows.Forms.Application.Exit();
            }
        }

        

        public Dictionary<string,string> get_cep(string cep)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();
            try
            {
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                Encoding utf8 = Encoding.UTF8;
                string cep5 = cep.Substring(0, 5);
                DataTable dt = new DataTable("uf");
                MySqlConnection con = new MySqlConnection("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=ceps");
                con.Open();
                MySqlDataAdapter mAdapter = new MySqlDataAdapter("select * from cep_log_index where cep5=\"" + cep5 + "\"", conexao);
                con.Close();
                mAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string uf = dt.Rows[0]["uf"].ToString();
                    DataTable dados = this.Query("select * from " + uf + " where cep=\"" + cep + "\"");
                    if (dados != null)
                    {
                        resultado.Add("id", iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(dados.Rows[0]["id"].ToString()))));
                        resultado.Add("cidade", iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(dados.Rows[0]["cidade"].ToString()))));
                        resultado.Add("logradouro", iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(dados.Rows[0]["logradouro"].ToString()))));
                        resultado.Add("bairro", iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(dados.Rows[0]["bairro"].ToString()))));
                        resultado.Add("cep", iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(dados.Rows[0]["cep"].ToString()))));
                        resultado.Add("tp_logradouro", iso.GetString(Encoding.Convert(utf8, iso, utf8.GetBytes(dados.Rows[0]["tp_logradouro"].ToString()))));
                        resultado.Add("uf", uf);
                    }
                }
                //_setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
                return resultado;
            }
            catch
            {
                _setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
                return resultado;
            }
        }

        public DataRowCollection get_cep2(int cep)
        {
            try
            {
                DataTable dt = new DataTable("ceps");

                string conStr = "server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=cep";
                MySqlConnection con = new MySqlConnection(conStr);
                con.Open();
                string query = "select * from cep where cep='" + cep.ToString() + "'";
                MySqlDataAdapter mAdapter = new MySqlDataAdapter(query, con);
                con.Close();
                mAdapter.Fill(dt);
                if (dt.Rows.Count == 0)
                    return null;
                else
                    return dt.Rows;
            }
            catch
            {
                _setConStr("server=" + this.server + ";user id=" + this.user + ";password=" + this.pass + ";database=" + this.dataBase);
                return null;
            }
        }

        public DataTable Query(string query)
        {
            try
            {
                string tableName = "";
                int index = query.IndexOf("FROM ");
                if (index != -1)
                {
                    tableName = query.Substring(index + 5, query.Length - 5 - index);
                    tableName = tableName.Substring(0, tableName.IndexOf(" "));
                }
                DataTable dt = new DataTable();
                dt.TableName = tableName;
                if (conexao == null)
                {
                    conexao = new MySqlConnection("server=" + server + ";user id=" + user + ";password=" + pass + ";database=" + dataBase);
                    conexao.Open();
                }
                if (this.conexao.State == ConnectionState.Closed)
                    conexao.Open();
                //query = MySqlHelper.EscapeString(query);*/
                MySqlDataAdapter mAdapter = new MySqlDataAdapter(query, conexao);
                mAdapter.Fill(dt);
                conexao.Close();
                if (dt.Rows.Count == 0)
                    return null;
                else
                {
                    if (dt.Columns.IndexOf("id") != -1)
                    {
                        DataColumn[] key = {dt.Columns[dt.Columns.IndexOf("id")]};
                        dt.PrimaryKey = key;
                    }
                    else
                        if (dt.Columns.IndexOf("id") != -1)
                        {
                            DataColumn[] key = { dt.Columns[dt.Columns.IndexOf("id")] };
                            dt.PrimaryKey = key;
                        }
                    return dt;
                }
            }
            catch
            {
                conexao.Close();
                return null;
            }
        }

        public string QueryMultiple(string[] queries)
        ///Select into List S1 and List S2 from  Database (2 fields)
        {
            try
            {
                using (var connection = new MySqlConnection(conectString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        for (int i = 0; i < queries.Length; i++)
                            queries[i] += "; ";

                        command.CommandText = string.Concat(queries);
                        int aux = 0;
                        using (var reader = command.ExecuteReader())
                        {
                            do
                            {
                                while (reader.Read())
                                {
                                    //aux = reader.GetInt32(0);
                                }
                                //System.Windows.Forms.MessageBox.Show(aux.ToString());
                            } while (reader.NextResult());

                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        /*
        public int proximoCodigo(string tabela, string chavePrimaria)
        {
            try
            {
                if (this.conexao.State != ConnectionState.Open)
                {
                    this.conexao.Open();
                }
                DataTable tb = new DataTable();
                MySqlDataAdapter apt = new MySqlDataAdapter("select " + chavePrimaria + " from " + tabela, this.conexao);
                apt.Fill(tb);
                if (tb.Rows.Count > 0)
                {
                    DataColumn colCod = tb.Columns[chavePrimaria];
                    DataColumn[] keys = { colCod };
                    tb.PrimaryKey = keys;
                    int aux = 1;
                    DataRow row;
                    for (int i = 1; i <= tb.Rows.Count; i++)
                    {
                        aux = i;
                        row = tb.Rows.Find(i);
                        if (row == null)
                        {
                            return i;
                        }
                    }
                    aux++;
                    this.conexao.Close();
                    return aux;
                }
                else
                {
                    this.conexao.Close();
                    return 1;
                }
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show("procedure 'proximoCodigo'exception: "+e.Message);
                this.conexao.Close();
                return 1;
            }
        }
        */
        public string comandoMysql(string query)
        {
            try
            {
                //this.checkStrings();
                if (this.conexao.State == ConnectionState.Closed)
                    this.conexao.Open();
                //query = MySqlHelper.EscapeString(query);
                MySqlCommand comando = new MySqlCommand(query, conexao);
                comando.ExecuteNonQuery();
                Arquivos arc = new Arquivos();
                string log = arc.criaLog(query, "comandoMysql");
                this.conexao.Close();
                if (log != "")
                    return "Erro de Log: " + log;
                else
                    return "";
            }
            catch (Exception es)
            {
                this.conexao.Close();
                return es.Message;
            }
        }

        public bool CheckTables(MySqlConnection con)
        {
            try
            {
                if (con.State== ConnectionState.Open)
                {

                    Dictionary<string, string> createString = new Dictionary<string, string>(){
                        {"tipo_local","CREATE TABLE IF NOT EXISTS tipo_local ("+
                        "id INT NOT NULL AUTO_INCREMENT, "+
                        "nome VARCHAR(45) NULL, "+
                        "PRIMARY KEY (id)) ENGINE = InnoDB"
                        },
                        { "local","CREATE TABLE IF NOT EXISTS local ( "+
                        "id INT NOT NULL AUTO_INCREMENT,"+"tipo INT NULL, "+
                        "nome VARCHAR(90) NULL,"+
                        "cep VARCHAR(9) NULL, "+
                        "endereco VARCHAR(90) NULL, "+
                        "numero VARCHAR(5) NULL, "+
                        "cidade VARCHAR(70) NULL, "+
                        "bairro VARCHAR(70) NULL, "+
                        "estado VARCHAR(2) NULL,PRIMARY KEY (id),INDEX tipoFK_idx (tipo ASC), CONSTRAINT tipoFK FOREIGN KEY (tipo)"+
                        "REFERENCES tipo_local (id)    ON DELETE NO ACTION    ON UPDATE NO ACTION)ENGINE = InnoDB"},

                        {"pessoa","CREATE TABLE IF NOT EXISTS pessoa (id INT NOT NULL AUTO_INCREMENT, sexo TINYINT(1) NULL, cpf VARCHAR(14) NOT NULL, nome VARCHAR(80) NULL,"+
                        "celular VARCHAR(14) NULL, fixo VARCHAR(14) NULL," +
                        "email VARCHAR(70) NULL, facebook VARCHAR(70) NULL, endereco_id INT NULL, PRIMARY KEY (id, cpf),"+
                        "INDEX endFk_idx (endereco_id ASC), CONSTRAINT endFk  FOREIGN KEY (endereco_id)"+
                        " REFERENCES local (id)   ON DELETE NO ACTION    ON UPDATE NO ACTION) ENGINE = InnoDB" },

                        {"tipo_evento","CREATE TABLE IF NOT EXISTS tipo_evento (id INT NOT NULL AUTO_INCREMENT, nome VARCHAR(45) NULL, PRIMARY KEY (id)) ENGINE = InnoDB" },

                        {"evento","CREATE TABLE IF NOT EXISTS evento (id INT NOT NULL AUTO_INCREMENT,tipo INT NULL,data DATE NULL, ele INT NULL,ela INT NULL,"+
                        "religioso INT NULL, festa INT NULL,  PRIMARY KEY (id), INDEX ele_FK_idx (ele ASC),"+
                        " INDEX local_FK_idx (religioso ASC), INDEX ela_FK_idx (ela ASC), INDEX festa_idx (festa ASC), CONSTRAINT ele_FK"+
                        " FOREIGN KEY(ele) REFERENCES pessoa (id) ON DELETE NO ACTION  ON UPDATE NO ACTION, "+
                        "CONSTRAINT religioso_FK FOREIGN KEY (religioso) REFERENCES local (id) ON DELETE NO ACTION ON UPDATE NO ACTION,"+
                        "CONSTRAINT ela_FK FOREIGN KEY (ela) REFERENCES pessoa (id) ON DELETE NO ACTION ON UPDATE NO ACTION,"+
                        "CONSTRAINT festa_FK FOREIGN KEY (festa) REFERENCES local (id) ON DELETE NO ACTION ON UPDATE NO ACTION) ENGINE = InnoDB" },
                    };
                    Dictionary<string, List<string>> firstInsert = new Dictionary<string, List<string>>(){
                        {"tipo_evento", new List<string>(){
                            {"insert into tipo_evento values (0,'N/A')"},
                            {"insert into tipo_evento values (0,'Casamento')"},
                            {"insert into tipo_evento values (0,'Formatura')"},
                            {"insert into tipo_evento values (0,'Aniversario')"},
                            {"insert into tipo_evento values (0,'Outro')"},
                        }},
                        {"tipo_local", new List<string>(){
                            {"insert into tipo_local values (0,'N/A')"},
                            {"insert into tipo_local values (0,'Residencia')"},
                            {"insert into tipo_local values (0,'Igreja')"},
                            {"insert into tipo_local values (0,'Salão de Festa')"},
                            {"insert into tipo_local values (0,'Fazenda')"},
                            {"insert into tipo_local values (0,'Chacara')"},
                            {"insert into tipo_local values (0,'Outro')"},
                        }},

                    };
                    Dictionary<string, List<string>> fk_Insert = new Dictionary<string, List<string>>()
                    {
                    };
                    DataTable dt = new DataTable();
                    MySqlDataAdapter myAdap = new MySqlDataAdapter("show tables", con);
                    MySqlCommand comando;
                    myAdap.Fill(dt);
                    if (dt.Rows.Count != 0)
                    {
                        DataColumn[] key = {dt.Columns["Tables_in_"+this.getDatabase()]};
                        dt.PrimaryKey = key;
                        foreach( KeyValuePair<string,string> pair in createString)
                        {
                            if (!dt.Rows.Contains(pair.Key))
                            {
                                comando = new MySqlCommand(pair.Value, con);
                                comando.ExecuteNonQuery();
                                if (firstInsert.ContainsKey(pair.Key))
                                {
                                    foreach (string insertString in firstInsert[pair.Key])
                                    {
                                        //query = MySqlHelper.EscapeString(query);
                                        comando = new MySqlCommand(insertString, con);
                                        comando.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (KeyValuePair<string, string> pair in createString)
                        {
                            comando = new MySqlCommand(pair.Value, con);
                            comando.ExecuteNonQuery();
                            if (firstInsert.ContainsKey(pair.Key))
                            {
                                foreach (string insertString in firstInsert[pair.Key])
                                {
                                    comando = new MySqlCommand(insertString, con);
                                    comando.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    this.conexao.Close();
                    return true;
                }
                else
                    return false;
            }
            catch(Exception er)
            {
                this.conexao.Close();
                System.Windows.Forms.MessageBox.Show(er.Message);
                return false;
            }
        }
    }
}
