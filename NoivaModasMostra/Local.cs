using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace NoivaModasMostra
{
    class Local
    {
        public int ID { get; set; }
        public TipoLocal TIPO { get; set; }

        public string CEP { get; set; }
        public string NOME { get; set; }
        public string ENDERECO { get; set; }
        public string NUMERO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string ESTADO { get; set; }

        /// <summary>
        /// aaaa!
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static Local Last(MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from local order by id desc limit 1", con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Local ret = new Local();
                    ret.TIPO = TipoLocal.load((int)dt.Rows[0]["tipo"], con);
                    con.Close();
                    ret.ID = (int)dt.Rows[0]["id"];
                    ret.NUMERO = dt.Rows[0]["numero"].ToString();
                    ret.CEP = dt.Rows[0]["cep"].ToString();
                    ret.BAIRRO = dt.Rows[0]["bairro"].ToString();
                    ret.CIDADE = dt.Rows[0]["cidade"].ToString();
                    ret.ESTADO = dt.Rows[0]["estado"].ToString();
                    ret.ENDERECO = dt.Rows[0]["endereco"].ToString(); ;
                    return ret;
                }
                else
                {
                    con.Close();
                    return null;
                }
            }
            catch
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }

        public static Local New(int tipo, string nome, string cep, string endereco, string numero, string bairro, string cidade, string estado, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("insert into local (tipo,nome,cep,endereco,numero,bairro,cidade,estado) values (" +
                    tipo.ToString() + ",'" +
                    MySqlHelper.EscapeString(nome) + "','" +
                    MySqlHelper.EscapeString(cep) + "','" +
                    MySqlHelper.EscapeString(endereco) + "','" +
                    MySqlHelper.EscapeString(numero) + "','" +
                    MySqlHelper.EscapeString(bairro) + "','" +
                    MySqlHelper.EscapeString(cidade) + "','" +
                    MySqlHelper.EscapeString(estado) + "')", con);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    con.Close();
                    return Local.Last(con);
                }
                else
                {
                    con.Close();
                    return null;
                }

            }
            catch(Exception e)
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }
        public static Local Load(int id, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from local where id=" + id.ToString(), con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Local ret = new Local();
                    ret.ID = id;
                    ret.CEP = dt.Rows[0]["cep"].ToString();
                    ret.TIPO = TipoLocal.load((int)dt.Rows[0]["tipo"], con);
                    ret.NOME= dt.Rows[0]["nome"].ToString();
                    ret.ENDERECO = dt.Rows[0]["endereco"].ToString();
                    ret.CIDADE = dt.Rows[0]["cidade"].ToString();
                    ret.BAIRRO = dt.Rows[0]["bairro"].ToString();
                    ret.ESTADO = dt.Rows[0]["estado"].ToString();
                    ret.NUMERO = dt.Rows[0]["numero"].ToString();
                    con.Close();
                    return ret;
                }
                else
                {
                    con.Close();
                    return null;
                }
            }
            catch
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }
        public static bool Update(int id, int tipo, string nome, string cep, string endereco, string numero, string bairro, string cidade, string estado, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("update local set tipo=" +
                    tipo.ToString() + ",nome='" +
                    nome + "',cep='" +
                    cep + "',endereco='" +
                    endereco + "',numero='" +
                    numero + "',bairro='" +
                    bairro + "',cidade='" +
                    cidade + "',estado='" +
                    estado + "' WHERE id=" + id.ToString(), con);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    con.Close();
                    return true;
                }
                else
                {
                    con.Close();
                    return false;
                }

            }
            catch (Exception e)
            {
                if (con != null)
                    con.Close();
                return false;
            }
        }

        public static Dictionary<int, string> GetAll(MySqlConnection con)
        {
            Dictionary<int, string> dic = new Dictionary<int, string>();
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from local order by id asc", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 1)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        if ((int)r["tipo"]!=1)
                            dic.Add((int)r["id"], r["nome"].ToString());
                    }
                }
                return dic;

            }
            catch
            {
                if (con != null)
                    con.Close();
                return dic;
            }
        }
    }

    class TipoLocal
    {
        public int ID { get; set; }
        public string NOME { get; set; }

        public static List<TipoLocal> getAll(MySqlConnection con)
        {
            List<TipoLocal> all = new List<TipoLocal>();
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from tipo_local order by id asc",con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                con.Close();
                if (dt.Rows.Count>0)
                {
                    TipoLocal tl;
                    foreach (DataRow  r in dt.Rows)
                    {
                        tl = new TipoLocal();
                        tl.ID = (int)r["id"];
                        tl.NOME = r["nome"].ToString();
                        all.Add(tl);
                    }
                }
                return all;
            }
            catch
            {
                if (con != null)
                    con.Close();
                return all;
            }
        }

        public static TipoLocal load(int id, MySqlConnection con)
        {
            TipoLocal NA = new TipoLocal();
            NA.ID = 0;
            NA.NOME = "N/A";
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from tipo_local where id=" + id.ToString(), con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    TipoLocal ret = new TipoLocal();
                    ret.ID = id;
                    ret.NOME = dt.Rows[0]["nome"].ToString();
                    con.Close();
                    return ret;
                }
                else
                {
                    con.Close();
                    return NA;
                }
            }
            catch
            {
                if (con != null)
                    con.Close();
                return NA;
            }
        }

    }
}
