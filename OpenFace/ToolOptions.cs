using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenFace
{
    public class ToolOptions
    {
        private static Dictionary<String, Dictionary<String, List<Dictionary<String,String>>>> map;
        public static void Load() {
            string contents = File.ReadAllText(Constants.OPTIONS_PATH);
            map = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<String, Dictionary<String, List<Dictionary<String, String>>>>>(contents);
        }

        public static Color[] GetLipStickColors(string brand) {
            Dictionary<String, List<Dictionary<String, String>>> brands;
            List<Dictionary<String, String>> obj;
            map.TryGetValue("lip_stick_solid", out brands);
            brands.TryGetValue(brand, out obj);
            //List<String> colors =(List<String>) obj;
            Color[] result = new Color[obj.Count];
            for(int i=0;i<obj.Count; i++) {
                String val;
                obj[i].TryGetValue("color", out val);
                result[i] = ColorTranslator.FromHtml(val);
            }
            return result;
        }
    }
}
