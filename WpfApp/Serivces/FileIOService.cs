using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;
using WpfApp.Models;

namespace WpfApp.Serivces
{
    class FileIOService(string Path)
    {
        public BindingList<TodoModel> LoadData()
        {
            var fileExists = File.Exists(Path);
            if (!fileExists)
            {
                File.Create(Path).Dispose();
                return new BindingList<TodoModel>();
            }

            using (var reader = File.OpenText(Path))
            {
                var fileText = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<TodoModel>>(fileText);
            }
        }

        public void SaveData(object todoDataList)
        {
            using (StreamWriter writer = File.CreateText(Path))
            {
                string output = JsonConvert.SerializeObject(todoDataList);
                writer.WriteLine(output);
            }
        }
    }
}
