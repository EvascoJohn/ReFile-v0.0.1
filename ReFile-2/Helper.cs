using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ReFile_2
{
    internal class Helper
    {
        private List<string> existing_extension = new List<string>();
        private static Dictionary<string, string> foldernames = new Dictionary<string, string>();
        private static Dictionary<string, List<string>> dict_of_filetypes = new Dictionary<string, List<string>>()
        {
            {"pdf", new List<string>()}, {"docx", new List<string>()}, {"jpeg", new List<string>()}, {"jpg", new List<string>()},
            {"mp3", new List<string>()}, {"mp4", new List<string>()}, {"gif", new List<string>()}, {"svg", new List<string>()},
            {"png", new List<string>()}, {"tiff", new List<string>()}, {"tif", new List<string>()}, {"doc", new List<string>()},
            {"xls", new List<string>()}, {"xlsx", new List<string>()}, {"txt", new List<string>()}, {"avi", new List<string>()},
            {"mov", new List<string>()}, {"flv", new List<string>()}, {"avchd", new List<string>()}, {"ppt", new List<string>()},
            {"pptx", new List<string>()}, {"odp", new List<string>()}, {"key", new List<string>()}, {"mpa", new List<string>()},
            {"wav", new List<string>()},  {"exe", new List<string>()}
        };


        // Returns a string array of distinct extensions from a list of files.
        public string[] get_existing_extensions()
        {
            return this.existing_extension.Distinct().ToArray();
        }


        // returns a string of array from the keys that have List values count more than 0.
        public string[] get_populated_filetypes_from_dictionary()
        {
            List<string> temp_list = new List<string>();
            string[] temp_existing_extensions = get_existing_extensions();
            if (temp_existing_extensions.Count() > 0)
            {
                foreach (string extension in temp_existing_extensions)
                {
                    temp_list.Add(extension);
                }
            }
            return temp_list.ToArray();
        }


        // returns all the extensions from the dictionary having a "." in front ex. (.pdf).
        public string[] get_extensions_from_dict(Dictionary<string, List<string>> dictionary)
        {
            List<string> temp_extensions_list = new List<string>();
            foreach (KeyValuePair<string, List<string>> entry in dictionary)
            {
                string temp_entry_value = "." + entry.Key;
                temp_extensions_list.Add(temp_entry_value);
            }
            return temp_extensions_list.Distinct().ToArray();
        }


        // assigns every file from the path to the dictionary.
        public void arrange_files_to_dict(string path)
        {
            string[] temp_ext_array = get_extensions_from_dict(dict_of_filetypes);
            var scanned = Directory.EnumerateFiles(path, "*").Where(
                file => temp_ext_array.Any(ext => ext == Path.GetExtension(file))
                );
            foreach (string file in scanned)
            {
                string temp_file_extension = Path.GetExtension(file).Split('.')[1];
                if (dict_of_filetypes.ContainsKey(temp_file_extension)) {
                    existing_extension.Add(temp_file_extension);
                    dict_of_filetypes[temp_file_extension].Add(file);
                }
            }
        }


        // creates folder on the given path.
        public void create_folders(string path)
        {
            foreach (string filetype in get_populated_filetypes_from_dictionary())
            {
                string folder_name = filetype + " folder";
                string folder_path = path + "\\" + folder_name;
                try
                {
                    Directory.CreateDirectory(folder_path);
                }
                catch
                {
                }
                foldernames[filetype] = folder_path;
            }
        }


        // moves files to a given folder directory.
        public void move_files(string source, string destination, string[] checked_indices)
        {
            foreach(string check_filetypes in checked_indices)
            {
                foreach (string full_file_path in dict_of_filetypes[check_filetypes])
                {
                    string file_name = Path.GetFileName(full_file_path);
                    File.Move(full_file_path, foldernames[check_filetypes] + "\\" + file_name);
                }
            }
        }

    }
}