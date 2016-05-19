using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace NoivaModasMostra
{
    class Evento
    {
        public int ID { get; set; }
        public DateTime DATA { set; get; }
        public TipoEvento TIPO { set; get; }

        public Pessoa Ele { get; set; }
        public Pessoa Ela { get; set; }
        public Local RELIGIOSO { get; set; }
        public Local FESTA { set; get; }

        public static Evento Last(MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from evento order by id desc limit 1", con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Evento ret = new Evento();
                    ret.ID = (int)dt.Rows[0]["id"];
                    ret.TIPO = TipoEvento.load((int)dt.Rows[0]["tipo"], con);
                    ret.DATA = (DateTime)dt.Rows[0]["data"];
                    ret.Ela = Pessoa.Load((int)dt.Rows[0]["ela"], con);
                    ret.Ele = Pessoa.Load((int)dt.Rows[0]["ele"], con);
                    ret.RELIGIOSO = Local.Load((int)dt.Rows[0]["religioso"], con);
                    ret.FESTA = Local.Load((int)dt.Rows[0]["festa"], con);
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

        public static Evento New(int tipo, DateTime data, int ele,int ela,int religioso,int festa, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("insert into evento (tipo,data,ele,ela,religioso,festa) values (" +
                    tipo.ToString() + ",'" +
                    data.ToString("yyyy-MM-dd") +"'," +
                    ele.ToString() + "," +
                    ela.ToString() + "," +
                    religioso.ToString() + "," +
                    festa.ToString() + ")", con);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    con.Close();
                    return Evento.Last(con);
                }
                else
                {
                    con.Close();
                    return null;
                }

            }
            catch (Exception e)
            {
                if (con != null)
                    con.Close();
                return null;
            }
        }

        public static bool Update(int id, int tipo, DateTime data, int ele, int ela, int religioso, int festa, MySqlConnection con)
        {
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlCommand cmd = new MySqlCommand("update evento set tipo=" +
                    tipo.ToString() + ",data='" +
                    data.ToString("yyyy-MM-dd") + "',ele=" +
                    ele.ToString() + ",ela=" +
                    ela.ToString() + ",religioso=" +
                    religioso.ToString() + ",festa=" +
                    festa.ToString() + " WHERE id="+id.ToString(), con);

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

        public static Evento Load(int id, MySqlConnection con)
        {
            Evento NA = new Evento();
            NA.ID = 0;
            try
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from evento where id=" + id.ToString(), con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Evento ret = new Evento();
                    ret.ID = id;
                    ret.TIPO = TipoEvento.load((int)dt.Rows[0]["tipo"], con);
                    ret.DATA = (DateTime)dt.Rows[0]["data"];
                    ret.Ela = Pessoa.Load((int)dt.Rows[0]["ela"], con);
                    ret.Ele = Pessoa.Load((int)dt.Rows[0]["ele"], con);
                    ret.RELIGIOSO = Local.Load((int)dt.Rows[0]["religioso"], con);
                    ret.FESTA = Local.Load((int)dt.Rows[0]["festa"], con);
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

    class TipoEvento
    {
        public int ID { get; set; }
        public string NOME { get; set; }

        public static List<TipoEvento> getAll(MySqlConnection con)
        {
            List<TipoEvento> all = new List<TipoEvento>();
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from tipo_evento order by id asc", con);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    TipoEvento tl;
                    foreach (DataRow r in dt.Rows)
                    {
                        tl = new TipoEvento();
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

        public static TipoEvento load(int id, MySqlConnection con)
        {
            TipoEvento NA = new TipoEvento();
            NA.ID = 0;
            NA.NOME = "N/A";
            try
            {
                if (con.State != ConnectionState.Open)
                    con.Open();
                DataTable dt = new DataTable();
                MySqlDataAdapter ad = new MySqlDataAdapter("select * from tipo_evento where id=" + id.ToString(), con);
                ad.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    TipoEvento ret = new TipoEvento();
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
