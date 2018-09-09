﻿using System.Collections.Generic;

public class Layer
{
    public List<int> data { get; set; }
    public int height { get; set; }
    public string name { get; set; }
    public double opacity { get; set; }
    public string type { get; set; }
    public bool visible { get; set; }
    public int width { get; set; }
    public int x { get; set; }
    public int y { get; set; }
}

public class Tileset
{
    public int firstgid { get; set; }
    public string source { get; set; }
}

public class RootObject
{
    public int height { get; set; }
    public List<Layer> layers { get; set; }
    public int nextobjectid { get; set; }
    public string orientation { get; set; }
    public string renderorder { get; set; }
    public string tiledversion { get; set; }
    public int tileheight { get; set; }
    public List<Tileset> tilesets { get; set; }
    public int tilewidth { get; set; }
    public string type { get; set; }
    public int version { get; set; }
    public int width { get; set; }
}