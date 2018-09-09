using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : UIScript
{
    [SerializeField]
    Sprite[] map_sprites;
    [SerializeField]
    GameObject tree_prefab, little_flower_bed_prefab;

    Land[,] lands;

    protected override void Start()
    {
        base.Start();

        lands = fis.GetLands();
        SetGrid();
    }

    public Land GetLand(int x, int y)
    {
        return lands[x, y];
    }

    void SetGrid()
    {
        Texture2D texture = new Texture2D(1024, 1024);
        transform.Find("Map_Grid").GetComponent<Renderer>().material.mainTexture = texture;

        for (int height = 0; height < lands.GetLength(1); height++)
        {
            for (int width = 0; width < lands.GetLength(0); width++)
            {
                Land land = lands[width, height];

                if(land.type == LandType.Tree)
                {
                    GameObject instance = Instantiate(tree_prefab, transform);
                    instance.transform.position = new Vector3(land.pos.x + 0.5f, 0.59f, land.pos.y + 0.5f);
                    instance.transform.rotation = Quaternion.Euler(-90.0f, Random.value * 360.0f, -90.0f);
                }
                else if(land.type == LandType.LittleFlowerBed)
                {
                    GameObject instance = Instantiate(little_flower_bed_prefab, transform);
                    instance.transform.position = new Vector3(land.pos.x + 0.5f, 0, land.pos.y + 0.5f);
                    instance.transform.rotation = Quaternion.Euler(0, Random.Range(0, 4) * 90.0f, 0);
                }

                for (int y = height * 32; y < (height + 1) * 32; y++)
                {
                    for (int x = width * 32; x < (width + 1) * 32; x++)
                    {
                        if (x % 32 == 0 || y % 32 == 0 || x % 32 == 31 || y % 32 == 31)
                        {
                            //texture.SetPixel(x, y, Color.black);
                            Sprite sprite = map_sprites[(int)land.type - 1];
                            texture.SetPixel(x, y, sprite.texture.GetPixel((int)sprite.rect.x + x % 32, (int)sprite.rect.y + y % 32));
                        }
                        else
                        {
                            Sprite sprite = map_sprites[(int)land.type - 1];
                            texture.SetPixel(x, y, sprite.texture.GetPixel((int)sprite.rect.x + x % 32, (int)sprite.rect.y + y % 32));
                        }
                    }
                }
            }
        }

        texture.Apply();
    }
}