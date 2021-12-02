using System;
using UnityEngine;


[CreateAssetMenu(fileName = "New Text", menuName = "DocText")]
public class ReadMe : ScriptableObject {
    public Texture2D icon;
    public float iconMaxWidth = 128f;
    public string title;
    public Section[] sections;
    public bool loadedLayout;
	
    [Serializable]
    public class Section {
        public string heading, text, linkText, url;
    }
}
