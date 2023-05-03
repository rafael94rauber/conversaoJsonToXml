using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using DocumentoXml = conversaoJsonToXml.DocumentoDto;

namespace conversaoJsonToXml
{
    internal class Program
    {
        static void Main(string[] args)
        {



            //string nomeCompleto = "Maria José da Silva";

            //// Separa o nome completo em uma lista de palavras
            //string[] palavras = nomeCompleto.Split();

            //// Inicializa as variáveis para nome e sobrenome
            //string nomeInicial = "";
            //string sobrenome = "";

            //// Itera sobre as palavras e identifica os nomes de acordo com a posição dentro do nome
            //for (int i = 0; i < palavras.Length; i++)
            //{
            //    if (i == 0)
            //    {
            //        nomeInicial = palavras[i];
            //    }
            //    else if (i == palavras.Length - 1)
            //    {
            //        sobrenome = palavras[i];
            //    }
            //    else if (palavras[i].Length > 2 && (palavras[i].ToLower() == "da" || palavras[i].ToLower() == "de"))
            //    {
            //        // Caso a palavra seja "da" ou "de" e tenha mais de 2 caracteres, assume que é parte do sobrenome
            //        sobrenome += " " + palavras[i];
            //    }
            //    else
            //    {
            //        // Caso contrário, assume que é parte do nome
            //        nomeInicial += " " + palavras[i];
            //    }
            //}

            //Console.WriteLine("Nome: " + nomeInicial);
            //Console.WriteLine("Sobrenome: " + sobrenome);
















            //var teste = "rafael eduardo rauber paulo";
            //var lista = teste.Split(' ');

            //var primeiroNome = 2;
            //var ultimoNome = 3;
            //var tamanhoLista = lista.Length;
            //var primeiroNomeCliente = string.Empty;
            //var segundoNomeCliente = string.Empty;

            //for (int posicao = 0; posicao < primeiroNome; posicao++)
            //{
            //    if (posicao > tamanhoLista)
            //    {
            //        continue;
            //    }
            //    primeiroNomeCliente += posicao == 0 ? lista[posicao] : $" {lista[posicao]}";
            //}

            //var listaSobreNome = lista.Reverse().ToArray();
            //for (int posicao = 0; posicao < ultimoNome; posicao++)
            //{
            //    if (posicao > tamanhoLista)
            //    {
            //        continue;
            //    }

            //    var nome = listaSobreNome[posicao];
            //    if (primeiroNomeCliente.Contains(nome))
            //    {
            //        continue;
            //    }
            //    segundoNomeCliente += string.IsNullOrEmpty(primeiroNomeCliente) ? nome : $" {nome}";
            //}
            //var ordenacaoCorreta = segundoNomeCliente.Split(' ').Reverse().Where(x => !string.IsNullOrEmpty(x)).ToArray();
            //segundoNomeCliente = string.Empty;
            //for (int posicao = 0; posicao < ordenacaoCorreta.Length; posicao++)
            //{
            //    segundoNomeCliente += posicao == 0 ? ordenacaoCorreta[posicao] : $" {ordenacaoCorreta[posicao]}";
            //}

            //Console.WriteLine(primeiroNomeCliente);
            //Console.WriteLine(segundoNomeCliente);


















            string retorno = System.IO.File.ReadAllText(@"C:\rauber\json.txt");

            var doc = new XmlDocument();
            doc.LoadXml(retorno);
            string jsonText = JsonConvert.SerializeXmlNode(doc);
            var objetoDinamico = JsonConvert.DeserializeObject<dynamic>(jsonText.Replace("@", ""));

            if (objetoDinamico.Documento.Regioes.Regiao.Count is null)
            {
                try
                {

                    var ss = objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[0].Tabelas.Tabela.Linhas;
                    var xxx = JsonConvert.SerializeObject(objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[0].Tabelas.Tabela.Linhas);
                    var xxxx = JsonConvert.DeserializeObject<LinhasDto2>(xxx);

                    var xxxxxxx = DeserializarXmlLinhasPadrao(JsonConvert.SerializeObject(objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[0].Tabelas.Tabela.Linhas));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                var s = new DocumentoDto()
                {
                    Cabecalho = DeserializarXmlPadrao(objetoDinamico.Documento.Regioes.Regiao.Campos),
                    Corpo = DeserializarXmlLinhasPadrao(JsonConvert.SerializeObject(objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[0].Tabelas.Tabela.Linhas)),
                RodaPe = DeserializarXmlPadrao(objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[1].Campos)
                };
            }

            //XmlDocument doc = JsonConvert.DeserializeXmlNode(RemoverCaracteres(retorno), "Retorno");
            //var teste = RemoverCaracteres(doc.OuterXml);
            //
            //XmlDocument semRemover = JsonConvert.DeserializeXmlNode(retorno, "Retorno");
            //var testesemRemover = semRemover.OuterXml;


            Console.ReadKey();
        }

        private static CampoDto DeserializarXmlForaPadrao(dynamic objeto)
        {
            return JsonConvert.DeserializeObject<CampoDto>(JsonConvert.SerializeObject(objeto));
        }

        private static CamposDto DeserializarXmlPadrao(dynamic objeto)
        {
            try
            {
                return JsonConvert.DeserializeObject<CamposDto>(JsonConvert.SerializeObject(objeto));
            }
            catch (Exception)
            {
                return new CamposDto() { Campo = DeserializarXmlForaPadrao(objeto) };
            }
        }

        private static LinhasDto DeserializarXmlLinhasPadrao(dynamic objeto)
        {
            try
            {
                return JsonConvert.DeserializeObject<LinhasDto>(objeto);           
            }
            catch (Exception)
            {
                return DeserializarXmlLinhasForaPadrao(objeto);
            }
        }

        private static LinhasDto DeserializarXmlLinhasForaPadrao(dynamic objeto)
        {            
            var xxxx = JsonConvert.DeserializeObject<LinhasDto2>(objeto);            
            
            return new LinhasDto()
            {
                Linha = new List<LinhaDto> { xxxx.Linha }
            };
        }

        public static string RemoverCaracteres(string Text)
        {
            var texto = System.IO.File.ReadAllText(@"C:\rauber\especial.txt");
            string[] ListCaracters = texto.ToString().Split(',');
            if (ListCaracters.Length <= 0)
            {
                ListCaracters = @"',\.".Split(',');
            }

            foreach (string item in ListCaracters)
            {
                Text = Text.Replace(item, string.Empty);
            }

            return Text;
        }
    }

    //private DocumentoXml RetornoTabelaXmlExtracao(List<string> xmlExtracao)
    //{
    //    if (xmlExtracao is null)
    //        return null;

    //    try
    //    {
    //        var doc = new XmlDocument();
    //        doc.LoadXml(xmlExtracao[0]);

    //        if (!doc.OuterXml.Contains("Regiao"))
    //            return null;

    //        string jsonText = JsonConvert.SerializeXmlNode(doc);
    //        var objetoDinamico = JsonConvert.DeserializeObject<dynamic>(jsonText.Replace("@", ""));

    //        if (objetoDinamico.Documento.Regioes.Regiao.Count is null)
    //        {
    //            return new DocumentoXml()
    //            {
    //                Cabecalho = DeserializarXmlPadrao(objetoDinamico.Documento.Regioes.Regiao.Campos),
    //                Corpo = JsonConvert.DeserializeObject<LinhasDto>(JsonConvert.SerializeObject(objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[0].Tabelas.Tabela.Linhas)),
    //                RodaPe = DeserializarXmlPadrao(objetoDinamico.Documento.Regioes.Regiao.Regioes.Regiao[1].Campos)
    //            };
    //        }
    //        else
    //        {
    //            return new DocumentoXml()
    //            {
    //                Cabecalho = DeserializarXmlPadrao(objetoDinamico.Documento.Regioes.Regiao[0].Campos)
    //            };
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        DocumentoXml erro = new()
    //        {
    //            Erro = ex.ToString()
    //        };
    //        return erro;
    //    }
    //}

    [XmlRoot(ElementName = "Coluna")]
    public class ColunaDto
    {
        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }

        [XmlAttribute(AttributeName = "Valor")]
        public string Valor { get; set; }
    }

    [XmlRoot(ElementName = "Colunas")]
    public class ColunasDto
    {
        [XmlElement(ElementName = "Coluna")]
        public List<ColunaDto> Coluna { get; set; }
    }

    [XmlRoot(ElementName = "Linha")]
    public class LinhaDto
    {
        [XmlElement(ElementName = "Colunas")]
        public ColunasDto Colunas { get; set; }
    }

    [XmlRoot(ElementName = "Linhas")]
    public class LinhasDto
    {
        [XmlElement(ElementName = "Linha")]
        public List<LinhaDto> Linha { get; set; }
    }

    [XmlRoot(ElementName = "Linhas")]
    public class LinhasDto2
    {        
        [XmlElement(ElementName = "Linha")]
        public LinhaDto Linha { get; set; }
    }

    public class DocumentoDto
    {
        public CamposDto Cabecalho { get; set; }
        public LinhasDto Corpo { get; set; }
        public CamposDto RodaPe { get; set; }
    }

    [XmlRoot(ElementName = "Campos")]
    public class CamposDto
    {

        [XmlElement(ElementName = "Campo")]
        public List<CampoDto> Campo { get; set; }

        [XmlElement(ElementName = "Campo")]
        public CampoDto CampoDados { get; set; }
    }

    [XmlRoot(ElementName = "Campo")]
    public class CampoDto
    {

        [XmlAttribute(AttributeName = "Nome")]
        public string Nome { get; set; }

        [XmlAttribute(AttributeName = "Valor")]
        public object Valor { get; set; }
    }
}
