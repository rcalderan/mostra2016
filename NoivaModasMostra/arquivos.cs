using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Configuration;
using Microsoft.Win32;


namespace NoivaModasMostra
{
    class Arquivos
    {
        /*private static string dRaiz = @"c:\NmSystemV2";
        private static string dirLog = @"\log";
        private static string dirSys = @"\sys";*/
        private static string dRaiz = System.Windows.Forms.Application.StartupPath;
        private static string dirLog = @"\log";
        private static string dirSys = @"\sys";
        private static string securityKey = "nMlHvNcR";

        private static Dictionary<string,string> arqs =  new Dictionary<string,string>()
        { 
            {"log.txt",dirLog},
            {"erro.txt",dirLog},
            {"tp.txt",dirSys}
        };

        public Dictionary<string,string> linhas = new Dictionary<string,string>()
        {
            {"BR","BRACELETE"},{"C","CAMISA"},{"C4","CAMISA 4"},{"C6","CAMISA 6"},{"C8","CAMISA 8"},{"C10","CAMISA 10"},
            {"C12","CAMISA 12"},{"C14","CAMISA 14"},{"C16","CAMISA 16"},{"C36","CAMISA 36"},{"C38","CAMISA 38"},
            {"C39","CAMISA 39"},{"C40","CAMISA 40"},{"C41","CAMISA 41"},{"C42","CAMISA 42"},{"C43","CAMISA 43"},{"C44","CAMISA 44"},{"C45","CAMISA 45"},{"C46","CAMISA 46"},
            {"C47","CAMISA 47"},{"C48","CAMISA 48"},{"C49","CAMISA 49"},{"C50","CAMISA 50"},{"C51","CAMISA 51"},{"C52","CAMISA 52"},
            {"C54","CAMISA 54"},{"C58","CAMISA 58"},{"C60","CAMISA 60"},{"C62","CAMISA 62"},{"CC","CALÇA"},{"CC2","CALÇA 2"},{"CC4","CALÇA 4"},
            {"CC6","CALÇA 6"},{"CC8","CALÇA 8"},{"CC10","CALÇA 10"},{"CC12","CALÇA 12"},{"CC14","CALÇA 14"},{"CC16","CALÇA 16"},{"GA","GALHINHOS"},
            {"S24","SAPATO 24"},{"S25","SAPATO 25"},{"S26","SAPATO 26"},{"S27","SAPATO 27"},{"S28","SAPATO 28"},{"S29","SAPATO 29"},{"S30","SAPATO 30"},
            {"S31","SAPATO 31"},{"S32","SAPATO 32"},{"S33","SAPATO 33"},{"S34","SAPATO 34"},{"S35","SAPATO 35"},{"S36","SAPATO 36"},{"S37","SAPATO 37"},
            {"S38","SAPATO 38"},{"S39","SAPATO 39"},{"S40","SAPATO 40"},{"S41","SAPATO 41"},{"S42","SAPATO 42"},{"S43","SAPATO 43"},{"S44","SAPATO 44"},
            {"S45","SAPATO 45"},{"S46","SAPATO 46"},{"AL","ALMOFADA"},{"PA","PASSO ART"},{"G","GRAVATA"},{"G2","2 GRAVATAS"},
            {"G3","3 GRAVATAS"},{"ES","ESTOLA"},{"BA","BAMBOLE"},{"CO","COROA"},{"TI","TIARA"},{"LU","LUVA"},{"VEU","VEU"},{"SAI","SAIOTE"},
            {"cop","CALSIVER OP"},{"CBOX","CALSIVER BOX"},{"BOX","BOX"},{"OP","OP"},{"CAL","CALSIVER"},{"PAR","PASSO ART"},
            {"VM","COLOCAR VIRA NA MANGA"},{"VB","COLOCAR VIRA NA BARRA"}
        };
            

        
        public Arquivos()
        {
            try
            {
                //string t = "teste!",e = Encrypt(t,false),d = Decrypt(e,false);
                //System.Windows.Forms.MessageBox.Show("string: "+t+"\nEncrypt: "+e+"\nDecrypt"+d);
                //checa diretorios
                if (!Directory.Exists(dRaiz))
                {
                    Directory.CreateDirectory(dRaiz);
                }
                if (!Directory.Exists(dRaiz + dirSys))
                {
                    Directory.CreateDirectory(dRaiz + dirSys);
                }
                if (!Directory.Exists(dRaiz + dirLog))
                {
                    Directory.CreateDirectory(dRaiz + dirLog);
                }
                //checa arquivos
                foreach (KeyValuePair<string, string> pair in arqs)
                {
                    if (!File.Exists(dRaiz + pair.Value + "\\" + pair.Key))
                    {
                        if (pair.Key == "tp.txt")
                            escreveTp();
                        else
                            File.Create(dRaiz + pair.Value + "\\" + pair.Key);
                    }
                }

            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
            }
        }

        /*
        public static Dictionary<string,string> getDataFromArq()
        {
            try
            {
                
                Dictionary<string, string> result = new Dictionary<string, string>();
                
                string aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem", "nm", null);
                if (aux != null)
                {
                    string decr = Decrypt(aux, true);
                    string[] s = decr.Split(' ');
                    result.Add("confHostTb", s[0]);
                    result.Add("confUserTb", s[1]);
                    result.Add("confPassTb", s[2]);
                }
                return result;
            }
            catch
            {
                return new Dictionary<string, string>();
            }
        }
         */

        public static Dictionary<string,string> getDataFromReg()
        { //host, user, pass
            try
            {
                Dictionary<string, string> result = new Dictionary<string, string>();
                string aux= (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem", "nm", null);
                if (aux != null)
                {
                    string decr = Decrypt(aux, true);
                    string[] s = decr.Split(' ');
                    result.Add("confHostTb", s[0]);
                    result.Add("confUserTb", s[1]);
                    result.Add("confPassTb", s[2]);
                }
                else
                {
                    string[] s = {"localhost","",""};
                    string valor=Encrypt(s[0]+" "+s[1]+" "+s[2],true);
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem", "nm", valor, RegistryValueKind.String);
                    result.Add("confHostTb", s[0]);
                    result.Add("confUserTb", s[1]);
                    result.Add("confPassTb", s[2]);
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confAoFecharCh", null);
                if (aux != null)
                {
                    result.Add("confAoFecharCh", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confAoFecharCh", "1", RegistryValueKind.String);
                    result.Add("confAoFecharCh", "1");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confAoAbrirCh", null);
                if (aux != null)
                {
                    result.Add("confAoAbrirCh", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confAoAbrirCh", "0", RegistryValueKind.String);
                    result.Add("confAoAbrirCh", "0");
                }

                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confFantasiaTb", null);
                if (aux != null)
                {
                    result.Add("confFantasiaTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confFantasiaTb", "Nome Fantasia da Empresa", RegistryValueKind.String);
                    result.Add("confFantasiaTb", "Nome Fantasia da Empresa");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEmpresaTb", null);
                if (aux != null)
                {
                    result.Add("confEmpresaTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEmpresaTb", "", RegistryValueKind.String);
                    result.Add("confEmpresaTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confCnpjTb", null);
                if (aux != null)
                {
                    result.Add("confCnpjTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confCnpjTb", "", RegistryValueKind.String);
                   result.Add("confCnpjTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEnderTb", null);
                if (aux != null)
                {
                    result.Add("confEnderTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEnderTb", "", RegistryValueKind.String);
                   result.Add("confEnderTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confNumeroTb", null);
                if (aux != null)
                {
                    result.Add("confNumeroTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confNumeroTb", "", RegistryValueKind.String);
                    result.Add("confNumeroTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confCepTb", null);
                if (aux != null)
                {
                    result.Add("confCepTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confCepTb", "", RegistryValueKind.String);
                    result.Add("confCepTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confBairroTb", null);
                if (aux != null)
                {
                    result.Add("confBairroTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confBairroTb", "", RegistryValueKind.String);
                    result.Add("confBairroTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confCidadeTb", null);
                if (aux != null)
                {
                    result.Add("confCidadeTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confCidadeTb", "", RegistryValueKind.String);
                    result.Add("confCidadeTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confUfTb", null);
                if (aux != null)
                {
                    result.Add("confUfTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confUfTb", "", RegistryValueKind.String);
                    result.Add("confUfTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confTel1Tb", null);
                if (aux != null)
                {
                    result.Add("confTel1Tb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confTel1Tb", "", RegistryValueKind.String);
                    result.Add("confTel1Tb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confTel2Tb", null);
                if (aux != null)
                {
                    result.Add("confTel2Tb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confTel2Tb", "", RegistryValueKind.String);
                    result.Add("confTel2Tb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEmailTb", null);
                if (aux != null)
                {
                    result.Add("confEmailTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEmailTb", "", RegistryValueKind.String);
                    result.Add("confEmailTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confSiteTb", null);
                if (aux != null)
                {
                    result.Add("confSiteTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confSiteTb", "", RegistryValueKind.String);
                    result.Add("confSiteTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEstadualTb", null);
                if (aux != null)
                {
                    result.Add("confEstadualTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confEstadualTb", "", RegistryValueKind.String);
                    result.Add("confEstadualTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confMunicipalTb", null);
                if (aux != null)
                {
                    result.Add("confMunicipalTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confMunicipalTb", "", RegistryValueKind.String);
                    result.Add("confMunicipalTb", "");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confBkpCh", null);
                if (aux != null)
                {
                    result.Add("confBkpCh", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confBkpCh", "0", RegistryValueKind.String);
                    result.Add("confBkpCh", "0");
                }
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confBkpPathTb", null);
                if (aux != null)
                {
                    result.Add("confBkpPathTb", aux);
                }
                else
                {
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confBkpPathTb", @"c:\backup", RegistryValueKind.String);
                    result.Add("confBkpPathTb", @"c:\backup");
                } 
                string padrao;
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confDesistenciaTb", null);
                if (aux != null)
                {
                    result.Add("confDesistenciaTb", aux);
                }
                else
                {
                    padrao = @"                                          T E R M O   DE   D E S I S T Ê N C I A"
                        + @"\\\Eu, <nome>, prorador(a) do CPF <cpf> e do RG <rg>, declaro que"
                        + @"\estou DESISTINDO do aluguel de:"
                        + @"\<desc1>" + @"\" + @"\<desc2>" + @"\<desc3>" + @"\<desc4>" + @"\<desc5>" + @"\<desc6>"
                        + @"\<desc7>" + @"\<desc8>" + @"\<desc9>" + @"\<desc10>" + @"\<desc11>" + @"\<desc12>" + @"\<desc13>"
                        + @"\\descriminado(s) no contrato numero <contrato>, que foi assinado no dia <hoje> e seria usado"
                        + @"\no dia <usa>."
                        + @"\Declaro ainda estar ciente que os valores pagos não serão devolvidos a mim e nem transferidos"
                        + @"\a outra pessoa ou outro aluguel." 
                        + @"\\\\<Cidade>,  <agora>"
                        + @"\                                                                               _____________________________________" 
                        + @"\                                      <nome>";
                    /*padrao = @"LEIA COM ATENÇÃO:\MARIA HELENA DE CARVALHO CONFECÇÕES-ME  acima identificada, e a seguir denominada LOCADORA, e de outro lado "
                        + @"\o Cliente acima identificado, e a seguir denominado LOCATÁRIO, celebram o presente contrato de locação mediante as seguintes"
                        + @"\Cláusulas e Comdições:\Cláusula 1a - O objeto do presente contrato é a Locação de artigos do vestuário, acima especificado."
                        + @"\Cláusula 2a - As datas, para retirada das mercadorias, bem como da sua utilização e devolução encontram-se acima especificadas."
                        + @"\Cláusula 3a - A LOCADORA não se responsabilizará pelas mercadorias que não forem retiradas até a data de uso estabelecida "
                        + @"\                       neste contrato.\Cláusula 4a - A mercadoria alugada veverá ser devolvida até as 18:00 (Dezoito) horas da data estabelecida neste contrato, completa,"
                        + @"\                       tal como foi retirada.\Parágrafo Primeiro: Caso as mercadorias não sejam devolvidas na data prevista neste contrato sofrerão um acréscimo de R$ 10,00"
                        + @"\                       (Dez Reais) por dia útil de atraso, se esta condicão perdurar por 07 (sete) dias será cobrado uma nova taxa de"
                        + @"\                       Locação da mesma de acordo com o valor pago pelo LOCATÁRIO.\Parágrafo Segundo: Caso as mercadorias sejam devolvidas com manchas de gordura, graxa, tinta ou qualquer outro produto que as"
                        + @"\                       danifique, será cobrado uma taxa de R$ 30,00 (Trinta Reais).\Cláusula 5a - Em caso de troca das mercadorias objeto do presente contrato será cobrado taxa de R$ 20,00 referente"
                        + @"\                       aos Serviços Executados.\Parágrafo Único: Não é permitido a troca das Mercadorias, após 05 (cinco) dias corridos da data de locação."
                        + @"\Cláusula 6a - Em caso de desistência por parte do LOCATÁRIO, o valor pago a título de Locação não será devolvido nem transferido"
                        + @"\                       a outras pessoas.\Cláusula 7a - As mercadorias, objeto da referida locação não poderão ser emprestadas ou tranferidas a outras pessoas.\Cláusula 8a - O LOCATÁRIO se compromete a ressarcir a LOCADORA pelo valor de tabela de Mercado, vigente na data do evento,"
                        + @"\                       pelo extravio ou dano nas mercadorias objeto deste contrato, bem como seu uso indevido.\Cláusula 9a - A LOCADORA se compromete a entregar a Mercadoria lavada e passada, com os devidos ajustes solicitados pelo"
                        + @"\                       LOCATÁRIO e em perfeito estado de conservação e uso, de acordo como foi requirido durante a prova na data deste contrato.\Cláusula 10a - Caso seja constatada alguma denificação na Mercadoria locada no momento da retirada pelo LOCATÁRIO, a"
                        + @"\                       LOCADORA se compromete  a efetuar a substituição, ou a troca, independente do seu preço de locação, ou  a devida\                       devolução do valor pago, tudo conforme a disponibilidade do produto ou conveniência da LOCADORA."
                        + @"\Cláusula 11a - E por estarem juntos e acordados, firma o presente contrato, ficando eleito o Fórum desta Comarca para dirimir\                       quaisquer dúvidas que possam surgir.";*/
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confDesistenciaTb", padrao, RegistryValueKind.String);
                    result.Add("confDesistenciaTb", padrao);
                }
                
                aux = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confClausulasTb", null);
                if (aux != null)
                {
                    result.Add("confClausulasTb", aux);
                }
                else
                {
                    padrao = @"                                                                                <Nome Fantasia>                                                                                      N.  <contrato>"
                        +@"\<linha>"
                        +@"\<Nome da Empresa>. CNPJ: <CNPJ>, Inscrição Estadual: <Inscrição Estadual>"
                    +@"\e Instrição Municipal: <Inscrição Municipal>, Empresa especializada em Aluguéis de Noivas, Trajes a Rigor e"
                    +@"\Artigos do vestuário em geral. Telefone: <Telefone1> ou <Telefone2>, E-Mail: <email>"
                    +@"\Acesse: <Site>"
                    +@"\\Cliente: <ccli> - <nome>"
                    +@"\CPF: <cpf>"
                    +@"\Endereço: <endereco>, Bairro: <bairro>"
                    +@"\Cidade: <cidade>/<uf>    Telefone: (<ddd1>)<fone1> ou (<ddd2>)<fone2>"
                    +@"\<linha>"
                    +@"\RETIRADA: <retirada>                                             USA DIA: <usa>                                                      Devolução: <devolucao>"
                    +@"\<linha>"
                    +@"\codigo                                                           Descrição                                                                                                   Valor"
                    +@"\<descreve1>"+@"\<descreve2>"+@"\<descreve3>"+@"\<descreve4>"+@"\<descreve5>"+@"\<descreve6>"
                    +@"\<descreve7>"+@"\<descreve8>"+@"\<descreve9>"+@"\<descreve10>"+@"\<descreve11>"+@"\<descreve12>"+@"\<descreve13>"
                    +@"\<linha>"
                    +@"\                                                                                                                                                                          Total R$ <total>,00"
                    +@"\<linha>"+@"\FORMAS DE PAGAMENTO"+@"\VALOR R$                                                                Vencimento                                                                             Carimbo"
                    +@"\<pagamentos0>"+@"\<pagamentos1>"+@"\<pagamentos2>"+@"\<pagamentos3>"+@"\<pagamentos4>"+@"\<pagamentos5>"
                    +@"\<pagamentos6>"+@"\<pagamentos7>"+@"\<pagamentos8>"+@"\<pagamentos9>"
                    +@"\<linha>"
                    +@"\\LEIA COM ATENÇÃO:"
                    +@"\MARIA HELENA DE CARVALHO CONFECÇÕES-ME  acima identificada, e a seguir denominada LOCADORA, e de outro lado "
                    +@"\o Cliente <nome> (acima identificado), e a seguir denominado LOCATÁRIO,"
                    +@"\celebram o presente contrato de locação mediante as seguintes Cláusulas e Comdições:"
                    +@"\Cláusula 1a - O objeto do presente contrato é a Locação de artigos do vestuário, acima especificado."
                    +@"\Cláusula 2a - As datas, para retirada das mercadorias, bem como da sua utilização e devolução encontram-se acima especificadas."
                    +@"\Cláusula 3a - A LOCADORA não se responsabilizará pelas mercadorias que não forem retiradas até a data de uso estabelecida "
                    +@"\neste contrato."
                    +@"\Cláusula 4a - A mercadoria alugada veverá ser devolvida até as 18:00 (Dezoito) horas da data estabelecida neste contrato, completa,"
                    +@"\                       tal como foi retirada."
                    +@"\Parágrafo Primeiro: Caso as mercadorias não sejam devolvidas na data prevista neste contrato sofrerão um acréscimo de R$ 10,00"
                    +@"\                       (Dez Reais) por dia útil de atraso, se esta condicão perdurar por 07 (sete) dias será cobrado uma nova taxa de"
                    +@"\                       Locação da mesma de acordo com o valor pago pelo LOCATÁRIO."
                    +@"\Parágrafo Segundo: Caso as mercadorias sejam devolvidas com manchas de gordura, graxa, tinta ou qualquer outro produto que as"
                    +@"\                       danifique, será cobrado uma taxa de R$ 30,00 (Trinta Reais)."
                    +@"\Cláusula 5a - Em caso de troca das mercadorias objeto do presente contrato será cobrado taxa de R$ 20,00 referente"
                    +@"\                       aos Serviços Executados."
                    +@"\Parágrafo Único: Não é permitido a troca das Mercadorias, após 05 (cinco) dias corridos da data de locação."
                    +@"\Cláusula 6a - Em caso de desistência por parte do LOCATÁRIO, o valor pago a título de Locação não será devolvido nem transferido"
                    +@"\                       a outras pessoas."
                    +@"\Cláusula 7a - As mercadorias, objeto da referida locação não poderão ser emprestadas ou tranferidas a outras pessoas."
                    +@"\Cláusula 8a - O LOCATÁRIO se compromete a ressarcir a LOCADORA pelo valor de tabela de Mercado, vigente na data do evento,"
                    +@"\                       pelo extravio ou dano nas mercadorias objeto deste contrato, bem como seu uso indevido."
                    +@"\Cláusula 9a - A LOCADORA se compromete a entregar a Mercadoria lavada e passada, com os devidos ajustes solicitados pelo"
                    +@"\                      LOCATÁRIO e em perfeito estado de conservação de acordo como foi requirido durante a prova na data deste contrato."
                    +@"\Cláusula 10a - Caso seja constatada alguma denificação na Mercadoria locada no momento da retirada pelo LOCATÁRIO, a"
                    +@"\                       LOCADORA se compromete  a efetuar a substituição, ou a troca, independente do seu preço de locação, ou  a devida"
                    +@"\                       devolução do valor pago, tudo conforme a disponibilidade do produto ou conveniência da LOCADORA."
                    +@"\Cláusula 11a - E por estarem juntos e acordados, firma o presente contrato, ficando eleito o Fórum desta Comarca para dirimir"
                    +@"\                       quaisquer dúvidas que possam surgir."
                    +@"\<linha>"
                    +@"\<Cidade>, <hoje>"
                    +@"\"
                    +@"\                                                                                                         ______________________________________"
                    +@"\                                                                                                         <nome>"
                    +@"\"
                    +@"\É OBRIGATÓRIO APRESENTAR ESTE CONTRATO NA RETIRADA!";
                    Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", "confClausulasTb", padrao, RegistryValueKind.String);
                    result.Add("confClausulasTb", padrao);
                }
                return result;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }

        }

        public static void updateReg(string key,string value)
        {
            Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem\settings", key, value, RegistryValueKind.String);
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();

            // Get the key from config file

            //string securityKey = (string)settingsReader.GetValue("nMlHvNcR",typeof(String));//caso resolva utizar app.config file
            
            //System.Windows.Forms.MessageBox.Show(key);
            //If hashing use get hashcode regards to your key
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));
                //Always release the resources and flush data
                // of the Cryptographic service provide. Best Practice

                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray =
              cTransform.TransformFinalBlock(toEncryptArray, 0,
              toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();
            //Get your key from config file to open the lock!
            //string key = (string)settingsReader.GetValue("SecurityKey",typeof(String));

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(securityKey));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(securityKey);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(
                                 toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        
        public static void SetDataToArq(string host,string user,string pass)
        {
            string[] r = new string[3];
            if (!File.Exists(dRaiz + @"\" + dirSys + @"\nm"))
                File.CreateText(dRaiz + @"\" + dirSys + @"\nm");

            string line = Encrypt(host + " " + user + " " + pass, true);
            using (StreamWriter sw = new StreamWriter(dRaiz + @"\" + dirSys + @"\nm"))
            {
                sw.WriteLine(line);
                sw.Close();
            }

        }

        public static void setNmData(string host, string user, string pass)
        {
            string encrpted = Encrypt(host + " " + user + " " + pass, true);

            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(System.Security.Principal.WindowsIdentity.GetCurrent());
            if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
            {//caso admin, salve no reg 
                Registry.SetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\nmsystem", "nm", encrpted, RegistryValueKind.String);
            }

            //if (!File.Exists(dRaiz + @"\" + dirSys + @"\nm"))
              //  File.CreateText(dRaiz + @"\" + dirSys + @"\nm");
            
            using (StreamWriter sw = new StreamWriter(dRaiz + @"\" + dirSys + @"\nm"))
            {
                sw.WriteLine(encrpted);
                sw.Close();
            }
        }
        public static string[] getNmData()
        {
            string decrpted = "";
            using (StreamReader sr = new StreamReader(dRaiz + @"\" + dirSys + @"\nm"))
            {
                decrpted = sr.ReadLine();
                sr.Close();
            }
            decrpted = Decrypt(decrpted, true);
            return decrpted.Split(' ');
        }

        public static string[] getDataFromArq()
        {
            string[] r = new string[3];
            string line;
            if (!File.Exists(dRaiz + @"\" + dirSys + @"\nm"))
            {
                File.CreateText(dRaiz + @"\" + dirSys + @"\nm");
            }
            using (StreamReader sr = new StreamReader(dRaiz + @"\" + dirSys + @"\nm"))
            {
                line = sr.ReadLine();
                sr.Close();
            }
            line = Decrypt(line, true);
            r = line.Split(' ');
            return r;

        }

        public List<string> getIpFromFile(string x)
        { //host, user, pass
            try
            {
                List<string> result = new List<string>();
                DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
                //A 64 bit key and IV is required for this provider.
                //Set secret key For DES algorithm.
                DES.Key = ASCIIEncoding.ASCII.GetBytes(securityKey);
                //Set initialization vector.
                DES.IV = ASCIIEncoding.ASCII.GetBytes(securityKey);

                //Create a file stream to read the encrypted file back.
                FileStream fsread = new FileStream(dRaiz + @"\" + dirSys + @"\conf.nm",
                   FileMode.Open,
                   FileAccess.Read);
                //Create a DES decryptor from the DES instance.
                ICryptoTransform desdecrypt = DES.CreateDecryptor();
                //Create crypto stream set to read and do a 
                //DES decryption transform on incoming bytes.
                CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt, CryptoStreamMode.Read);
                //Print the contents of the decrypted file.
                StreamWriter fsDecrypted = new StreamWriter(dRaiz + @"\" + dirSys + @"\conf2.nm");
                fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
                fsDecrypted.Flush();
                fsDecrypted.Close();
                using (StreamReader srw = new StreamReader(dRaiz + @"\" + dirSys + @"\conf2.nm"))
                    if (srw != null)
                    {
                        while (!srw.EndOfStream)
                            result.Add(srw.ReadLine());
                        srw.Close();
                        File.Delete(dRaiz + @"\" + dirSys + @"\conf2.nm");
                        return result;
                    }
                    else
                    {
                        File.Delete(dRaiz + @"\" + dirSys + @"\conf2.nm");
                        return null;
                    }    
            }
            catch(Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.Message);
                return null;
            }

        }
         
        public string criaLog(string log, string tipo_de_log)
        {
            try
            {
                string path = dRaiz + dirLog + @"\log.txt";
                string linha = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + "(" + tipo_de_log + ") -  " + log;
                List<string> linhas = new List<string>();
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                        linhas.Add(sr.ReadLine());
                    linhas.Add(linha);
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (String ln in linhas)
                    {
                        sw.WriteLine(ln);
                    }
                }
                return "";
            }
            catch (Exception erro)
            {
                return erro.Message;
            }
        }

        public string logErro(string log)
        {
            try
            {
                string path = dRaiz + dirLog + @"\erro.txt";
                string linha = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString() + " -  " + log;
                List<string> linhas = new List<string>();
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                        linhas.Add(sr.ReadLine());
                    linhas.Add(linha);
                    sr.Close();
                }
                using (StreamWriter sw = new StreamWriter(path))
                {
                    foreach (String ln in linhas)
                    {
                        sw.WriteLine(ln);
                    }
                    sw.Close();
                }
                return "";
            }
            catch (Exception erro)
            {
                return erro.Message;
            }
        }
        
        private void escreveTp()
        {
            
            using (StreamWriter sw = new StreamWriter(dRaiz + @"\" + dirSys + @"\tp.txt"))
            {
                foreach (KeyValuePair<string, string> pair in linhas)
                {
                    sw.WriteLine(pair.Key+"\t"+pair.Value);
                }
                sw.Close();
            }
            
        }
        
        private static void EncryptData(String inName, String outName)
        {
            
            byte[] desKey = new byte[8], desIV = new byte[8];
            //Create the file streams to handle the input and output files.
            FileStream fin = new FileStream(inName, FileMode.Open, FileAccess.Read);
            FileStream fout = new FileStream(outName, FileMode.OpenOrCreate, FileAccess.Write);
            fout.SetLength(0);

            //Create variables to help with read and write.
            byte[] bin = new byte[100]; //This is intermediate storage for the encryption.
            long rdlen = 0;              //This is the total number of bytes written.
            long totlen = fin.Length;    //This is the total length of the input file.
            int len;                     //This is the number of bytes to be written at a time.

            DES des = new DESCryptoServiceProvider();
            for (int i=0;i<8;i++)
            {
                desKey[i] = Convert.ToByte(securityKey[i]);
                desIV[i] = Convert.ToByte(securityKey[i]);
            }
            CryptoStream encStream = new CryptoStream(fout, des.CreateEncryptor(desKey, desIV), CryptoStreamMode.Write);


            //Read from the input file, then encrypt and write to the output file.
            while (rdlen < totlen)
            {
                len = fin.Read(bin, 0, 100);
                encStream.Write(bin, 0, len);
                rdlen = rdlen + len;
            }

            encStream.Close();
            fout.Close();
            fin.Close();
        }

        private static void EncryptFile(string sInputFilename,
           string sOutputFilename,
           string sKey)
        {
            FileStream fsInput = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);

            FileStream fsEncrypted = new FileStream(sOutputFilename,
               FileMode.Create,
               FileAccess.Write);
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            System.Security.Cryptography.ICryptoTransform desencrypt = DES.CreateEncryptor();
            System.Security.Cryptography.CryptoStream cryptostream = new System.Security.Cryptography.CryptoStream(fsEncrypted,
               desencrypt,
               System.Security.Cryptography.CryptoStreamMode.Write);

            byte[] bytearrayinput = new byte[fsInput.Length];
            fsInput.Read(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Write(bytearrayinput, 0, bytearrayinput.Length);
            cryptostream.Close();
            fsInput.Close();
            fsEncrypted.Close();
        }

        private static void DecryptFile(string sInputFilename,
           string sOutputFilename,
           string sKey)
        {
            DESCryptoServiceProvider DES = new DESCryptoServiceProvider();
            //A 64 bit key and IV is required for this provider.
            //Set secret key For DES algorithm.
            DES.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            //Set initialization vector.
            DES.IV = ASCIIEncoding.ASCII.GetBytes(sKey);

            //Create a file stream to read the encrypted file back.
            FileStream fsread = new FileStream(sInputFilename,
               FileMode.Open,
               FileAccess.Read);
            //Create a DES decryptor from the DES instance.
            ICryptoTransform desdecrypt = DES.CreateDecryptor();
            //Create crypto stream set to read and do a 
            //DES decryption transform on incoming bytes.
            CryptoStream cryptostreamDecr = new CryptoStream(fsread, desdecrypt,CryptoStreamMode.Read);
            //Print the contents of the decrypted file.
            StreamWriter fsDecrypted = new StreamWriter(sOutputFilename);
            fsDecrypted.Write(new StreamReader(cryptostreamDecr).ReadToEnd());
            fsDecrypted.Flush();
            fsDecrypted.Close();
        } 
    }
}
