using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LoadSceneManager : MonoBehaviour
{
        [HideInInspector] public int numberOfRoom;
        [HideInInspector] public List<int> randomIndex = new List<int>();
    
        [Header("Liste pour la Génération des Salles")]
        public List<string> roomS1 = new List<string>(); 
        public List<string> roomS2 = new List<string>();
        public List<string> utilityRoom = new List<string>();
    
        [Header("Liste des Salles pour cette partie")]
        public List<string> finalList = new List<string>(); 

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            numberOfRoom = 0;
        }

        public void Start()
        {
            GetRandomNumber();
            CreateFinalList();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                ChangeRoom();
            }
        }

        void GetRandomNumber()
        {
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, 7);
                if (!randomIndex.Contains(index))
                {
                    randomIndex.Add(index);
                }
                else
                {
                    i--;
                }
            }
        }

        void CreateFinalList()
        {
            finalList.Add(roomS1[randomIndex[0]]);
            finalList.Add(roomS1[randomIndex[1]]);
            finalList.Add(roomS1[randomIndex[2]]);
        
            finalList.Add(utilityRoom[0]);
            finalList.Add(utilityRoom[1]);
        
            finalList.Add(roomS2[randomIndex[2]]);
            finalList.Add(roomS2[randomIndex[1]]);
            finalList.Add(roomS2[randomIndex[0]]);
        
            finalList.Add(utilityRoom[0]);
            finalList.Add(utilityRoom[2]);
            finalList.Add(utilityRoom[3]);
        }

        void ChangeRoom()
        { 
            SceneManager.LoadSceneAsync(finalList[numberOfRoom]);
            numberOfRoom++;

            if (numberOfRoom == 11)
            {
                ResetProcedural();
            }
        }

        void ResetProcedural()
        {
            finalList = new List<string>();
            randomIndex = new List<int>();
            numberOfRoom = 0;
        
            GetRandomNumber();
            CreateFinalList();
        }
}