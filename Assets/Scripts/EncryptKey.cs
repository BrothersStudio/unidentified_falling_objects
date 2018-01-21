using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EncryptKey : MonoBehaviour
{
	void Start ()
    {
        TextAsset key = Resources.Load("key") as TextAsset;
        string encrypted_key = Encrypt.EncryptString(key.text, "ICARUS");
        StreamWriter cache = new StreamWriter("key.txt");
        cache.Write(encrypted_key);
        cache.Close();
    }
}
