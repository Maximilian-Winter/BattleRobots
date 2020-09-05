using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class LoadAndSaveFromPlayerPrefs : MonoBehaviour
{
    /// <summary>
    /// Save obj to PlayerPrefs ( in WebGL it will be save to IndexedDB
    /// </summary>
    void Save<T>(string filename, T obj)
    {
        using (var sw = new StringWriter())
        {
            using (var xw = XmlWriter.Create(sw))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(xw, obj);
            }
            PlayerPrefs.SetString(filename, sw.ToString());
            PlayerPrefs.Save();
        }
    }
    /// <summary>
    /// Load obj from PlayerPrefs ( in WebGL it will be save to IndexedDB
    /// </summary>
    T Load<T>(string filename)
    {
        if (!PlayerPrefs.HasKey(filename)) return default(T);

        var xml = PlayerPrefs.GetString(filename);
        using (var sr = new StringReader(xml))
        {
            using (var xw = XmlReader.Create(sr))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(xw);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
