using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MRR.Controller
{
    public class MrrPromterController : MonoBehaviour
    {
        public string fileName = "prompterPages.txt";
        private string path;
        private string[] pages;
        private int pageIndex = 0;

        private Text text;

        // Start is called before the first frame update
        void Start()
        {
            text = this.gameObject.GetComponent<Text>();
            path = Application.persistentDataPath;
            LoadText();
            DispayPage(pageIndex);
        }

        private void DispayPage(int index)
        {
            if (index < pages.Length)
                text.text = pages[index];
            else if (index == pages.Length)
            {
                pageIndex = 0;
                text.text = pages[pageIndex];
            }
        }

        public void DisplayNextPage()
        {
            DispayPage(++pageIndex);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                Reload();
        }

        private void Reload()
        {
            LoadText();
            pageIndex = 0;
            DispayPage(pageIndex);
        }

        private void LoadText()
        {
            if (File.Exists(path + "/" + fileName))
                pages = File.ReadAllLines(path + "/" + fileName);
            else
                Debug.LogError("Prompter pages file not found at path = " + path);
        }
    }
}


