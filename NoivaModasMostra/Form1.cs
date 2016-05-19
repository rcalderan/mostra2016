using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoivaModasMostra
{
    public partial class Mostra2016 : Form
    {
        private Conexao conexao;

        public Mostra2016()
        {
            InitializeComponent();
            conexao = new Conexao("localhost", "root", "33722363", "mostra2016");
        }
        private void mostraGb(GroupBox gb)
        {
            GroupBox[] gbs = { pessoaGb,evGb, locGb};
            foreach (GroupBox g in gbs)
            {
                if (g == gb)
                {
                    g.Show();
                    g.BringToFront();
                    g.Dock = DockStyle.Fill;
                }
                else
                {
                    g.Hide();
                    g.Dock = DockStyle.None;
                }
            }
        }

        private void pessoaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mostraGb(pessoaGb);
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void limpaEv()
        {
            evIdMtb.Clear();
            evLocal1Tb.Clear();
            evLocal2Tb.Clear();
            evDataDtp.Value = DateTime.Today;
            evEleIdTb.Text = "0";
            evElaIdTb.Text = "0";
            evElaTb.Clear();
            evEleTb.Clear();
            evTipoIdTb.Text = "0";
            evTipoCb.Text = "N/A";
        }

        private void evLimpaBt_Click(object sender, EventArgs e)
        {
            limpaEv();
        }

        private void EvSavebt_Click(object sender, EventArgs e)
        {


            try
            {
                int id;
                if (!int.TryParse(evIdMtb.Text, out id))
                    id = 0;

                if ((string.IsNullOrEmpty(evElaTb.Text) && (string.IsNullOrEmpty(evEleTb.Text))))
                {
                    MessageBox.Show("Escolha pelo menos 1 nome! (ele ou ela)");
                    return;
                }
                Evento evento = Evento.Load(id, conexao.getConnection());
                if (evento == null)
                {
                    evento = Evento.New(int.Parse(evTipoIdTb.Text), evDataDtp.Value, int.Parse(evEleIdTb.Text), int.Parse(evElaIdTb.Text), int.Parse(evLocal1IdTb.Text), int.Parse(evLocal2IdTb.Text), conexao.getConnection());
                    if (evento != null)
                    {
                        MessageBox.Show("Cadastro realizado com sucesso!");
                    }
                    else
                    {
                        MessageBox.Show("Erro ao cadastrar");
                    }
                }
                else
                {
                    if (DialogResult == MessageBox.Show("Deseja mesmo atualizar o cadastro?", "atualizar", MessageBoxButtons.YesNo))
                    {
                        if (Evento.Update(evento.ID, int.Parse(evTipoIdTb.Text), evDataDtp.Value,int.Parse(evEleIdTb.Text), int.Parse(evElaIdTb.Text), int.Parse(evLocal1IdTb.Text), int.Parse(evLocal2IdTb.Text), conexao.getConnection()))
                        {
                            MessageBox.Show("Atualização feita!");
                        }
                        else
                            MessageBox.Show("Erro ao atualizar");
                    }
                    else
                        carregaPes(id);
                }
            }
            catch (Exception er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void limpaPes()
        {
            pesHomenRb.Checked = false;
            pesMulherRb.Checked = false;
            pesBairroTb.Clear();
            pesCepMtb.Clear();
            pesCidadeTb.Clear();
            pesEstadoTb.Clear();
            pesEnderecoTb.Clear();
            pesNumeroTb.Clear();
            pesCpfMtb.Clear();
            pesNomeTb.Clear();
            pesEmailTb.Clear();
            pesFaceTb.Clear();
            pesFixoTb.Clear();
            pesCelTb.Clear();
            Pessoa p = Pessoa.Last(conexao.getConnection());
            if (p != null)
                pesIdMtb.Text = (p.ID+1).ToString();
            else
                pesIdMtb.Text="1";


        }
        private void pesLimpaBt_Click(object sender, EventArgs e)
        {
            limpaPes();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void locSalvaBt_Click(object sender, EventArgs e)
        {

        }

        private void limpaLoc()
        {
            locIdMtb.Clear();
            locNometb.Clear();
            locEstadotb.Clear();
            locBairroTb.Clear();
            locCidadeTb.Clear();
            locEnderecoTb.Clear();
            locNumeroTb.Clear();
            locTipoIdTb.Text = "0";
            locTipoCb.Text = "N/A";
        }

        private void locLimpaBt_Click(object sender, EventArgs e)
        {
            limpaLoc();
        }

        private void eventoToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            mostraGb(evGb);
        }

        private void localToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            mostraGb(locGb);
        }

        private void pesSaveTb_Click(object sender, EventArgs e)
        {
            try
            {
                if ((!pesMulherRb.Checked)&&(!pesHomenRb.Checked))
                {
                    MessageBox.Show("Selecione o sexo");
                    return;
                }
                int id;
                if (!int.TryParse(pesIdMtb.Text, out id))
                    id = 0;
                if (string.IsNullOrEmpty(pesNomeTb.Text))
                {
                    MessageBox.Show(" o Nome não deve estar em branco!");
                    return;
                }
                Pessoa pessoa = Pessoa.Load(int.Parse(pesIdMtb.Text), conexao.getConnection());
                if (pessoa == null)
                {
                    Local endereco = Local.New(1, "residencia", pesCepMtb.Text, pesEnderecoTb.Text, pesNumeroTb.Text, pesBairroTb.Text, pesCidadeTb.Text, pesEstadoTb.Text, conexao.getConnection());
                    pessoa = Pessoa.New(pesHomenRb.Checked,pesCpfMtb.Text, pesNomeTb.Text, pesCelTb.Text, pesFixoTb.Text,pesEmailTb.Text, pesNumeroTb.Text, endereco, conexao.getConnection());
                    if (pessoa != null)
                    {
                        limpaEv();
                        if (pesHomenRb.Checked)
                        {
                            evEleIdTb.Text = pessoa.ID.ToString();
                            evEleTb.Text = pessoa.NOME;
                        }
                        else
                        {
                            evElaIdTb.Text = pessoa.ID.ToString();
                            evElaTb.Text = pessoa.NOME;
                        }
                        MessageBox.Show("Cadastro realizado com sucesso!");
                        limpaPes();
                        mostraGb(evGb);
                    }
                    else
                    {
                        MessageBox.Show("Erro ao cadastrar");
                    }
                }
                else
                {
                    if (DialogResult == MessageBox.Show("Deseja mesmo atualizar o cadastro?", "atualizar", MessageBoxButtons.YesNo))
                    {
                        Local endereco = Local.New(1, "residencia", pesCepMtb.Text, pesEnderecoTb.Text, pesNumeroTb.Text, pesBairroTb.Text, pesCidadeTb.Text, pesEstadoTb.Text, conexao.getConnection());
                        if (Pessoa.Update(pessoa.ID, pesHomenRb.Checked,pesCpfMtb.Text, pesNomeTb.Text, pesCelTb.Text, pesFixoTb.Text, pesEmailTb.Text, pesNumeroTb.Text, endereco, conexao.getConnection()))
                        {
                            MessageBox.Show("Atualização feita!");
                        }
                        else
                            MessageBox.Show("Erro ao atualizar");
                    }
                    else
                        carregaPes(id);
                }
            }
            catch(Exception  er)
            {
                MessageBox.Show(er.Message);
            }
        }

        private void pessoaGb_VisibleChanged(object sender, EventArgs e)
        {
            if (pessoaGb.Visible)
            {
                Pessoa p = Pessoa.Last(conexao.getConnection());
                if (p != null)
                    pesIdMtb.Text = (p.ID + 1).ToString();
                else
                    pesIdMtb.Text = "1";
                pesNomeTb.AutoCompleteCustomSource.AddRange( Pessoa.getAllNames(conexao.getConnection()).ToArray());
            }

        }

        private void pesNomeTb_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (string.IsNullOrEmpty(tb.Text))
            {
                MessageBox.Show(" o Nome não deve estar em branco!");
                pesNomeTb.Select();
            }
        }

        private void evIdMtb_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void carregaEvento(int id)
        {
            Evento eve = Evento.Load(id, conexao.getConnection());
            if (eve != null)
            {
                evEleIdTb.Text = eve.Ele.ID.ToString();
                evEleTb.Text = eve.Ele.NOME;
                evElaIdTb.Text = eve.Ela.ID.ToString();
                evElaTb.Text = eve.Ela.NOME;
                evLocal1IdTb.Text = eve.RELIGIOSO.ID.ToString();
                evLocal1IdTb.Text = eve.RELIGIOSO.NOME;
                evLocal2IdTb.Text = eve.FESTA.ID.ToString();
                evLocal2IdTb.Text = eve.FESTA.NOME;
            }
            else
            {
                MessageBox.Show("Pessoa não encontrada.");
                limpaPes();
            }
        }

        private void evIdMtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode ==Keys.Enter)
            {
                int id;
                if (int.TryParse(evIdMtb.Text, out id))
                {
                    carregaEvento(id);
                }
                else
                {
                    MessageBox.Show("ID incorreto");
                }
            }
        }

        private void pesIdMtb_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void pesIdMtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int id;
                if (int.TryParse(pesIdMtb.Text, out id))
                {
                    carregaPes(id);
                }
                else
                {
                    MessageBox.Show("ID incorreto");
                }
            }
        }

        private void carregaPes(int id)
        {
            Pessoa p = Pessoa.Load(id, conexao.getConnection());
            if (p != null)
            {
                pesNomeTb.Text = p.NOME;
                pesEmailTb.Text = p.EMAIL;
                pesFaceTb.Text = p.FACEBOOK;
                pesCpfMtb.Text = p.CPF;
                pesCelTb.Text = p.CELULAR;
                pesFixoTb.Text = p.TELEFONE;
                if (p.SEXO)
                {
                    pesHomenRb.Checked = true;
                }
                else
                    pesMulherRb.Checked = true;

                pesCepMtb.Text = p.ENDERECO.CEP;
                pesEnderecoTb.Text = p.ENDERECO.ENDERECO;
                pesNumeroTb.Text = p.ENDERECO.NUMERO;
                pesBairroTb.Text = p.ENDERECO.BAIRRO;
                pesCidadeTb.Text = p.ENDERECO.CIDADE;
                pesEstadoTb.Text = p.ENDERECO.ESTADO;
            }
            else
            {
                MessageBox.Show("Pessoa não encontrada.");
                limpaPes();
            }
        }

        private void carregaPes(string nome_cpf)
        {
            Pessoa p = Pessoa.Load(nome_cpf, conexao.getConnection());
            if (p != null)
            {
                pesNomeTb.Text = p.NOME;
                pesEmailTb.Text = p.EMAIL;
                pesFaceTb.Text = p.FACEBOOK;
                pesCpfMtb.Text = p.CPF;
                pesCelTb.Text = p.CELULAR;
                pesFixoTb.Text = p.TELEFONE;
                if (p.SEXO)
                {
                    pesHomenRb.Checked = true;
                }
                else
                    pesMulherRb.Checked = true;

                pesCepMtb.Text = p.ENDERECO.CEP;
                pesEnderecoTb.Text = p.ENDERECO.ENDERECO;
                pesNumeroTb.Text = p.ENDERECO.NUMERO;
                pesBairroTb.Text = p.ENDERECO.BAIRRO;
                pesCidadeTb.Text = p.ENDERECO.CIDADE;
                pesEstadoTb.Text = p.ENDERECO.ESTADO;
            }
            else
            {
                MessageBox.Show("Pessoa não encontrada.");
                limpaPes();
            }
        }

        private void carregaLocal(int id)
        {
            Local l = Local.Load(id, conexao.getConnection());
            if (l != null)
            {
                locCepMtb.Text = l.CEP;
                locNometb.Text = l.NOME;
                locEnderecoTb.Text = l.ENDERECO;
                locNumeroTb.Text = l.NUMERO;
                locBairroTb.Text = l.BAIRRO;
                locCidadeTb.Text = l.CIDADE;
                locEstadotb.Text = l.ESTADO;
                locTipoIdTb.Text = l.TIPO.ID.ToString();
                locTipoCb.Text = l.TIPO.NOME;

            }
            else
            {
                MessageBox.Show("Local não encontrado.");
                limpaPes();
            }
        }

        private void locIdMtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int id;
                if (int.TryParse(locIdMtb.Text, out id))
                {
                    carregaLocal(id);
                }
                else
                {
                    MessageBox.Show("ID incorreto");
                }
            }
        }

        private void locGb_VisibleChanged(object sender, EventArgs e)
        {
            if (locGb.Visible)
            {
                locTipoCb.Items.Clear();
                locTipoCb.Items.AddRange(TipoLocal.getAll(conexao.getConnection()).ToArray());
            }
        }

        private void evGb_VisibleChanged(object sender, EventArgs e)
        {
            if (evGb.Visible)
            {
                evTipoCb.Items.Clear();
                evTipoCb.Items.AddRange(TipoEvento.getAll(conexao.getConnection()).ToArray());
            }
        }

        private void pesNomeTb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Pessoa p = Pessoa.Load(pesNomeTb.Text, conexao.getConnection());
                if (p != null)
                {
                    pesNomeTb.Text = p.NOME;
                    pesEmailTb.Text = p.EMAIL;
                    pesFaceTb.Text = p.FACEBOOK;
                    pesCpfMtb.Text = p.CPF;
                    pesCelTb.Text = p.CELULAR;
                    pesFixoTb.Text = p.TELEFONE;
                    if (p.SEXO)
                    {
                        pesHomenRb.Checked = true;
                    }
                    else
                        pesMulherRb.Checked = true;

                    pesCepMtb.Text = p.ENDERECO.CEP;
                    pesEnderecoTb.Text = p.ENDERECO.ENDERECO;
                    pesNumeroTb.Text = p.ENDERECO.NUMERO;
                    pesBairroTb.Text = p.ENDERECO.BAIRRO;
                    pesCidadeTb.Text = p.ENDERECO.CIDADE;
                    pesEstadoTb.Text = p.ENDERECO.ESTADO;
                }
            }
        }

        private void pesCpfMtb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Pessoa p = Pessoa.Load(pesCpfMtb.Text, conexao.getConnection());
                if (p != null)
                {
                    pesNomeTb.Text = p.NOME;
                    pesEmailTb.Text = p.EMAIL;
                    pesFaceTb.Text = p.FACEBOOK;
                    pesCpfMtb.Text = p.CPF;
                    pesCelTb.Text = p.CELULAR;
                    pesFixoTb.Text = p.TELEFONE;
                    if (p.SEXO)
                    {
                        pesHomenRb.Checked = true;
                    }
                    else
                        pesMulherRb.Checked = true;

                    pesCepMtb.Text = p.ENDERECO.CEP;
                    pesEnderecoTb.Text = p.ENDERECO.ENDERECO;
                    pesNumeroTb.Text = p.ENDERECO.NUMERO;
                    pesBairroTb.Text = p.ENDERECO.BAIRRO;
                    pesCidadeTb.Text = p.ENDERECO.CIDADE;
                    pesEstadoTb.Text = p.ENDERECO.ESTADO;
                }
            }
        }
    }
}
