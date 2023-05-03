using Newtonsoft.Json;
using System.Xml;

string retorno = System.IO.File.ReadAllText(@"C:\rauber\json.txt");


XmlDocument doc = JsonConvert.DeserializeXmlNode(RemoverCaracteres(retorno), "Retorno");
var teste = RemoverCaracteres(doc.OuterXml);



Console.WriteLine(teste);
Console.ReadKey();


string RemoverCaracteres(string Text)
{
    string[] ListCaracters = System.IO.File.ReadAllText(@"C:\rauber\especial.txt").ToString().Split(',');
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

