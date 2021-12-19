using Microsoft.CSharp;
using Newtonsoft.Json;
using OfficeOpenXml;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var fileInfo = new FileInfo("test.xlsx");

            //using (var excelPackage = new ExcelPackage(fileInfo))
            {
                //var excelWorksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();

                //var excelFileName = Path.GetFileNameWithoutExtension(excelPackage.File.Name);

                var excelFileName = "test";

                StringBuilder sb = new StringBuilder(), sb2 = new StringBuilder();
                sb.AppendFormat("public class {0} {{", excelFileName);

                var i = 1;

                var arr = new List<List<string>>
                {
                    new List<string>{
                        "Name|string",
                        "Age|int",
                        "Address|string",
                        null
                    },
                    new List<string>
                    {
                        "Lily",
                        "13",
                        "GZRoad",
                        null
                    },
                    new List<string>
                    {
                        "Tom",
                        "23",
                        "NJRoad",
                        null
                    },
                    new List<string>
                    {
                        null,
                        null,
                        null,
                        null
                    }
                };

                while (true)
                {
                    //var currentElement = excelWorksheet.Cells[1, i].Value;
                    var currentElement = arr[0][i - 1];

                    if (currentElement == null)
                    {
                        break;
                    }

                    var temp = currentElement.ToString().Split('|');
                    sb.AppendFormat("\tpublic {0} {1};\n", temp[1], temp[0]);
                    i++;
                }

                sb.AppendLine("}");

                // use CodeDom load the class
                var codeDomProvider = new CSharpCodeProvider(new Dictionary<string, string> { { "CompilerVersion", "v3.5" } });
                var compilerResults = codeDomProvider.CompileAssemblyFromSource(
                    new CompilerParameters {
                        GenerateInMemory = true,
                        ReferencedAssemblies = { "mscorlib.dll", "System.dll", "System.Core.dll"}
                    },
                    new string [] {
                        sb.ToString()
                    }
                );

                var type = compilerResults.CompiledAssembly.GetTypes().FirstOrDefault();
                var list = new List<object>();

                i = 2;

                while (true)
                {
                    //if (excelWorksheet.Cells[i, 1].Value == null)
                    if (arr[i - 1][0] == null)
                    {
                        break;
                    }

                    var dynamicObject = Activator.CreateInstance(type);
                    var fields = dynamicObject.GetType().GetFields();

                    for(var field_index = 0; field_index < fields.Length; field_index++)
                    {
                        var currentElement = arr[i - 1][field_index];

                        var value = Convert.ChangeType(currentElement, fields[field_index].FieldType);

                        fields[field_index].SetValue(dynamicObject, value);
                    }

                    list.Add(dynamicObject);

                    i++;
                }

                sb2.AppendLine("using System.Collection.Generic;");
                //sb2.AppendFormat("public class {0}_TableData {{", Path.GetFileNameWithoutExtension(excelPackage.File.Name));
                //sb2.AppendFormat("\tpublic static List<{0}> {1}_List;\n", type.FullName, Path.GetFileNameWithoutExtension(excelPackage.File.Name));
                sb2.AppendFormat("public class {0}_TableData {{", "test");
                sb2.AppendFormat("\tpublic static List<{0}> {1}_List;\n", type.FullName, "test");
                sb2.AppendLine("}");

                File.WriteAllText(excelFileName + "_TableData.cs", sb2.ToString());
                File.WriteAllText(excelFileName + ".cs", sb.ToString());
                File.WriteAllText(excelFileName + ".json", JsonConvert.SerializeObject(list));
            }
        }
    }
}
