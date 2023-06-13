using UnityEngine;
public class Rotator : MonoBehaviour
{
    public float speed = 100;
    public bool setHeightManual;
    public bool finishRotator;
    public int height;
    public GameObject[] model;
    public GameObject[] modelPrefab = new GameObject[4];
    public GameObject winPrefab;
    public Material plateMat, baseMat;
    public MeshRenderer playerMesh;
    private GameObject temp1, temp2;
    private void Start()
    {
            //playerMesh = FindObjectOfType<Player>().GetComponent();
            float i = 0;
            plateMat.color = Random.ColorHSV(0, 1, 0.5f, 1, 1, 1);
            baseMat.color = plateMat.color + Color.gray;
            //playerMesh.material.color = plateMat.color;

            int currentLevel = LevelSpawner.level;

            ModelSelection();
            float random = Random.value;

            if (setHeightManual)
                height /= -2;
            else
                height = -currentLevel - LevelSpawner.addOn;
            
            for (i = 0; i > height; i -= 0.5f)
            {
                if (currentLevel  <= 20)
                    temp1 = Instantiate(modelPrefab[Random.Range(0, 2)]);
                if(currentLevel  > 20 && currentLevel <= 50)
                    temp1 = Instantiate(modelPrefab[Random.Range(1, 3)]);
                if(currentLevel  > 50 && currentLevel <= 100)
                    temp1 = Instantiate(modelPrefab[Random.Range(2, 4)]);
                if(currentLevel  > 100)
                    temp1 = Instantiate(modelPrefab[Random.Range(3, 4)]);

                temp1.transform.position = new Vector3(transform.position.x, transform.position.y + i - 0.01f, transform.position.z);
                temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

                if(Mathf.Abs(i) >= currentLevel * .3f && Mathf.Abs(i) <= currentLevel * .6f)
                {
                    temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);
                    temp1.transform.eulerAngles += Vector3.up * 180;
                }else if(Mathf.Abs(i) >= currentLevel * .8f)
                {
                    temp1.transform.eulerAngles = new Vector3(0, i * 8, 0);

                    if(random > .75f)
                        temp1.transform.eulerAngles += Vector3.up * 180;
                }

                temp1.transform.parent = transform;

            }

            if (finishRotator)
            {
                temp2 = Instantiate(winPrefab);
                temp2.transform.position = new Vector3(transform.position.x, transform.position.y  + i - 0.01f, transform.position.z);
            }
    }
    void Update()
    {
        transform.Rotate(new Vector3(0, speed * Time.deltaTime, 0));
    }
    void ModelSelection()
    {
        int randomModel = Random.Range(0, 5);

        switch (randomModel)
        {
            case 0:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i];
                break;

            case 1:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 4];
                break;

            case 2:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 8];
                break;

            case 3:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 12];
                break;

            case 4:
                for (int i = 0; i < 4; i++)
                    modelPrefab[i] = model[i + 16];
                break;
        }
    }
}
