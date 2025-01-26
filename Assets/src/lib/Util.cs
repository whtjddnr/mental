using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class Util {
    public static T getJson<T>(string path) {
        return JsonConvert.DeserializeObject<T>(Resources.Load<TextAsset>(path).text);
    }
}
