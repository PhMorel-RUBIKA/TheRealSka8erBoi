using System.Collections;
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
        
        [Header("Attributs")]
        public GameObject player;
        public Animator transition;
        public float transitionTime;

        public static LoadSceneManager instance;
        public GameObject nextItemToSpawn;

        public bool canChangeRoom;

        public void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            if (instance == null) instance = this;
            numberOfRoom = 0;
        }

        public void Start()
        {
            GetRandomNumber();
            CreateFinalList();
            canChangeRoom = true;
        }

        private void Update()
        {
           if (Input.GetKey(KeyCode.O) && Input.GetKeyDown(KeyCode.P))
           {
                transition.SetTrigger("Start");
                StartCoroutine(ChangeRoom());
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
            finalList.Add(utilityRoom[4]);

            finalList.Add(roomS2[randomIndex[2]]);
            finalList.Add(roomS2[randomIndex[1]]);
            finalList.Add(roomS2[randomIndex[0]]);
        
            finalList.Add(utilityRoom[4]);
            finalList.Add(utilityRoom[1]);
            finalList.Add(utilityRoom[2]);
        }

        public IEnumerator ChangeRoom()
        {
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(finalList[numberOfRoom]);
            player.transform.position = new Vector3(0,0,0);
            transition.SetTrigger("Stop");
            
            numberOfRoom++;
            canChangeRoom = true;
            if (numberOfRoom == 11) ResetProcedural();
        }

        public void ResetProcedural()
        {
            finalList = new List<string>();
            randomIndex = new List<int>();
            numberOfRoom = 0;
        
            GetRandomNumber();
            CreateFinalList(); 
        }
}