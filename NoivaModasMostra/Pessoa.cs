using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace NoivaModasMostra
{
    class Pessoa
    {
        public int ID { get; set; }

        public string CPF { get; set; }
        
        public bool SEXO { get; set; }

        public string NOME { get; set; }
        public string CELULAR { get; set; }
        public string TELEFONE { get; set; }

        public string EMAIL { get; set; }

        public string FACEBOOK { get; set; }

        public Local ENDERECO { get; set; }

        public static Pessoa Last(MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from pessoa order by id desc limit 1", con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Pessoa ret = new Pessoa();
                    ret.ID = (int)dt.Rows[0]["id"];
                    ret.SEXO = (bool)dt.Rows[0]["sexo"];
                    ret.NOME = dt.Rows[0]["nome"].ToString();
                    ret.CELULAR = dt.Rows[0]["celular"].ToString();
                    ret.TELEFONE = dt.Rows[0]["fixo"].ToString();
                    ret.FACEBOOK = dt.Rows[0]["facebook"].ToString();
                    ret.EMAIL = dt.Rows[0]["email"].ToString();
                    ret.CPF = dt.Rows[0]["cpf"].ToString();
                    ret.ENDERECO = Local.Load((int)dt.Rows[0]["endereco_id"], con);
                    return ret;
                }
                else
                    return null;
            }
            catch
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }


        public static Pessoa New ( bool sexo,string cpf, string nome, string celular, string telefone,string email, string face, Local endereco ,MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("insert into pessoa (sexo,cpf,nome,celular,fixo,email,facebook,endereco_id) values ('" +
                    Convert.ToInt32(sexo).ToString()+"','"+
                    MySqlHelper.EscapeString(cpf) + "','" +
                    MySqlHelper.EscapeString(nome) + "','" +
                    MySqlHelper.EscapeString(celular) + "','" +
                    MySqlHelper.EscapeString(telefone) + "','" +
                    MySqlHelper.EscapeString(email) + "','" +
                    MySqlHelper.EscapeString(face) + "'," + 
                    endereco.ID.ToString()+")", con);
                
                if (cmd.ExecuteNonQuery() > 0)
                { 
                    con.Close();
                    return Pessoa.Last(con);
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

        public static bool Update(int id,bool sexo,string cpf,string celular,string telefone,string nome, string email, string face, Local endereco, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("UPDATE pessoa set sexo='" +
                    Convert.ToInt32(sexo).ToString() + "',cpf='" +
                    MySqlHelper.EscapeString(cpf) + "',nome='" +
                    MySqlHelper.EscapeString(nome) + "',celular='" +
                    MySqlHelper.EscapeString(celular) + "',telefone='" +
                    MySqlHelper.EscapeString(telefone) + "',email='" +
                    MySqlHelper.EscapeString(email) + "',facebook'" +
                    MySqlHelper.EscapeString(face) + "',endereco_id=" +
                    endereco.ID.ToString() + " WHERE id="+id, con);

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

        public static Pessoa Load( int id,MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from pessoa where id="+id.ToString(), con);
                ad.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    Pessoa ret = new Pessoa();
                    ret.ID = id;
                    ret.SEXO = (bool)dt.Rows[0]["sexo"];
                    ret.NOME = dt.Rows[0]["nome"].ToString();
                    ret.CELULAR = dt.Rows[0]["celular"].ToString();
                    ret.TELEFONE = dt.Rows[0]["fixo"].ToString();
                    ret.CPF = dt.Rows[0]["cpf"].ToString();
                    ret.EMAIL = dt.Rows[0]["email"].ToString();
                    ret.FACEBOOK = dt.Rows[0]["facebook"].ToString();
                    ret.ENDERECO = Local.Load((int)dt.Rows[0]["endereco_id"], con);
                    return ret;
                }
                else
                    return null;
            }
            catch
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }
        public static Pessoa Load(string cpf_nome, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from pessoa where cpf='" + cpf_nome+"' or nome='"+ cpf_nome + "'", con);
                ad.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    Pessoa ret = new Pessoa();
                    ret.ID = (int)dt.Rows[0]["id"];
                    ret.FACEBOOK = dt.Rows[0]["facebook"].ToString();
                    ret.EMAIL = dt.Rows[0]["email"].ToString();
                    ret.CPF = cpf_nome;
                    ret.SEXO = (bool)dt.Rows[0]["sexo"];
                    ret.NOME = dt.Rows[0]["nome"].ToString();
                    ret.CELULAR = dt.Rows[0]["celular"].ToString();
                    ret.TELEFONE = dt.Rows[0]["fixo"].ToString();
                    ret.ENDERECO = Local.Load((int)dt.Rows[0]["endereco_id"], con);
                    return ret;
                }
                else
                    return null;
            }
            catch
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }

        public static List<string> getAllNames(MySqlConnection con)
        {
            List<string> all = new List<string>();
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from pessoas order by id asc", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    string tl;
                    foreach (DataRow r in dt.Rows)
                    {
                        tl = r["nome"].ToString();
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
    }
}
